using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Bases
{
  public abstract class Window : Control
  {
    protected readonly GameWorldManager _gwm;
    protected readonly Texture2D _windowTexture;
    protected readonly SpriteFont _font;

    public bool IsOpen = false;

    protected override bool IsClickable => IsOpen;
    public override int Width => _windowTexture != null ? _windowTexture.Width : 0;
    public override int Height => _windowTexture != null ? _windowTexture.Height : 0;
    public override Action OnLeftClickOutside => () => IsOpen = false;

    public Window(GameWorldManager gwm, Vector2 position, int width, int height, string title)
      : base(position)
    {
      _gwm = gwm;

      _windowTexture = TextureHelpers.CreateBorderedTexture(
        gwm.GameModel.GraphicsDevice,
        width,
        height,
        Color.White,
        Color.Black,
        1
      );
      _font = gwm.GameModel.Content.Load<SpriteFont>("Font");

      AddChild(GetCloseButton(gwm));
      AddChild(new Label(_font, title, new Vector2(10, 10)));
    }

    private Button GetCloseButton(GameWorldManager gwm)
    {
      var closeButtonTexture = TextureHelpers.CreateBorderedTexture(gwm.GameModel.GraphicsDevice, 30, 30, Color.White, Color.Black, 1);
      var padding = 10;
      var button = new Button(
        _font,
        closeButtonTexture,
        "X",
        new Vector2(this.ClickRectangle.Width - (closeButtonTexture.Width + padding), padding)
      )
      {
        OnClicked = () => IsOpen = false,
      };

      return button;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      spriteBatch.Draw(_windowTexture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer);
    }
  }
}
