using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using Village123.Shared.Data;

namespace Village123.Shared.Entities
{
  public class Resource : IEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }
    public Point Point { get; set; }

    [JsonIgnore]
    public Vector2 Position => Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    [JsonIgnore]
    public ResourceData.Resource Data { get; private set; }

    public Resource() { }

    public Resource(int id, ResourceData.Resource data, Texture2D texture, Point point)
    {
      Id = id;
      Texture = texture;
      Point = point;

      Name = Path.GetFileName(Texture.Name);

      SetData(data);
    }

    public void SetData(ResourceData.Resource data)
    {
      Data = data;
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
    }
  }
}
