using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  [Serializable]
  public abstract class VillagerAction : IVillagerAction
  {
    protected Villager _villager;

    public bool Started { get; set; } = false;

    protected VillagerAction(Villager villager)
    {
      _villager = villager;
    }

    public abstract bool IsComplete();

    public abstract void Start();

    public abstract void Update();
  }
}
