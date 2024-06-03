﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;

namespace Village123.Shared.Managers
{
  public class PlaceManager
  {
    private static PlaceManager _instance;
    private static readonly object _lock = new();

    private const string fileName = "places.json";
    private GameWorldManager _gwm;
    public List<Place> Places { get; private set; } = new();

    private PlaceManager() { }

    public static PlaceManager GetInstance(GameWorldManager gwm)
    {
      lock (_lock)
      {
        _instance ??= Load(gwm);
      }

      return _instance;
    }

    #region Serialization
    public void Save()
    {
      var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(fileName, jsonString);
    }

    private static PlaceManager Load(GameWorldManager gwm)
    {
      var manager = new PlaceManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        manager = JsonConvert.DeserializeObject<PlaceManager>(jsonString)!;
      }

      manager._gwm = gwm;

      foreach (var place in manager.Places)
      {
        place.SetData(gwm.PlaceData.Places[place.Key]);
        place.Texture = gwm.GameModel.Content.Load<Texture2D>($"Places/{place.Key}");
      }

      return manager;
    }
    #endregion

    public void Update(GameTime gameTime)
    {
      foreach (var place in Places)
      {
        place.Update(_gwm, gameTime);
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
      var id = _gwm.IdManager.PlaceId++;
      var place = new Place(data, _gwm.GameModel.Content.Load<Texture2D>($"Places/{data.Key}"), point)
      {
        Id = id,
        Name = $"{data.Name} {id}",
      };

      Places.Add(place);

      _gwm.Map.Add(point, place.Data.Size);

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
