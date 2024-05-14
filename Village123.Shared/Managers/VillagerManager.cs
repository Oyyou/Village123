﻿using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Models;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Managers
{
  public class VillagerManager
  {
    private static List<string> MaleFirstNames = new();
    private static List<string> FemaleFirstNames = new();
    private static List<string> LastNames = new();

    private readonly GameWorld _gameWorld;
    private readonly IdData _idData;
    private readonly VillagerData _villagerData;

    public VillagerManager(
      GameWorld gameWorld,
      IdData idData,
      VillagerData villagerData
      )
    {
      _gameWorld = gameWorld;
      _idData = idData;
      _villagerData = villagerData;

      MaleFirstNames = File.ReadAllLines("Content/maleFirstNames.txt").ToList();
      FemaleFirstNames = File.ReadAllLines("Content/femaleFirstNames.txt").ToList();
      LastNames = File.ReadAllLines("Content/lastNames.txt").ToList();
    }

    public void Update()
    {
      foreach (var villager in _villagerData.Villagers)
      {
        villager.Update();

        DetermineNextActions(villager);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var villager in _villagerData.Villagers)
      {
        villager.Draw(spriteBatch);
      }
    }

    private void DetermineNextActions(Villager villager)
    {
      if (villager.ActionQueue.Count > 0)
      {
        return;
      }

      if (villager.Conditions["Energy"].Value <= 0)
      {
        var beds = _gameWorld.Places.Where(p => p.Name.Contains("Bed"));
        var villagerBed = beds.FirstOrDefault(b => b.OwnerIds.Contains(villager.Id));

        if (villagerBed == null)
        {
          var emptyBed = beds.FirstOrDefault(b => b.OwnerIds.Count == 0);

          if (emptyBed != null)
          {
            villager.AddAction(new WalkAction(villager, _gameWorld, emptyBed.Point, true));
          }
        }
        else
        {
          villager.AddAction(new WalkAction(villager, _gameWorld, villagerBed.Point, true));
        }

        villager.AddAction(new SleepAction(villager, _gameWorld));
        return;
      }

      villager.AddAction(new IdleAction(villager, _gameWorld));
    }

    public Villager CreateRandomVillager()
    {
      var gender = BaseGame.Random.Next(0, 2) == 0 ? Genders.Male : Genders.Female;
      var firstNames = (gender == Genders.Male ? MaleFirstNames : FemaleFirstNames);
      var villager = new Villager(_gameWorld.Content.Load<Texture2D>("Circle"))
      {
        Id = _idData.VillagerId++,
        FirstName = firstNames[BaseGame.Random.Next(0, firstNames.Count)],
        LastName = LastNames[BaseGame.Random.Next(0, firstNames.Count)],
        Gender = gender,
        Conditions = new Dictionary<string, Condition>()
        {
          { "Energy", new(100f, -0.10f) }
        }
      };

      _villagerData.Add(villager);

      return villager;
    }

    /// <summary>
    /// Absolutely horrible name for a method. Re-think
    /// </summary>
    /// <returns></returns>
    public Villager BirthVillager(Villager mother, Villager father)
    {
      var gender = BaseGame.Random.Next(0, 2) == 0 ? Genders.Male : Genders.Female;
      var firstNames = (gender == Genders.Male ? MaleFirstNames : FemaleFirstNames);
      var villager = new Villager(_gameWorld.Content.Load<Texture2D>("Circle"))
      {
        Id = _idData.VillagerId++,
        FirstName = firstNames[BaseGame.Random.Next(0, firstNames.Count)],
        LastName = father.LastName,
        Gender = gender,
        FatherId = father.Id,
        MotherId = mother.Id,
        Father = father,
        Mother = mother,
      };

      _villagerData.Add(villager);

      return villager;
    }
  }
}
