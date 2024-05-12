using System.IO;
using System.Text.Json;

namespace Village123.Shared.Managers
{
  public class IdManager
  {
    private const string fileName = "ids.json";

    public int VillagerId { get; set; }
    public int PlaceId { get; set; }
    public int JobId { get; set; }

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
