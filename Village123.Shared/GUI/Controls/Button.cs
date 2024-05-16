using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Input;

namespace Village123.Shared.GUI.Controls
{
  public class Button : Control
  {
    private Vector2 _textPosition;
    private bool _isMouseOver = false;

    public string Text { get; set; }
    public SpriteFont Font { get; init; }
    public Texture2D Texture { get; init; }

    public Color BackgroundColor { get; set; } = Color.White;
    public Color TextColor { get; set; } = Color.Black;
    public Color HoverBackgroundColor { get; set; } = Color.Gray;

    public override bool ClickIsVisible => true;

    public override int Width => Texture != null ? Texture.Width : 0;

    public override int Height => Texture != null ? Texture.Height : 0;

    public override Action OnLeftClick => OnClicked;

    public override Action OnMouseOver => () => _isMouseOver = true;

    public Button(SpriteFont font, Texture2D texture, string text, Vector2 position) : base(position)
    {
      Font = font;
      Texture = texture;

      SetText(text);

      _onPositionChanged = () =>
      {
        UpdateTextPosition();
      };
    }

    public void SetText(string text)
    {
      Text = text;
      UpdateTextPosition();
    }

    private void UpdateTextPosition()
    {
      var textSize = Font.MeasureString(Text);

      var x = (Position.X + (Texture.Width / 2)) - (textSize.X / 2);
      var y = (Position.Y + (Texture.Height / 2)) - (textSize.Y / 2);
      _textPosition = new Vector2(x, y);
    }

    public override void Update(GameTime gameTime)
    {
      _isMouseOver = false;

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      spriteBatch.Draw(Texture, Position, null, _isMouseOver ? HoverBackgroundColor : BackgroundColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer);
      spriteBatch.DrawString(Font, Text, _textPosition, TextColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer + 0.005f);
    }
  }
}
