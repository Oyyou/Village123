using Microsoft.Xna.Framework;
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
    private const string fileName = "places.json";
    public List<Place> Places { get; private set; } = new();

    private PlaceManager() { }

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

    public static PlaceManager Load()
    {
      var manager = new PlaceManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        manager = JsonConvert.DeserializeObject<PlaceManager>(jsonString)!;
      }

      foreach (var place in manager.Places)
      {
        place.SetData(BaseGame.GWM.PlaceData.Places[place.Key]);
        place.Texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>($"Places/{place.Key}");
      }

      return manager;
    }
    #endregion

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
      var id = BaseGame.GWM.IdManager.PlaceId++;
      var place = new Place(data, BaseGame.GWM.GameModel.Content.Load<Texture2D>($"Places/{data.Key}"), point)
      {
        Id = id,
        Name = $"{data.Name} {id}",
      };

      Places.Add(place);

      BaseGame.GWM.Map.Add(point, place.Data.Size);

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
