using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using Village123.Shared.Data;

namespace Village123.Shared.Entities
{
  public class Item : IEntity
  {
    private int _craftPercentage = 0;

    public int Id { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// The villager holding the item
    /// </summary>
    public int? VillagerId { get; set; }

    /// <summary>
    /// The storage containing the item
    /// </summary>
    public int? StorageId { get; set; }
    public Point Point { get; set; }

    [JsonIgnore]
    public Vector2 Position => Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    [JsonIgnore]
    public ItemData.Item Data { get; private set; }

    public Item() { }

    public Item(ItemData.Item data, Texture2D texture, Point point)
    {
      Texture = texture;
      Point = point;

      Name = Path.GetFileName(Texture.Name);

      SetData(data);
    }

    public void SetData(ItemData.Item data)
    {
      Data = data;
    }

    public void DrawInInventory(SpriteBatch spriteBatch, Vector2 position)
    {
      spriteBatch.Draw(Texture, position, Color.White);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (VillagerId.HasValue || StorageId.HasValue)
      {
        return;
      }

      spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
    }
  }
}
