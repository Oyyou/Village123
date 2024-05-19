using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Village123.Shared.Data
{
  public class ResourceModifiersData
  {
    public Dictionary<string, ResourceModifier> ResourceModifiers { get; set; } = new();

    public class ResourceModifier
    {
      [JsonProperty("name")]
      public string Name { get; set; }
    }

    public static ResourceModifiersData Load()
    {
      var data = new ResourceModifiersData();

      var files = Directory.GetFiles("Content/Data", "resourceModifiersData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var values = JsonConvert.DeserializeObject<Dictionary<string, ResourceModifier>>(jsonString);

        foreach (var kvp in values)
        {
          if (!data.ResourceModifiers.ContainsKey(kvp.Key))
          {
            data.ResourceModifiers.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
