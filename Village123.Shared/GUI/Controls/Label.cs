using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Village123.Shared.GUI.Controls.Bases;

namespace Village123.Shared.GUI.Controls
{
  public class Label : Control
  {
    private readonly SpriteFont _font;

    public override int Width => (int)_font.MeasureString(Text).X;
    public override int Height => (int)_font.MeasureString(Text).Y;

    public Color TextColor { get; set; } = Color.Black;
    public string Text { get; set; }

    public Label(SpriteFont font, string text, Vector2 position) : base()
    {
      _font = font;
      Text = text;
      Position = position;

      ClickableComponent.IsClickable = () => false;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      spriteBatch.DrawString(
        _font,
        Text,
        DrawPosition,
        TextColor,
        0f,
        new Vector2(0, 0),
        1f,
        SpriteEffects.None,
        ClickableComponent.ClickLayer()
      );
    }
  }
}
