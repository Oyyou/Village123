using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class ItemData
  {
    public Dictionary<string, Item> Items { get; set; } = new();

    public class Item
    {
      [JsonProperty("name")]
      public string Name { get; set; }
      [JsonProperty("type")]
      public string Type { get; set; }
    }

    public static ItemData Load()
    {
      var data = new ItemData();

      var files = Directory.GetFiles("Content/Data", "itemData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var itemTypes = JsonConvert.DeserializeObject<Dictionary<string, Item>>(jsonString);

        foreach (var kvp in itemTypes)
        {
          if (!data.Items.ContainsKey(kvp.Key))
          {
            data.Items.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
