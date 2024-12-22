using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Village123.Shared.Sprites
{
  public class Sprite
  {
    public readonly Texture2D Texture;

    public Vector2 Position { get; set; }

    public Color Colour { get; set; } = Color.White;

    public float Opacity { get; set; } = 1f;

    public float Scale { get; set; } = 1f;

    public float Layer { get; set; } = 1f;

    public float Width => Texture.Width * Scale;
    public float Height => Texture.Height * Scale;

    public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);

    public Sprite(Texture2D texture)
    {
      Texture = texture;
    }

    public Sprite(Texture2D texture, Vector2 position)
    {
      Texture = texture;
      Position = position;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, null, Colour * Opacity, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, Layer);
    }
  }
}
