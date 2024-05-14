using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class PlaceData
  {
    public Dictionary<string, Place> Places { get; set; } = new();

    public class Place
    {
      public string Name { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("category")]
      public string Category { get; set; }

      /// <summary>
      /// The dimensions of the place (used for blocking paths)
      /// </summary>
      [JsonProperty("size")]
      public Point Size { get; set; }

      /// <summary>
      /// Does it block the path while under construction
      /// </summary>
      [JsonProperty("constructionBlocksPath")]
      public bool ConstructionBlocksPath { get; set; }

      /// <summary>
      /// Once built, does it block the path
      /// </summary>
      [JsonProperty("blocksPath")]
      public bool BlocksPath { get; set; }

      /// <summary>
      /// The resources required to build
      /// </summary>
      [JsonProperty("requiredBuildResources")]
      public Dictionary<string, int> RequiredBuildResources { get; set; }

      /// <summary>
      /// The skills required by the villager to be built
      /// </summary>
      [JsonProperty("requiredBuildSkills")]
      public Dictionary<string, int> RequiredBuildSkills { get; set; }

      /// <summary>
      /// The types items that can be produced
      /// </summary>
      [JsonProperty("producedItemTypes")]
      public List<string> ProducedItemTypes { get; set; }
    }

    public static PlaceData Load()
    {
      var data = new PlaceData();

      var placeDataFiles = Directory.GetFiles("Content/Data", "placeData*.json");
      foreach (var file in placeDataFiles)
      {
        var jsonString = File.ReadAllText(file);
        var places = JsonConvert.DeserializeObject<Dictionary<string, Place>>(jsonString);

        foreach (var kvp in places)
        {
          var placeName = kvp.Key;
          var place = kvp.Value;

          place.Name = placeName;

          if (!data.Places.ContainsKey(placeName))
          {
            data.Places.Add(placeName, place);
          }
        }
      }

      return data;
    }
  }
}
