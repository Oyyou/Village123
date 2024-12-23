using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Components;
using Village123.Shared.Data;
using Village123.Shared.Maps;
using Village123.Shared.Utils;

namespace Village123.Shared.Entities
{
  public class Place : IEntity
  {
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private float _destroyTimer = 0f;

    [JsonIgnore]
    private Texture2D _hoverBoarder;
    private bool _showBorder;

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

    [JsonIgnore]
    public Map InternalMap { get; set; } = null;

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
          (int)Position.X + (Data.Offset.X * BaseGame.TileSize),
          (int)Position.Y + (Data.Offset.Y * BaseGame.TileSize),
          Data.Size.X * BaseGame.TileSize,
          Data.Size.Y * BaseGame.TileSize
        ),
        OnClicked = () =>
        {
          BaseGame.GWM.PlaceManager.EnterBuilding(this);
          // BaseGame.GWM.GUIManager.HandlePlaceClicked(this);
        },
        OnHover = () => Opacity = 0.75f,
        OnMouseLeave = () => Opacity = 1,
      };

      _hoverBoarder = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        this.Data.Size.X * BaseGame.TileSize,
        this.Data.Size.Y * BaseGame.TileSize,
        Color.White * 0,
        Color.Yellow,
        2);

      if (this.Data.InternalMap != null)
      {
        this.InternalMap = new Map(this.Data.InternalMap);
      }
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      var point = this.Point; // Map position (top-left corner of the object)
      var size = this.Data.Size; // Size of the collisionable area (width & height in tiles)
      var offset = this.Data.Offset; // Offset for visual adjustment

      // Calculate the "base" Y-position for layering
      int baseY = point.Y + size.Y + offset.Y;

      // Normalize to a layer value in [0.0, 1.0]
      var layer = MathHelper.Clamp((float)baseY / BaseGame.GWM.Map.Height, 0.0f, 0.998f);

      if (this.ClickableComponent.IsMouseOver)
      {
        spriteBatch.Draw(_hoverBoarder, Position + (Data.Offset.ToVector2() * BaseGame.TileSize), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.999f);
      }

      spriteBatch.Draw(Texture, Position, null, Colour * Opacity, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, layer);
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
