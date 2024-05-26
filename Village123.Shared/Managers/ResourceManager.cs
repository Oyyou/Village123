using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;

namespace Village123.Shared.Managers
{
  public class ResourceManager
  {
    private static ResourceManager _instance;
    private static readonly object _lock = new();

    private const string fileName = "villagers.json";

    private GameWorldManager _gwm;

    public List<Resource> Resources { get; private set; } = new();

    public ResourceManager()
    {

    }

    public static ResourceManager GetInstance(GameWorldManager gwm)
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

    private static ResourceManager Load(GameWorldManager gwm)
    {
      var manager = new ResourceManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        manager = JsonConvert.DeserializeObject<ResourceManager>(jsonString)!;
      }

      manager._gwm = gwm;

      foreach (var resource in manager.Resources)
      {
        resource.SetData(gwm.ResourceData.Resources[resource.Name]);
      }

      return manager;
    }
    #endregion
  }
}
