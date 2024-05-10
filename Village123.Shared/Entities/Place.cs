using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Entities
{
  public class Place : IEntity
  {
    public int Id { get; set; }
    public List<int> OwnerIds { get; set; } = new();
    public string Name { get; set; }
    public Point Point { get; set; }

    [JsonIgnore]
    public Vector2 Position => Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    public Place() { }

    public Place(Texture2D texture, Point point)
    {
      Texture = texture;
      Point = point;

      Name = Path.GetFileName(Texture.Name);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, Color.White);
    }

    public void AddOwner(Villager villager)
    {
      OwnerIds.Add(villager.Id);
    }
  }
}
