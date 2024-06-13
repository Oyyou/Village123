using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Village123.Shared.Components;
using Village123.Shared.Data;

namespace Village123.Shared.Entities
{
  public class Material : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public Point Point { get; set; }
    public bool IsInUse { get; set; } = false;

    [JsonIgnore]
    public Vector2 Position => Carriable.BeingCarried ? Carriable.Position : Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    [JsonIgnore]
    public MaterialData.Material Data { get; private set; }

    #region Components
    public CarriableComponent Carriable { get; set; }
    public StorableComponent Storable { get; set; }
    #endregion

    public Material() { }

    public Material(int id, MaterialData.Material data, Texture2D texture, Point point)
    {
      Id = id;
      Texture = texture;
      Point = point;

      Name = data.Key;

      Carriable = new CarriableComponent(this);
      Storable = new StorableComponent();

      SetData(data);
    }

    public void SetData(MaterialData.Material data)
    {
      Data = data;

      Carriable.OnPickup = () =>
      {
        if (!Storable.IsStored)
        {
          BaseGame.GWM.Map.RemoveEntity(Point, new Point(1, 1));
        }

        Storable.IsStored = false;
        Storable.PlaceId = null;
      };
      Carriable.OnDrop = (point) =>
      {
        this.Point = point;
      };
    }

    public void Update(GameTime gameTime)
    {
      Carriable?.Update();
    }

    public void DrawInInventory(SpriteBatch spriteBatch, Vector2 position)
    {
      spriteBatch.Draw(Texture, position, Color.White);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (Storable.IsStored)
      {
        return;
      }

      spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
    }
  }
}
