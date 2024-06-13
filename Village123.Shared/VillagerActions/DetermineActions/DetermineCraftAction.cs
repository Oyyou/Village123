using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;

namespace Village123.Shared.VillagerActions.DetermineActions
{
  public class DetermineCraftAction : IDetermineAction
  {
    private int _jobId;

    public string Name => "Craft";

    public float Priority => 2f;

    public bool CanExecute(Villager villager)
    {
      if (villager.JobIds.Count > 0)
        return false;

      var craftJobs = BaseGame.GWM.JobManager.Jobs.Where(c => c.Type == "crafting" && c.WorkerIds.Count < c.MaxWorkers);

      if (!craftJobs.Any())
        return false;

      var job = craftJobs.First();
      _jobId = job.Id;

      if (job.ProducedItem.Materials.Any(m => !BaseGame.GWM.MaterialManager.IsMaterialAvailable(m.Key, m.Value)))
      {
        return false;
      }

      return true;
    }

    public void Execute(Villager villager)
    {
      var job = BaseGame.GWM.JobManager.Jobs.Find(j => j.Id == _jobId);
      villager.JobIds.Add(job.Id);
      job.WorkerIds.Add(villager.Id);

      var materialIds = new List<int>();
      foreach (var requiredMaterial in job.ProducedItem.Materials)
      {
        for (int i = 0; i < requiredMaterial.Value; i++)
        {
          foreach (var material in BaseGame.GWM.MaterialManager.Materials)
          {
            if (material.IsInUse)
              continue;

            if (requiredMaterial.Key != material.Name)
              continue;

            villager.AddAction(new WalkAction(villager, material.Point, false));
            villager.AddAction(new CarryAction(villager, material));
            villager.AddAction(new WalkAction(villager, job.Point, false));
            villager.AddAction(new DropAction(villager, job.Point, material));

            materialIds.Add(material.Id);
            break;
          }
        }
      }
      villager.AddAction(new WalkAction(villager, job.Point, false));
      villager.AddAction(new CraftAction(villager, job, materialIds));
    }
  }
}
