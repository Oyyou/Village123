using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Village123.Shared.Input;
using Village123.Shared.Interfaces;

namespace Village123.Shared.GUI.Controls
{
  public class Button : IClickable
  {
    private Vector2 _position;
    private Vector2 _textPosition;
    private Rectangle _clickRectangle;

    private bool _isMouseOver = false;

    public string Text { get; set; }
    public SpriteFont Font { get; init; }
    public Texture2D Texture { get; init; }

    public Vector2 Position
    {
      get => _position;
      set
      {
        _position = value;
        UpdateClickRectangle();
        UpdateTextPosition();
      }
    }

    public Color BackgroundColor { get; set; } = Color.White;
    public Color TextColor { get; set; } = Color.Black;
    public Color HoverBackgroundColor { get; set; } = Color.Gray;

    public Rectangle ClickRectangle => _clickRectangle;

    public float ClickLayer => 1f;

    public bool ClickIsVisible => true;

    public Action OnClicked { get; set; }

    public Button(string text, SpriteFont font, Texture2D texture)
    {
      Font = font;
      Texture = texture;

      UpdateClickRectangle();
      SetText(text);
    }

    private void UpdateClickRectangle()
    {
      _clickRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
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

    public void Update(GameTime gameTime)
    {
      _isMouseOver = false;

      if (GameMouse.Intersects(ClickRectangle))
      {
        GameMouse.AddObject(this);

        // If this control is what the gameMouse is able to currently click (based off what control is layered at the top)
        if (GameMouse.ValidObject == this)
        {
          _isMouseOver = true;
          if (GameMouse.IsLeftClicked)
          {
            OnClicked?.Invoke();
          }
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, _isMouseOver ? HoverBackgroundColor : BackgroundColor);
      spriteBatch.DrawString(Font, Text, _textPosition, TextColor);
    }
  }
}
