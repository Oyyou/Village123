using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
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

    private List<IDetermineAction> _determineActions;

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

      LoadDetermineActions();
    }

    private void LoadDetermineActions()
    {
      _determineActions = new List<IDetermineAction>();

      var types = Assembly.GetExecutingAssembly().GetTypes()
          .Where(t => typeof(IDetermineAction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

      foreach (var type in types)
      {
        var instance = Activator.CreateInstance(type) as IDetermineAction;
        _determineActions.Add(instance);
      }
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

      foreach (var action in _determineActions.OrderByDescending(a => a.Priority))
      {
        if (action.CanExecute(villager, _gameWorld))
        {
          action.Execute(villager, _gameWorld);
          return;
        }
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
