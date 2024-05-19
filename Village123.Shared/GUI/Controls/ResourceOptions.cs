using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ResourceOptions : Control
  {
    private Texture2D _backgroundTexture;

    #region Overrides
    public override int Width => _backgroundTexture != null ? _backgroundTexture.Width : 0;
    public override int Height => _backgroundTexture != null ? _backgroundTexture.Height : 0;
    public override Action OnLeftClick => () => { };
    public override Action OnMouseOver => () => { };
    public override Action OnLeftClickOutside => () => { };
    #endregion

    public ResourceOptions(GameWorldManager gwm, string resourceType, int width, Vector2 position) : base(position)
    {
      var font = gwm.GameModel.Content.Load<SpriteFont>("Font");
      var buttonTexture = TextureHelpers.CreateBorderedTexture(
        gwm.GameModel.GraphicsDevice,
        100,
        20,
        Color.White,
        Color.Black,
        1
      );

      var titleLabel = new Label(font, $"{resourceType} options", new Vector2(5, 5));
      AddChild(titleLabel);

      var startPosition = new Vector2(5, 5 + titleLabel.Height + 10);

      var resourceOptions = gwm.ResourceData.GetResourcesByType(resourceType);
      for (int i = 0; i < resourceOptions.Count; i++)
      {
        var resource = resourceOptions[i];
        var button = new Button(font, buttonTexture, resource.Name, startPosition);
        AddChild(button);

        startPosition += new Vector2(button.Width + 5, 0);
        if (startPosition.X > width - (button.Width + 5))
        {
          startPosition = new Vector2(5, startPosition.Y + button.Height + 5);
        }
      }
      UpdateBackgroundTexture(gwm, width);
    }

    private void UpdateBackgroundTexture(GameWorldManager gwm, int width)
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
        gwm.GameModel.GraphicsDevice,
        width,
        bounds.Height + 20,
        Color.White,
        Color.Black,
        1
      );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      if (_backgroundTexture != null)
      {
        spriteBatch.Draw(_backgroundTexture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer);
      }
    }
  }
}
