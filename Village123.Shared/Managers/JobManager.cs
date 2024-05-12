using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;

namespace Village123.Shared.Managers
{
  public class JobManager
  {
    private const string fileName = "jobs.json";

    private GameWorldManager _gwm;

    public List<Job> Jobs { get; private set; } = new();

    public JobManager()
    {
    }

    private void Initialize(GameWorldManager gwm)
    {
      _gwm = gwm;
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

    public static JobManager Load(GameWorldManager gwm)
    {
      var jobManager = new JobManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        jobManager = JsonConvert.DeserializeObject<JobManager>(jsonString)!;
      }

      jobManager.Initialize(gwm);

      return jobManager;
    }
    #endregion

    public Job Add(string name, Point point)
    {
      var job = new Job()
      {
        Id = _gwm.IdManager.JobId++,
        Name = name,
        Point = point,
      };

      Jobs.Add(job);

      return job;
    }
  }
}
