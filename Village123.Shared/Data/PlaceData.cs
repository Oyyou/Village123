using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class PlaceData
  {
    public Dictionary<string, Place> Places { get; set; } = new();

    public class Place
    {
      public string Name { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }
      [JsonProperty("category")]
      public string Category { get; set; }
    }

    public static PlaceData Load()
    {
      var data = new PlaceData();

      var placeDataFiles = Directory.GetFiles("Content/Data", "placeData*.json");
      foreach (var file in placeDataFiles)
      {
        var jsonString = File.ReadAllText(file);
        var places = JsonConvert.DeserializeObject<Dictionary<string, Place>>(jsonString);

        foreach (var kvp in places)
        {
          var placeName = kvp.Key;
          var place = kvp.Value;

          place.Name = placeName;

          if (!data.Places.ContainsKey(placeName))
          {
            data.Places.Add(placeName, place);
          }
        }
      }

      return data;
    }
  }
}
