using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class PlaceOptionsPanel : Control
  {
    private GameWorldManager _gwm;

    private float _timer = 0f;
    private bool _closing = false;
    private Texture2D _backgroundTexture;

    public bool Closed { get; private set; }

    public override int Width => _backgroundTexture != null ? _backgroundTexture.Width : 0;

    public override int Height => _backgroundTexture != null ? _backgroundTexture.Height : 0;

    public PlaceOptionsPanel(GameWorldManager gwm, Place place, Vector2 position)
      : base()
    {
      _gwm = gwm;
      Position = position;

      ClickableComponent.IsClickable = () => !Closed;
      ClickableComponent.OnClickedOutside = () =>
      {
        _isEnabled = false;
        _closing = true;
      };

      Layer = 0.7f;

      var font = gwm.GameModel.Content.Load<SpriteFont>("Font");
      var texture = TextureHelpers.CreateBorderedTexture(
        gwm.GameModel.GraphicsDevice,
        100,
        20,
        Color.White,
        Color.Black,
        1
      );

      if (place.BeingDestroyed)
      {
        AddChild(new Label(font, $"{place.Data.Name} - destroying", new Vector2(10, 10)) { IsVisible = true });
        AddChild(new Button(font, texture, "Cancel", new Vector2(10, 55))
        {
          IsVisible = false,
          OnClicked = () =>
          {
            _closing = true;
            PlaceManager.GetInstance(_gwm).CancelDestruction(place);
          }
        });
      }
      else
      {
        AddChild(new Label(font, place.Name, new Vector2(10, 10)) { IsVisible = true });

        var placeCategory = _gwm.PlaceCategoryData.Categories[place.Data.Category];
        // TODO: Find a better way to determine what windows are opened by the buttons
        if (placeCategory.ButtonLabel == "Craft")
        {
          AddChild(new Button(font, texture, placeCategory.ButtonLabel, new Vector2(10, 30))
          {
            IsVisible = false,
            OnClicked = () =>
            {
              _closing = true;
              _gwm.GUIManager.HandleCraftClicked(place);
            }
          });
        }
        if (placeCategory.ButtonLabel == "Manage")
        {
          AddChild(new Button(font, texture, placeCategory.ButtonLabel, new Vector2(10, 30))
          {
            IsVisible = false,
            OnClicked = () =>
            {
              _closing = true;
              _gwm.GUIManager.HandleManageStorageClicked(place);
            }
          });
        }
        AddChild(new Button(font, texture, "Destroy", new Vector2(10, 55))
        {
          IsVisible = false,
          OnClicked = () =>
          {
            _closing = true;
            PlaceManager.GetInstance(_gwm).Destroy(place);
          }
        });
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
          _gwm.GameModel.GraphicsDevice,
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
