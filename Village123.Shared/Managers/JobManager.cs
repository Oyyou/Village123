﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.Managers
{
  public class JobManager
  {
    private static JobManager _instance;
    private static readonly object _lock = new();

    private const string fileName = "jobs.json";

    public List<Job> Jobs { get; private set; } = new();

    private JobManager()
    {
    }

    public static JobManager GetInstance()
    {
      lock (_lock)
      {
        _instance ??= Load();
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

    private static JobManager Load()
    {
      var mamager = new JobManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        mamager = JsonConvert.DeserializeObject<JobManager>(jsonString)!;
      }

      return mamager;
    }
    #endregion

    public Job Add(Place place, CraftItemModel craftItem = null)
    {
      var job = new Job()
      {
        Id = BaseGame.GWM.IdManager.JobId++,
        PlaceId = place.Id,
        ProducedItem = craftItem != null ? new ProducedItemModel() { ItemName = craftItem.Item.Key, Resources = craftItem.Resources } : null,
        Name = $"{craftItem.Item.Key}",
        Point = place.Point,
        MaxWorkers = 1,
        // RequiredEquipment = item.RequiredEquipment,
        Type = place.Data.Category,
        WorkerIds = new List<int>(),
      };

      Jobs.Add(job);

      return job;
    }

    public void CompleteJob(Villager villager, Job job)
    {
      Jobs.Remove(job);
      villager.JobIds.RemoveAt(0);

      ItemManager.GetInstance(BaseGame.GWM).AddCraftedItem(job.ProducedItem, job.Point);
    }
  }
}
