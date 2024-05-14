using System.IO;
using System.Text.Json;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;

namespace Village123.Shared.Data
{
  public class IdData : IPersistable<IdData>
  {
    private const string fileName = "ids.json";

    public int VillagerId { get; set; }
    public int PlaceId { get; set; }

    public void Save()
    {
      var jsonString = JsonSerializer.Serialize(this);
      File.WriteAllText(fileName, jsonString);
    }

    public static IdData Load(GameWorld gameWorld)
    {
      var idData = new IdData();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        idData = JsonSerializer.Deserialize<IdData>(jsonString)!;
      }

      return idData;
    }
  }
}
