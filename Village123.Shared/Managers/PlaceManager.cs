using Microsoft.Xna.Framework;
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
    private const string fileName = "places.json";

    private GameWorldManager _gwm;

    public List<Place> Places { get; private set; } = new();

    public PlaceManager()
    {

    }

    private void Initialize(GameWorldManager gwm)
    {
      _gwm = gwm;
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

    public static PlaceManager Load(GameWorldManager gwm)
    {
      var placeManager = new PlaceManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        placeManager = JsonConvert.DeserializeObject<PlaceManager>(jsonString)!;
      }

      placeManager.Initialize(gwm);

      foreach (var place in placeManager.Places)
      {
        place.Texture = gwm.GameModel.Content.Load<Texture2D>($"Places/{place.Name}");
      }

      return placeManager;
    }
    #endregion

    public void Update()
    {
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
      var place = new Place(_gwm.GameModel.Content.Load<Texture2D>($"Places/{data.Name}"), point)
      {
        Id = _gwm.IdManager.PlaceId++,
      };

      Places.Add(place);

      return place;
    }
  }
}
