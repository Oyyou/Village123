using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;

namespace Village123.Shared.Managers
{
  public class ItemManager
  {
    private static ItemManager _instance;
    private static readonly object _lock = new();

    private const string fileName = "items.json";

    private GameWorldManager _gwm;

    public List<Item> Items { get; private set; } = new();

    private ItemManager() { }

    public static ItemManager GetInstance(GameWorldManager gwm)
    {
      lock (_lock)
      {
        _instance ??= Load(gwm);
      }

      return _instance;
    }

    #region Serialization
    public void Save()
    {
      var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(fileName, jsonString);
    }

    private static ItemManager Load(GameWorldManager gwm)
    {
      var mamager = new ItemManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        mamager = JsonConvert.DeserializeObject<ItemManager>(jsonString)!;
      }

      mamager._gwm = gwm;

      return mamager;
    }
    #endregion
  }
}
