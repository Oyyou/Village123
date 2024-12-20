using Village123.Shared.Entities;
using Village123.Shared.Interfaces;

namespace Village123.Shared.VillagerActions.DetermineActions
{
  public class DetermineRecreationalWalkAction : IDetermineAction
  {
    private int _jobId;

    public string Name => "Recreationally Walking";

    public float Priority(Villager villager)
    {
      var offset = 0f;
      var social = villager.Conditions["Happiness"].Value;
      switch (social)
      {
        case < 25:
          offset = 2f;
          break;
        case < 50:
          offset = 1f;
          break;
      }

      return 4f - offset;
    }

    public bool CanExecute(Villager villager)
    {
      if (villager.ActionQueue.Count > 0)
        return false;

      if (villager.GetCooldown("RecreationalWalk").Duration > 0)
        return false;

      return true;
    }

    public void Execute(Villager villager)
    {
      var destination = BaseGame.GWM.Map.FindNearestValidPoint(villager.Point, 20);
      if (!destination.HasValue)
      {
        return;
      }

      villager.AddAction(new WalkAction(villager, destination.Value, true) { IsRecreational = true });
    }
  }
}
