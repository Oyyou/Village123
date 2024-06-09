using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
namespace Village123.Shared.Data
{
  public class ResourceTypeData
  {
    public Dictionary<string, ResourceType> ResourceTypes { get; set; } = new();

    public class ResourceType
    {
      public class ResourceTypeAction
      {
        public bool CanCancel { get; set; } = false;
      }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("actions")]
      public Dictionary<string, ResourceTypeAction> Actions { get; set; }
    }

    public static ResourceTypeData Load()
    {
      var data = new ResourceTypeData();

      var files = Directory.GetFiles("Content/Data", "resourceTypeData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var values = JsonConvert.DeserializeObject<Dictionary<string, ResourceType>>(jsonString);

        foreach (var kvp in values)
        {
          if (!data.ResourceTypes.ContainsKey(kvp.Key))
          {
            data.ResourceTypes.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
