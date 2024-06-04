using Microsoft.Xna.Framework;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  internal class IdleAction : VillagerAction
  {
    public override string Name => "Idle";

    public IdleAction() { }

    public IdleAction(Villager villager) : base(villager)
    {

    }

    public override void Start()
    {
      _villager.ActionQueue.Enqueue(new WalkAction(_villager, _villager.Point, false));
    }

    protected override void OnInitialize()
    {
    }

    public override void Update(GameTime gameTime)
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
