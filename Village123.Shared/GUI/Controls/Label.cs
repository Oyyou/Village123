using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Village123.Shared.GUI.Controls.Bases;

namespace Village123.Shared.GUI.Controls
{
  public class Label : Control
  {
    public readonly string Text;
    private readonly SpriteFont _font;

    protected override bool IsClickable => false;

    public override int Width => _font != null ? (int)_font.MeasureString(Text).X : 0;
    public override int Height => _font != null ? (int)_font.MeasureString(Text).Y : 0;

    public override Action OnLeftClick => () => { };
    public override Action OnMouseOver => () => { };
    public override Action OnLeftClickOutside => () => { };

    public Color TextColor { get; set; } = Color.Black;

    public Label(SpriteFont font, string text, Vector2 position) : base(position)
    {
      _font = font;
      Text = text;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      spriteBatch.DrawString(_font, Text, Position, TextColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer);
    }
  }
}
