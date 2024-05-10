using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Data
{
  public class VillagerData : IPersistable<VillagerData>
  {
    private const string fileName = "villagers.json";

    private List<Villager> _villagers = new();

    public List<Villager> Villagers
    {
      get => _villagers;
      set
      {
        _villagers = value;
      }
    }

    public void Add(Villager villager)
    {
      _villagers.Add(villager);
    }

    public void Save()
    {
      var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(fileName, jsonString);
    }

    public static VillagerData Load(GameWorld gameWorld)
    {
      var villagerData = new VillagerData();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        villagerData = JsonConvert.DeserializeObject<VillagerData>(jsonString)!;
      }

      foreach (var villager in villagerData.Villagers)
      {
        foreach (var action in villager.ActionQueue)
        {
          action.Initialize(villager, gameWorld);
        }
      }

      return villagerData;
    }
  }
}
