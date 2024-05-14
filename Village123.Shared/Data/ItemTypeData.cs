using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class ItemTypeData
  {
    public Dictionary<string, ItemType> ItemTypes { get; set; } = new();

    public class ItemType
    {
      [JsonProperty("name")]
      public string Name { get; set; }
    }

    public static ItemTypeData Load()
    {
      var data = new ItemTypeData();

      var files = Directory.GetFiles("Content/Data", "itemTypeData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var itemTypes = JsonConvert.DeserializeObject<Dictionary<string, ItemType>>(jsonString);

        foreach (var kvp in itemTypes)
        {
          if (!data.ItemTypes.ContainsKey(kvp.Key))
          {
            data.ItemTypes.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
