using System;
using Village123.Shared.Entities;
using Village123.Shared.Managers;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions
{
  [Serializable]
  public abstract class VillagerAction : IVillagerAction
  {
    protected Villager _villager;
    protected GameWorldManager _gwm;

    protected ConditionManager _conditionManager;

    public abstract string Name { get; }

    public bool Started { get; set; } = false;

    protected VillagerAction() { }
    protected VillagerAction(Villager villager, GameWorldManager gwm)
    {
      Initialize(villager, gwm);
    }

    public void Initialize(Villager villager, GameWorldManager gwm)
    {
      _villager = villager;
      _gwm = gwm;

      _conditionManager = new ConditionManager(_villager);

      OnInitialize();
    }

    protected abstract void OnInitialize();

    public abstract bool IsComplete();
    public abstract void OnComplete();

    public abstract void Start();

    public abstract void Update();
  }
}
