using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Services;
using Village123.Shared.Utils;
using static Village123.Shared.Data.ResourceData;

namespace Village123.Shared.Managers
{
  public class PlaceManager
  {
    private const string fileName = "places.json";
    private SaveFileService _saveFileService;

    public List<Place> Places { get; private set; } = new();

    private PlaceManager() { }

    public PlaceManager(SaveFileService saveFileService)
    {
      _saveFileService = saveFileService;
    }

    #region Serialization
    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static PlaceManager Load(SaveFileService saveFileService)
    {
      var manager = saveFileService.Load<PlaceManager>(fileName);

      if (manager == null)
      {
        manager = new PlaceManager(saveFileService);
      }
      else
      {
        manager._saveFileService = saveFileService;
      }

      foreach (var place in manager.Places)
      {
        place.SetData(BaseGame.GWM.PlaceData.Places[place.Key]);
        place.Texture = TextureHelpers.LoadTexture($"Places/{place.Key}");
      }

      return manager;
    }
    #endregion

    public void UpdateMouse()
    {
      foreach (var place in Places)
      {
        // place.ClickableComponent.Camera = matrix;
        place.ClickableComponent.Update();
      }
    }

    public void Update(GameTime gameTime)
    {
      foreach (var place in Places)
      {
        place.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var place in Places)
      {
        place.Draw(spriteBatch);
      }
    }

    public Place Add(PlaceData.Place data, Point point)
    {
      var texturePath = $"Places/{data.Key}";

      if (data.Type == "building")
      {
        texturePath = $"Places/{data.Key}/{data.Key}_front";
      }

      var id = BaseGame.GWM.IdManager.PlaceId++;
      var place = new Place(data, TextureHelpers.LoadTexture(texturePath), point)
      {
        Id = id,
        Name = $"{data.Name} {id}",
      };

      Places.Add(place);
      if (data.Type == "building")
      {
        BaseGame.GWM.Map.AddObstacle(point + data.Offset, data.Size, Point.Zero);
      }
      else
      {
        BaseGame.GWM.Map.AddEntity(point + data.Offset, place.Data.Size);
      }

      return place;
    }

    public void Destroy(Place place)
    {
      place.StartDestruction();
      // TODO: Add destruction job
    }

    public void CancelDestruction(Place place)
    {
      place.CancelDestruction();
      // TODO: Remove destruction job
    }

    public IEnumerable<Place> GetPlacesByType(string type) => Places.Where(p => p.Data.Type == type);
  }
}
