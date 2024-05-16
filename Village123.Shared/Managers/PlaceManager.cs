﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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
      var placeManager = new PlaceManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        placeManager = JsonConvert.DeserializeObject<PlaceManager>(jsonString)!;
      }

      placeManager._gwm = gwm;

      foreach (var place in placeManager.Places)
      {
        place.SetData(gwm.PlaceData.Places[place.Name]);
        place.Texture = gwm.GameModel.Content.Load<Texture2D>($"Places/{place.Name}");
      }

      return placeManager;
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
      var place = new Place(data, _gwm.GameModel.Content.Load<Texture2D>($"Places/{data.Name}"), point)
      {
        Id = _gwm.IdManager.PlaceId++,
      };

      Places.Add(place);

      return place;
    }
  }
}
