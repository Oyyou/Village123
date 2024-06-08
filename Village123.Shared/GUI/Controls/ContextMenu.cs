using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Models;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ContextMenu : Control
  {
    private float _timer = 0f;
    private bool _closing = false;
    private Texture2D _backgroundTexture;

    public bool Closed { get; private set; }

    public override int Width => _backgroundTexture != null ? _backgroundTexture.Width : 0;

    public override int Height => _backgroundTexture != null ? _backgroundTexture.Height : 0;

    public ContextMenu(ContextMenuModel model)
      : base()
    {
      Position = model.Position;

      ClickableComponent.IsClickable = () => !Closed;
      ClickableComponent.OnClickedOutside = () =>
      {
        _isEnabled = false;
        _closing = true;
      };

      Layer = 0.7f;

      var font = BaseGame.GWM.GameModel.Content.Load<SpriteFont>("Font");
      var texture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        100,
        20,
        Color.White,
        Color.Black,
        1
      );

      AddChild(new Label(font, model.Name, new Vector2(10, 10)) { IsVisible = true });

      var y = 30;
      foreach (var item in model.Items)
      {
        AddChild(new Button(font, texture, item.Label, new Vector2(10, y))
        {
          IsVisible = false,
          OnClicked = () =>
          {
            item.OnClick();
            _closing = true;
          }
        });

        y += texture.Height + 5;
      }

      UpdateBackgroundTexture();
    }

    public override void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer > (_closing ? 0.03f : 0.05f))
      {
        if (_closing)
        {
          foreach (var child in Children.OrderByDescending(c => c.ClickRectangle.Top))
          {
            if (child.IsVisible)
            {
              child.IsVisible = false;
              UpdateBackgroundTexture();
              break;
            }
          }

          if (Children.Where(c => c.IsVisible).Count() <= 1)
          {
            Closed = true;
          }
        }
        else
        {
          foreach (var child in Children.OrderBy(c => c.ClickRectangle.Top))
          {
            if (!child.IsVisible)
            {
              child.IsVisible = true;
              UpdateBackgroundTexture();
              break;
            }
          }
        }

        _timer = 0f;
      }

      base.Update(gameTime);
    }

    private void UpdateBackgroundTexture()
    {
      var childRectangles = Children
        .Where(c => c.IsVisible)
        .Select(c => c.ClickRectangle)
        .ToList();

      if (childRectangles == null || childRectangles.Count == 0)
      {
        _backgroundTexture = null;
        return;
      }

      var bounds = childRectangles[0];

      foreach (var rect in childRectangles)
      {
        int minX = Math.Min(bounds.Left, rect.Left);
        int minY = Math.Min(bounds.Top, rect.Top);

        int maxX = Math.Max(bounds.Right, rect.Right);
        int maxY = Math.Max(bounds.Bottom, rect.Bottom);

        bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
      }

      _backgroundTexture = TextureHelpers.CreateBorderedTexture(
          BaseGame.GWM.GameModel.GraphicsDevice,
          bounds.Width + 20,
          bounds.Height + 20,
          Color.White,
          Color.Black,
          1
        );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

      base.Draw(spriteBatch);
      if (_backgroundTexture != null)
      {
        spriteBatch.Draw(
          _backgroundTexture,
          DrawPosition,
          null,
          Color.White,
          0f,
          new Vector2(0, 0),
          1f,
          SpriteEffects.None,
          ClickableComponent.ClickLayer()
        );
      }

      spriteBatch.End();
    }
  }
}
