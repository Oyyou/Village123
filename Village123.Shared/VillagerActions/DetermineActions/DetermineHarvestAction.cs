using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;

namespace Village123.Shared.VillagerActions.DetermineActions
{
  public class DetermineHarvestAction : IDetermineAction
  {
    private int _jobId;

    public string Name => "Harvest";

    public float Priority => 2.5f;

    public bool CanExecute(Villager villager)
    {
      if (villager.JobIds.Count > 0)
        return false;

      var jobs = BaseGame.GWM.JobManager.Jobs.Where(c => c.Type == "harvest" && c.WorkerIds.Count < c.MaxWorkers);

      if (!jobs.Any())
        return false;

      var job = jobs.First();
      _jobId = job.Id;

      return true;
    }

    public void Execute(Villager villager)
    {
      var job = BaseGame.GWM.JobManager.Jobs.Find(j => j.Id == _jobId);
      villager.JobIds.Add(job.Id);
      job.WorkerIds.Add(villager.Id);

      // TODO: Add collect resources to the list
      villager.AddAction(new WalkAction(villager, job.Point, false));
      villager.AddAction(new HarvestAction(villager, job));
    }
  }
}
