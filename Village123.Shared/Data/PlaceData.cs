using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;

namespace Village123.Shared.Data
{
  public class PlaceData : IPersistable<VillagerData>
  {
    private const string fileName = "places.json";

    private List<Place> _places = new();

    public List<Place> Places
    {
      get => _places;
      set
      {
        _places = value;
      }
    }

    public void Add(Place place)
    {
      _places.Add(place);
    }

    public void Save()
    {
      var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(fileName, jsonString);
    }

    public static PlaceData Load(GameWorld gameWorld)
    {
      var placeData = new PlaceData();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        placeData = JsonConvert.DeserializeObject<PlaceData>(jsonString)!;
      }

      foreach (var place in placeData.Places)
      {
        place.Texture = gameWorld.Content.Load<Texture2D>($"Places/{place.Name}");
      }

      return placeData;
    }
  }
  }
