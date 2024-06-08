using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Components;
using Village123.Shared.Data;

namespace Village123.Shared.Entities
{
  public class Place : IEntity
  {
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private float _destroyTimer = 0f;

    [JsonIgnore]
    public ClickableComponent ClickableComponent { get; protected set; }

    public int Id { get; set; }
    public List<int> OwnerIds { get; set; } = new();
    public string Key { get; set; }
    public string Name { get; set; }
    public Point Point { get; set; }
    public bool BeingDestroyed { get; private set; }
    /// <summary>
    /// Jobs at this place
    /// </summary>
    public List<int> JobIds { get; set; } = new List<int>();

    [JsonIgnore]
    public Vector2 Position => Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public PlaceData.Place Data { get; private set; }

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    public Color Colour { get; set; } = Color.White;
    public float Opacity { get; set; } = 1f;

    public List<Point> RadiusPoints = new();

    public Place() { }

    public Place(PlaceData.Place data, Texture2D texture, Point point)
    {
      SetData(data);

      Texture = texture;
      Point = point;

      Name = Path.GetFileName(Texture.Name);
      Key = data.Key;
    }

    public void SetData(PlaceData.Place data)
    {
      Data = data;

      ClickableComponent = new ClickableComponent()
      {
        ClickRectangle = () => new(
          (int)Position.X,
          (int)Position.Y,
          Data.Size.X * BaseGame.TileSize,
          Data.Size.Y * BaseGame.TileSize
        )
      };
    }

    public void Update(GameTime gameTime)
    {
      ClickableComponent.Update(gameTime);

      if (ClickableComponent.IsMouseClicked)
      {
        BaseGame.GWM.GUIManager.HandlePlaceClicked(this);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, Colour * Opacity);
    }

    public void AddOwner(Villager villager)
    {
      OwnerIds.Add(villager.Id);
    }

    public void StartDestruction()
    {
      _destroyTimer = 0f;
      BeingDestroyed = true;
    }

    public void CancelDestruction()
    {
      _destroyTimer = 0f;
      BeingDestroyed = false;
    }
  }
}
