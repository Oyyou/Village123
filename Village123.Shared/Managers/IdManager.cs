using System.IO;
using System.Text.Json;

namespace Village123.Shared.Managers
{
  public class IdManager
  {
    private const string fileName = "ids.json";

    public int VillagerId { get; set; } = 1;
    public int PlaceId { get; set; } = 1;
    public int JobId { get; set; } = 1;
    public int ItemId { get; set; } = 1;
    public int MaterialId { get; set; } = 1;
    public int ResourceId { get; set; } = 1;

    public void Save()
    {
      var jsonString = JsonSerializer.Serialize(this);
      File.WriteAllText(fileName, jsonString);
    }

    public static IdManager Load()
    {
      var idData = new IdManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        idData = JsonSerializer.Deserialize<IdManager>(jsonString)!;
      }

      return idData;
    }
  }
}
