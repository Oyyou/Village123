using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class PlaceData
  {
    public Dictionary<string, Place> Places { get; set; } = new();

    public class Place
    {
      private const int MaxRadius = 10;
      private int _radius = 0;

      [JsonProperty("key")]
      public string Key { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("category")]
      public string Category { get; set; }

      [JsonProperty("draggable")]
      public bool Draggable { get; set; } = false;

      [JsonProperty("radius")]
      public int Radius
      {
        get => _radius;
        set => _radius = Math.Min(value, MaxRadius);
      }

      /// <summary>
      /// The dimensions of the place (used for blocking paths)
      /// </summary>
      [JsonProperty("size")]
      public Point Size { get; set; }

      /// <summary>
      /// Once built, does it block the path
      /// </summary>
      [JsonProperty("blocksPath")]
      public bool BlocksPath { get; set; } = false;

      /// <summary>
      /// If it's a blocking object, these points will be walkable
      /// </summary>
      [JsonProperty("walkablePoints")]
      public List<Point> WalkablePoints { get; set; } = new();

      /// <summary>
      /// The resources required to build
      /// </summary>
      [JsonProperty("requiredBuildResources")]
      public Dictionary<string, int> RequiredBuildResources { get; set; } = new();

      /// <summary>
      /// The skills required by the villager to be built
      /// </summary>
      [JsonProperty("requiredBuildSkills")]
      public Dictionary<string, int> RequiredBuildSkills { get; set; } = new();

      /// <summary>
      /// The types items that can be produced
      /// </summary>
      [JsonProperty("producedItemTypes")]
      public List<string> ProducedItemTypes { get; set; } = new();
    }

    public static PlaceData Load()
    {
      var data = new PlaceData();

      var placeDataFiles = Directory.GetFiles("Content/Data", "placeData*.json");
      foreach (var file in placeDataFiles)
      {
        var jsonString = File.ReadAllText(file);
        var results = JsonConvert.DeserializeObject<Dictionary<string, Place>>(jsonString);

        foreach (var kvp in results)
        {
          kvp.Value.Key = kvp.Key;
          if (!data.Places.ContainsKey(kvp.Key))
          {
            data.Places.Add(kvp.Key, kvp.Value);
          }
        }
      }

      return data;
    }
  }
}
