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
    private const string fileName = "villagers.json";

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

    public static VillagerManager Load()
    {
      var villagerManager = new VillagerManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        villagerManager = JsonConvert.DeserializeObject<VillagerManager>(jsonString)!;
      }

      foreach (var villager in villagerManager.Villagers)
      {
        villager.Texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>("Circle");
        foreach (var action in villager.ActionQueue)
        {
          action.Initialize(villager);
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

      foreach (var job in BaseGame.GWM.JobManager.Jobs)
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
        if (action.CanExecute(villager))
        {
          action.Execute(villager);
          return;
        }
      }

      if (villager.JobIds.Count > 0)
      {
        var job = BaseGame.GWM.JobManager.Jobs.FirstOrDefault(a => a.Id == villager.JobIds[0]);

        villager.AddAction(new WalkAction(villager, job.Point, false));

        if (job.Type == "harvest")
        {
          villager.AddAction(new HarvestAction(villager, job));
        }
        else if (job.Type == "craft")
        {
          villager.AddAction(new CraftAction(villager, job));
        }
        return;
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
          { "Energy", new(100f, -0.10f) }
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
