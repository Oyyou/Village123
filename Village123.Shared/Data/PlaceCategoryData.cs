using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class PlaceCategoryData
  {
    public Dictionary<string, PlaceCategory> Categories { get; set; } = new();

    public class PlaceCategory
    {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("buttonLabel")]
      public string ButtonLabel { get; set; }
    }

    public static PlaceCategoryData Load()
    {
      var data = new PlaceCategoryData();

      var files = Directory.GetFiles("Content/Data", "placeCategoryData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var values = JsonConvert.DeserializeObject<Dictionary<string, PlaceCategory>>(jsonString);

        foreach (var kvp in values)
        {
          if (!data.Categories.ContainsKey(kvp.Key))
          {
            data.Categories.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
