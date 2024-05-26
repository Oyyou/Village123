using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Village123.Shared.Data;
using Village123.Shared.Interfaces;

namespace Village123.Shared.Entities
{
  public class Item : IEntity, IStorable
  {
    public int Id { get; set; }

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

    [JsonProperty]
    public int StorableId => Id;
    [JsonIgnore]
    public string Name => Data.Name;
    [JsonIgnore]
    public string Type => this.GetType().Name;

    public Item() { }

    public Item(ItemData.Item data, Texture2D texture, Point point)
    {
      Texture = texture;
      Point = point;

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
      if (!VillagerId.HasValue && !StorageId.HasValue)
      {
        return;
      }

      spriteBatch.Draw(Texture, Position, Color.White);
    }
  }
}
