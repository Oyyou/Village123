using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class PlaceCategoryData
  {
    public List<string> Categories { get; set; } = new();

    public static PlaceCategoryData Load()
    {
      var data = new PlaceCategoryData();

      var placeDataFiles = Directory.GetFiles("Content/Data", "placeCategoryData*.json");
      foreach (var file in placeDataFiles)
      {
        var jsonString = File.ReadAllText(file);
        var categories = JsonConvert.DeserializeObject<List<string>>(jsonString);

        foreach (var category in categories)
        {
          if (!data.Categories.Contains(category))
          {
            data.Categories.Add(category);
          }
        }
      }

      return data;
    }
  }
}
