using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;

namespace Village123.Shared.VillagerActions.DetermineActions
{
  public class DetermineCraftAction : IDetermineAction
  {
    public string Name => "Craft";

    public float Priority => 2f;

    public bool CanExecute(Villager villager)
    {
      return false;
    }

    public void Execute(Villager villager)
    {
      var beds = BaseGame.GWM.PlaceManager.Places.Where(p => p.Name.Contains("bed"));
      var villagerBed = beds.FirstOrDefault(b => b.OwnerIds.Contains(villager.Id));

      if (villagerBed == null)
      {
        var emptyBed = beds.FirstOrDefault(b => b.OwnerIds.Count == 0);

        if (emptyBed != null)
        {
          villager.AddAction(new WalkAction(villager, emptyBed.Point, true));
        }
      }
      else
      {
        villager.AddAction(new WalkAction(villager, villagerBed.Point, true));
      }

      villager.AddAction(new SleepAction(villager));
    }
  }
}
