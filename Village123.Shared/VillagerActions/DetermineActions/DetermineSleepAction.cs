﻿using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions.DetermineActions
{
  public class DetermineSleepAction : IDetermineAction
  {
    public string Name => "Sleep";

    public float Priority => 1f;

    public bool CanExecute(Villager villager, GameWorld gameWorld)
    {
      return villager.Conditions["Energy"].Value <= 0;
    }

    public void Execute(Villager villager, GameWorld gameWorld)
    {
      var beds = gameWorld.Places.Where(p => p.Name.Contains("Bed"));
      var villagerBed = beds.FirstOrDefault(b => b.OwnerIds.Contains(villager.Id));

      if (villagerBed == null)
      {
        var emptyBed = beds.FirstOrDefault(b => b.OwnerIds.Count == 0);

        if (emptyBed != null)
        {
          villager.AddAction(new WalkAction(villager, gameWorld, emptyBed.Point, true));
        }
      }
      else
      {
        villager.AddAction(new WalkAction(villager, gameWorld, villagerBed.Point, true));
      }

      villager.AddAction(new SleepAction(villager, gameWorld));
    }
  }
}