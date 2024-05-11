using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.Managers
{
  public class JobManager
  {
    private readonly GameWorld _gameWorld;
    private readonly IdData _idData;
    private readonly JobData _jobdata;

    public JobManager(
      GameWorld gameWorld,
      IdData idData,
      JobData jobData
      )
    {
      _gameWorld = gameWorld;
      _idData = idData;
      _jobdata = jobData;
    }

    public Job Add(string name, Point point)
    {
      var job = new Job()
      {
        Id = _idData.JobId++,
        Name = name,
        Point = point,
      };

      _jobdata.Add(job);

      return job;
    }
  }
}
