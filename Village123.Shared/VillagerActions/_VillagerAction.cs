using Microsoft.Xna.Framework;
using System;
using Village123.Shared.Entities;
using Village123.Shared.Managers;

namespace Village123.Shared.VillagerActions
{
  [Serializable]
  public abstract class VillagerAction : IVillagerAction
  {
    protected Villager _villager;

    protected ConditionManager _conditionManager;

    public abstract string Name { get; }

    public bool Started { get; set; } = false;

    protected VillagerAction() { }
    protected VillagerAction(Villager villager)
    {
      Initialize(villager);
    }

    public void Initialize(Villager villager)
    {
      _villager = villager;

      _conditionManager = new ConditionManager(_villager);

      OnInitialize();
    }

    protected abstract void OnInitialize();

    public abstract bool IsComplete();
    public abstract void OnComplete();

    public abstract void Start();

    public abstract void Update(GameTime gameTime);
  }
}
