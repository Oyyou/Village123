using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Village123.Shared.Data
{
  public class ItemData
  {
    public Dictionary<string, Item> Items { get; set; } = new();

    public class Item
    {
      [JsonProperty("key")]
      public string Key { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("job")]
      public string Job { get; set; }

      [JsonProperty("createdAt")]
      public CreatedAtType CreatedAt { get; set; }

      [JsonProperty("requiredMaterials")]
      public Dictionary<string, int> RequiredMaterials { get; set; } = new();

      [JsonProperty("requiredEquipment")]
      public List<string> RequiredEquipment { get; set; } = new();

      public class CreatedAtType
      {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }
      }
    }

    public static ItemData Load()
    {
      var data = new ItemData();

      var files = Directory.GetFiles("Content/Data", "itemData*.json");
      foreach (var file in files)
      {
        var jsonString = File.ReadAllText(file);
        var values = JsonConvert.DeserializeObject<Dictionary<string, Item>>(jsonString);

        foreach (var kvp in values)
        {
          kvp.Value.Key = kvp.Key;
          if (!data.Items.ContainsKey(kvp.Key))
          {
            data.Items.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }

    public Dictionary<string, Item> GetItemsByCategory(string category) =>
      Items.Where(i => i.Value.CreatedAt.Category == category)
      .ToDictionary(c => c.Key, v => v.Value);
  }
}
