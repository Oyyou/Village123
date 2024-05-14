using Village123.Shared.Entities;
using Village123.Shared.Managers;

namespace Village123.Shared.VillagerActions
{
  internal class IdleAction : VillagerAction
  {
    public override string Name => "Idle";

    public IdleAction() { }

    public IdleAction(Villager villager, GameWorldManager gwm) : base(villager, gwm)
    {

    }

    public override void Start()
    {
      _villager.ActionQueue.Enqueue(new WalkAction(_villager, _gwm, _villager.Point, false));
    }

    protected override void OnInitialize()
    {
    }

    public override void Update()
    {

    }

    public override bool IsComplete()
    {
      return true;
    }

    public override void OnComplete()
    {
    }
  }
}
