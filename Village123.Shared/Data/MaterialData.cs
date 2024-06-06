using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Village123.Shared.Data
{
  public class MaterialData
  {
    public Dictionary<string, Material> Materials { get; set; } = new();

    public class Material
    {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("modifiers")]
      public Dictionary<string, int> Modifiers { get; set; }
    }

    public static MaterialData Load()
    {
      var data = new MaterialData();

      var files = Directory.GetFiles("Content/Data", "materialData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var values = JsonConvert.DeserializeObject<Dictionary<string, Material>>(jsonString);

        foreach (var kvp in values)
        {
          if (!data.Materials.ContainsKey(kvp.Key))
          {
            data.Materials.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }

    public Dictionary<string, Material> GetByType(string type) => Materials.Where(i => i.Value.Type == type).ToDictionary(c => c.Key, v => v.Value);
  }
}
