using System.IO;
using System.Text.Json;
using Village123.Shared.Interfaces;

namespace Village123.Shared.Data
{
  public class IdData : IPersistable<IdData>
  {
    private const string fileName = "ids.json";

    private int _villagerId = 1;

    public int VillagerId
    {
      get => _villagerId;
      set
      {
        _villagerId = value;
      }
    }

    public void Save()
    {
      var jsonString = JsonSerializer.Serialize(this);
      File.WriteAllText(fileName, jsonString);
    }

    public static IdData Load()
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
