﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;

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

    public void Update()
    {
      foreach(var villager in _villagers)
      {
        villager.Update();
      }
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

    public static VillagerData Load()
    {
      var villagerData = new VillagerData();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        villagerData = JsonConvert.DeserializeObject<VillagerData>(jsonString)!;
      }

      return villagerData;
    }
  }
}
