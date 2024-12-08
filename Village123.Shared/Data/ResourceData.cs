using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Village123.Shared.Data
{
  public class ResourceData
  {
    public Dictionary<string, Resource> Resources { get; set; } = new();

    public class Resource
    {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("drop")]
      public string Drop { get; set; }

      [JsonProperty("harvestTime")]
      public int HarvestTime { get; set; }

      [JsonProperty("size")]
      public Point Size { get; set; }

      [JsonProperty("pointOffset")]
      public Point PointOffset { get; set; }
    }

    public static ResourceData Load()
    {
      var data = new ResourceData();

      var files = Directory.GetFiles("Content/Data", "resourceData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var values = JsonConvert.DeserializeObject<Dictionary<string, Resource>>(jsonString);

        foreach (var kvp in values)
        {
          if (!data.Resources.ContainsKey(kvp.Key))
          {
            data.Resources.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
