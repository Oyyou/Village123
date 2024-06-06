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

    public Job Add(Place place, CraftItemModel craftItem = null)
    {
      var job = new Job()
      {
        Id = BaseGame.GWM.IdManager.JobId++,
        PlaceId = place.Id,
        ProducedItem = craftItem != null ? new ProducedItemModel() { ItemName = craftItem.Item.Key, Materials = craftItem.Materials } : null,
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

      BaseGame.GWM.ItemManager.AddCraftedItem(job.ProducedItem, job.Point);
    }

    //public Job GetNextAvailableJob()
    //{
    //  var unworkedJobs = Jobs.Where(c => c.WorkerIds.Count < c.MaxWorkers);

    //  foreach (var job in unworkedJobs)
    //  {

    //  }
    //}
  }
}
