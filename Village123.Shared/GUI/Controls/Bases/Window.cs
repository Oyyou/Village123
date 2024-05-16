﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Bases
{
  public abstract class Window : Control
  {
    protected readonly Texture2D _windowTexture;
    protected readonly SpriteFont _font;
    protected readonly string _title;

    protected bool _isOpen = true;

    public override bool ClickIsVisible => _isOpen;

    public override int Width => _windowTexture != null ? _windowTexture.Width : 0;

    public override int Height => _windowTexture != null ? _windowTexture.Height : 0;

    public Window(GameWorldManager gwm, Vector2 position, int width, int height, string title)
      : base(position)
    {
      _title = title;

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
    }

    private Button GetCloseButton(GameWorldManager gwm)
    {
      var closeButtonTexture = TextureHelpers.CreateBorderedTexture(gwm.GameModel.GraphicsDevice, 30, 30, Color.White, Color.Black, 1);
      var padding = 10;
      var button = new Button(
        _font,
        closeButtonTexture,
        "X",
        new Vector2(this.ClickRectangle.Right - (closeButtonTexture.Width + padding), this.ClickRectangle.Top + padding)
      )
      {
        OnClicked = () => _isOpen = false,
      };

      return button;
    }

    public override void Update(GameTime gameTime)
    {
      if (!_isOpen)
      {
        return;
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!_isOpen)
      {
        return;
      }

      base.Draw(spriteBatch);

      spriteBatch.Draw(_windowTexture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer);
      spriteBatch.DrawString(_font, _title, Position + new Vector2(10, 10), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer + 0.05f);
    }
  }
}
