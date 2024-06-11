using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using Village123.Shared.Components;
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

    public Point Point { get; set; }

    [JsonIgnore]
    public Vector2 Position => Carriable.BeingCarried ? Carriable.Position : Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    [JsonIgnore]
    public ItemData.Item Data { get; private set; }

    #region Components
    public CarriableComponent Carriable { get; set; }
    public StorableComponent Storable { get; set; }
    #endregion

    public Item() { }

    public Item(int id, ItemData.Item data, Texture2D texture, Point point)
    {
      Id = id;
      Texture = texture;
      Point = point;

      Name = Path.GetFileName(Texture.Name);

      Carriable = new CarriableComponent(this);
      Storable = new StorableComponent();

      SetData(data);
    }

    public void SetData(ItemData.Item data)
    {
      Data = data;

      Carriable.OnPickup = () =>
      {
        BaseGame.GWM.Map.RemoveEntity(Point, new Point(1, 1));
      };
    }

    public void Update(GameTime gameTime)
    {
      Carriable?.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (VillagerId.HasValue || Storable.IsStored)
      {
        return;
      }

      spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
    }
  }
}
