using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.Managers
{
  public class JobManager
  {
    private const string fileName = "jobs.json";

    public List<Job> Jobs { get; private set; } = new();

    private JobManager()
    {
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

    public static JobManager Load()
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

    public Job AddJob(Point point, HarvestedResourceModel heavestedResource)
    {
      var job = new Job()
      {
        Id = BaseGame.GWM.IdManager.JobId++,
        HarvestedResource = heavestedResource,
        Point = point,
        MaxWorkers = 1,
        Type = "harvest",
        WorkerIds = new List<int>(),
      };

      Jobs.Add(job);

      return job;
    }

    public void RemoveJobById(int id)
    {
      var job = Jobs.Find(j => j.Id == id);

      if (job == null)
      {
        return;
      }

      foreach(var workerId in job.WorkerIds)
      {
        var villager = BaseGame.GWM.VillagerManager.Villagers.FirstOrDefault(v => job.WorkerIds.Contains(v.Id));
        villager.JobIds.Remove(job.Id);
        villager.ActionQueue.Clear(); // FYI: Not safe
      }
    }

    public Job Add(Place place, CraftItemModel craftItem = null)
    {
      var job = new Job()
      {
        Id = BaseGame.GWM.IdManager.JobId++,
        ProducedItem = craftItem != null ? new ProducedItemModel() { ItemName = craftItem.Item.Key, Materials = craftItem.Materials } : null,
        Point = place.Point,
        MaxWorkers = 1,
        // RequiredEquipment = item.RequiredEquipment,
        Type = place.Data.Category,
        WorkerIds = new List<int>(),
      };

      place.JobIds.Add(job.Id);
      Jobs.Add(job);

      return job;
    }

    public void CompleteJob(Villager villager, Job job)
    {
      Jobs.Remove(job);
      villager.JobIds.RemoveAt(0);

      if (job.ProducedItem != null)
      {
        BaseGame.GWM.ItemManager.AddCraftedItem(job.ProducedItem, job.Point);
      }

      if (job.HarvestedResource != null)
      {
        BaseGame.GWM.MaterialManager.Add(job.HarvestedResource.ResourceName, job.Point);
      }

      foreach (var place in BaseGame.GWM.PlaceManager.Places)
      {
        if (place.JobIds.Contains(job.Id))
        {
          place.JobIds.Remove(job.Id);
          break;
        }
      }
    }
  }
}
