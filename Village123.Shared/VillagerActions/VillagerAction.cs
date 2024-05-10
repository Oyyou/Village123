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
    protected GameWorld _gameWorld;

    protected ConditionManager _conditionManager;

    public abstract string Name { get; }

    public bool Started { get; set; } = false;

    protected VillagerAction() { }
    protected VillagerAction(Villager villager, GameWorld gameWorld)
    {
      Initialize(villager, gameWorld);
    }

    public void Initialize(Villager villager, GameWorld gameWorld)
    {
      _villager = villager;
      _gameWorld = gameWorld;

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
