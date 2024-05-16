using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Data;
using Village123.Shared.GUI.Controls;
using Village123.Shared.Input;
using Village123.Shared.Interfaces;
using Village123.Shared.Managers;
using Village123.Shared.Models;

namespace Village123.Shared.Entities
{
  public class Place : IEntity, IClickable
  {
    public int Id { get; set; }
    public List<int> OwnerIds { get; set; } = new();
    public string Name { get; set; }
    public Point Point { get; set; }

    [JsonIgnore]
    public Vector2 Position => Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public PlaceData.Place Data { get; private set; }

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    public Rectangle ClickRectangle => new(
        (int)Position.X,
        (int)Position.Y,
        Data.Size.X * BaseGame.TileSize,
        Data.Size.Y * BaseGame.TileSize
      );

    public float ClickLayer => 0.5f;

    public bool ClickIsVisible => true;

    public Place() { }

    public Place(PlaceData.Place data, Texture2D texture, Point point)
    {
      SetData(data);

      Texture = texture;
      Point = point;

      Name = Path.GetFileName(Texture.Name);
    }

    public void SetData(PlaceData.Place data)
    {
      Data = data;
    }

    public void Update(GameWorldManager gwm, GameTime gameTime)
    {
      var clickRectangle = new Rectangle(
        (int)Position.X,
        (int)Position.Y,
        Data.Size.X * BaseGame.TileSize,
        Data.Size.Y * BaseGame.TileSize
      );

      if (GameMouse.Intersects(clickRectangle))
      {
        GameMouse.AddObject(this);

        if (GameMouse.ValidObject == this)
        {
          if (GameMouse.IsLeftClicked)
          {
            CraftingPanel.GetInstance(gwm).SetPlace(Data);
          }
        }
      }
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
