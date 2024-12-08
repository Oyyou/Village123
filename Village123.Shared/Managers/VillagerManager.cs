using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;
using Village123.Shared.Services;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Managers
{
  public class VillagerManager
  {
    private const string fileName = "villagers.json";
    private SaveFileService _saveFileService;

    private static List<string> MaleFirstNames = new();
    private static List<string> FemaleFirstNames = new();
    private static List<string> LastNames = new();

    private List<IDetermineAction> _determineActions;

    public List<Villager> Villagers { get; private set; } = new();

    public VillagerManager()
    {
      MaleFirstNames = File.ReadAllLines("Content/maleFirstNames.txt").ToList();
      FemaleFirstNames = File.ReadAllLines("Content/femaleFirstNames.txt").ToList();
      LastNames = File.ReadAllLines("Content/lastNames.txt").ToList();

      LoadDetermineActions();
    }

    public VillagerManager(SaveFileService saveFileService)
      : this()
    {
      _saveFileService = saveFileService;
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

    #region Serialization
    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static VillagerManager Load(SaveFileService saveFileService)
    {
      var manager = saveFileService.Load<VillagerManager>(fileName);

      if (manager == null)
      {
        manager = new VillagerManager(saveFileService);
      }
      else
      {
        manager._saveFileService = saveFileService;
      }

      foreach (var villager in manager.Villagers)
      {
        villager.Texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>("Circle");
        foreach (var action in villager.ActionQueue)
        {
          action.Initialize(villager);
        }
      }

      return manager;
    }
    #endregion

    public void UpdateMouse()
    {
      foreach (var villager in Villagers)
      {
        // villager.ClickableComponent.Update();
      }
    }

    public void Update(GameTime gameTime)
    {
      foreach (var villager in Villagers)
      {
        DetermineNextActions(villager);

        villager.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var villager in Villagers)
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

      foreach (var action in _determineActions.OrderBy(a => a.Priority))
      {
        if (action.CanExecute(villager))
        {
          action.Execute(villager);
          return;
        }
      }

      villager.AddAction(new IdleAction(villager));
    }

    public Villager CreateRandomVillager()
    {
      var gender = BaseGame.Random.Next(0, 2) == 0 ? Genders.Male : Genders.Female;
      var firstNames = (gender == Genders.Male ? MaleFirstNames : FemaleFirstNames);
      var villager = new Villager(BaseGame.GWM.GameModel.Content.Load<Texture2D>("Circle"))
      {
        Id = BaseGame.GWM.IdManager.VillagerId++,
        FirstName = firstNames[BaseGame.Random.Next(0, firstNames.Count)],
        LastName = LastNames[BaseGame.Random.Next(0, firstNames.Count)],
        Gender = gender,
        Conditions = new Dictionary<string, Condition>()
        {
          { "Energy", new(100f, -100f / (16 * (60 * 60))) }
        },
        Colour = new Color(BaseGame.Random.Next(0, 255), BaseGame.Random.Next(0, 255), BaseGame.Random.Next(0, 255)),
      };

      Villagers.Add(villager);

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
      var villager = new Villager(BaseGame.GWM.GameModel.Content.Load<Texture2D>("Circle"))
      {
        Id = BaseGame.GWM.IdManager.VillagerId++,
        FirstName = firstNames[BaseGame.Random.Next(0, firstNames.Count)],
        LastName = father.LastName,
        Gender = gender,
        FatherId = father.Id,
        MotherId = mother.Id,
        Father = father,
        Mother = mother,
        Conditions = new Dictionary<string, Condition>()
        {
          { "Energy", new(100f, -0.10f) }
        }
      };

      Villagers.Add(villager);

      return villager;
    }
  }
}
