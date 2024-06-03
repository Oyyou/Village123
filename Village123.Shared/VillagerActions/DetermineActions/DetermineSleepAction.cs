﻿using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Managers;

namespace Village123.Shared.VillagerActions.DetermineActions
{
  public class DetermineSleepAction : IDetermineAction
  {
    public string Name => "Sleep";

    public float Priority => 1f;

    public bool CanExecute(Villager villager, GameWorldManager gwm)
    {
      return villager.Conditions["Energy"].Value <= 0;
    }

    public void Execute(Villager villager, GameWorldManager gwm)
    {
      var beds = PlaceManager.GetInstance(gwm).Places.Where(p => p.Name.Contains("bed"));
      var villagerBed = beds.FirstOrDefault(b => b.OwnerIds.Contains(villager.Id));

      if (villagerBed == null)
      {
        var emptyBed = beds.FirstOrDefault(b => b.OwnerIds.Count == 0);

        if (emptyBed != null)
        {
          villager.AddAction(new WalkAction(villager, gwm, emptyBed.Point, true));
        }
      }
      else
      {
        villager.AddAction(new WalkAction(villager, gwm, villagerBed.Point, true));
      }

      villager.AddAction(new SleepAction(villager, gwm));
    }
  }
}
