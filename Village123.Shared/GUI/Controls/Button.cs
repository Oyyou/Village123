using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Sprites;

namespace Village123.Shared.GUI.Controls
{
  public class Button : Control
  {
    private bool _hasUpdated = false;
    private Label _label;

    public Texture2D Texture { get; private set; }

    public Sprite Sprite { get; private set; }

    public Color BackgroundColor { get; set; } = Color.White;
    public Color TextColor
    {
      get => _label.TextColor;
      set => _label.TextColor = value;
    }
    public Color HoverBackgroundColor { get; set; } = Color.Gray;

    public Color SelectedBackgroundColor { get; set; } = Color.Yellow;
    public string Text => _label.Text;
    public override int Width => Texture != null ? Texture.Width : (Sprite != null ? (int)Sprite.Width : 0);

    public override int Height => Texture != null ? Texture.Height : (Sprite != null ? (int)Sprite.Height : 0);

    public Button(Sprite sprite)
      : base()
    {
      Sprite = sprite;
    }

    public Button(Texture2D texture)
      : base()
    {
      Texture = texture;
    }

    public Button(Texture2D texture, Vector2 position)
      : base()
    {
      Texture = texture;
      Position = position;
    }

    public Button(SpriteFont font, Texture2D texture, string text)
      : this(font, texture, text, Vector2.Zero)
    {
    }

    public Button(SpriteFont font, Texture2D texture, string text, Vector2 position)
      : base()
    {
      Texture = texture;
      Position = position;

      _label = new Label(font, text, Vector2.Zero);

      AddChild(_label);
    }

    public void UpdateTexture(Texture2D texture)
    {
      Texture = texture;
    }

    public override void Update(GameTime gameTime)
    {
      if (!IsVisible) return;
      if (!IsEnabled) return;

      _hasUpdated = true;

      if (_label != null)
      {
        _label.Position =
          new Vector2(
            (Width / 2) - (_label.Width / 2),
            (Height / 2) - (_label.Height / 2)
            );
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!_hasUpdated) return;
      if (!IsVisible) return;

      base.Draw(spriteBatch);

      if (Texture != null)
      {
        spriteBatch.Draw(
          Texture,
          DrawPosition,
          null,
          ClickableComponent.IsMouseOver ? HoverBackgroundColor : (IsSelected() ? SelectedBackgroundColor : BackgroundColor),
          0f,
          new Vector2(0, 0),
          1f,
          SpriteEffects.None,
          ClickableComponent.ClickLayer()
        );
      }

      if (Sprite != null)
      {
        Sprite.Position = DrawPosition;
        Sprite.Colour = ClickableComponent.IsMouseOver ? HoverBackgroundColor : (IsSelected() ? SelectedBackgroundColor : BackgroundColor);
        Sprite.Draw(spriteBatch);
      }
    }
  }
}
