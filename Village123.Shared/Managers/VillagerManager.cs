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
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Managers
{
  public class VillagerManager
  {
    private static VillagerManager _instance;
    private static readonly object _lock = new();

    private const string fileName = "villagers.json";

    private static List<string> MaleFirstNames = new();
    private static List<string> FemaleFirstNames = new();
    private static List<string> LastNames = new();

    private GameWorldManager _gwm;

    private List<IDetermineAction> _determineActions;

    public List<Villager> Villagers { get; private set; } = new();

    public VillagerManager()
    {
      MaleFirstNames = File.ReadAllLines("Content/maleFirstNames.txt").ToList();
      FemaleFirstNames = File.ReadAllLines("Content/femaleFirstNames.txt").ToList();
      LastNames = File.ReadAllLines("Content/lastNames.txt").ToList();

      LoadDetermineActions();
    }

    public static VillagerManager GetInstance(GameWorldManager gwm)
    {
      lock (_lock)
      {
        _instance ??= Load(gwm);
      }

      return _instance;
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
      var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(fileName, jsonString);
    }

    private static VillagerManager Load(GameWorldManager gwm)
    {
      var villagerManager = new VillagerManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        villagerManager = JsonConvert.DeserializeObject<VillagerManager>(jsonString)!;
      }

      villagerManager._gwm = gwm;

      foreach (var villager in villagerManager.Villagers)
      {
        villager.Texture = gwm.GameModel.Content.Load<Texture2D>("Circle");
        foreach (var action in villager.ActionQueue)
        {
          action.Initialize(villager, gwm);
        }
      }

      return villagerManager;
    }
    #endregion

    public void Update(GameTime gameTime)
    {
      foreach (var villager in Villagers)
      {
        DetermineNextActions(villager);

        villager.Update(gameTime);
      }

      foreach (var job in JobManager.GetInstance().Jobs)
      {
        if (job.WorkerIds.Count == job.MaxWorkers)
        {
          continue;
        }

        foreach (var villager in Villagers)
        {
          if (villager.JobIds.Count > 0)
          {
            continue;
          }

          villager.JobIds.Add(job.Id);
          job.WorkerIds.Add(villager.Id);
          break;
        }
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

      foreach (var action in _determineActions.OrderByDescending(a => a.Priority))
      {
        if (action.CanExecute(villager, _gwm))
        {
          action.Execute(villager, _gwm);
          return;
        }
      }

      if (villager.JobIds.Count > 0)
      {
        var job = JobManager.GetInstance().Jobs.FirstOrDefault(a => a.Id == villager.JobIds[0]);

        villager.AddAction(new WalkAction(villager, _gwm, job.Point, false));
        villager.AddAction(new CraftAction(villager, _gwm, job));
        return;
      }

      villager.AddAction(new IdleAction(villager, _gwm));
    }

    public Villager CreateRandomVillager()
    {
      var gender = BaseGame.Random.Next(0, 2) == 0 ? Genders.Male : Genders.Female;
      var firstNames = (gender == Genders.Male ? MaleFirstNames : FemaleFirstNames);
      var villager = new Villager(_gwm.GameModel.Content.Load<Texture2D>("Circle"))
      {
        Id = _gwm.IdManager.VillagerId++,
        FirstName = firstNames[BaseGame.Random.Next(0, firstNames.Count)],
        LastName = LastNames[BaseGame.Random.Next(0, firstNames.Count)],
        Gender = gender,
        Conditions = new Dictionary<string, Condition>()
        {
          { "Energy", new(100f, -0.10f) }
        }
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
      var villager = new Villager(_gwm.GameModel.Content.Load<Texture2D>("Circle"))
      {
        Id = _gwm.IdManager.VillagerId++,
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
