using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.Data
{
  public class JobData
  {
    private const string fileName = "jobs.json";

    private List<Job> _jobs = new();

    public List<Job> Jobs
    {
      get => _jobs;
      set
      {
        _jobs = value;
      }
    }

    public void Add(Job job)
    {
      _jobs.Add(job);
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

    public static JobData Load(GameWorld gameWorld)
    {
      var jobData = new JobData();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        jobData = JsonConvert.DeserializeObject<JobData>(jsonString)!;
      }

      return jobData;
    }
  }
}
