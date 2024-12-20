using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Village123.Shared.Entities;

namespace Village123.Shared.Models.WaitTypes
{
  [Serializable]
  public class WaitForVillagerToArrive : IWaitType
  {
    private Villager _villager;

    public int VillagerId { get; set; }
    public Point Destination { get; set; }

    public WaitForVillagerToArrive()
    {

    }

    public WaitForVillagerToArrive(Villager villager, Point destination)
    {
      VillagerId = villager.Id;
      Destination = destination;

      Initialize();
    }

    public void Initialize()
    {
      _villager = BaseGame.GWM.VillagerManager.Villagers.FirstOrDefault(v => v.Id == VillagerId);
    }

    public bool IsComplete()
    {
      return _villager.Point == Destination;
    }
  }
}
