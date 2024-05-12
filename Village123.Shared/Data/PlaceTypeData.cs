using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Village123.Shared.Data
{
  public class PlaceTypeData
  {
    public List<string> Types { get; set; } = new();

    public static PlaceTypeData Load()
    {
      var data = new PlaceTypeData();

      var placeDataFiles = Directory.GetFiles("Content/Data", "placeTypeData*.json");
      foreach (var file in placeDataFiles)
      {
        var jsonString = File.ReadAllText(file);
        var types = JsonConvert.DeserializeObject<List<string>>(jsonString);

        foreach (var type in types)
        {
          if (!data.Types.Contains(type))
          {
            data.Types.Add(type);
          }
        }
      }

      return data;
    }
  }
}
