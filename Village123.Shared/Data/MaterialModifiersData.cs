using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class MaterialModifiersData
  {
    public Dictionary<string, MaterialModifier> MaterialModifiers { get; set; } = new();

    public class MaterialModifier
    {
      [JsonProperty("name")]
      public string Name { get; set; }
    }

    public static MaterialModifiersData Load()
    {
      var data = new MaterialModifiersData();

      var files = Directory.GetFiles("Content/Data", "materialModifiersData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var values = JsonConvert.DeserializeObject<Dictionary<string, MaterialModifier>>(jsonString);

        foreach (var kvp in values)
        {
          if (!data.MaterialModifiers.ContainsKey(kvp.Key))
          {
            data.MaterialModifiers.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
