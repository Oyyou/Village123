using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions
{
  internal class IdleAction : VillagerAction
  {
    public override string Name => "Sleep";

    public IdleAction() { }

    public IdleAction(Villager villager, GameWorld gameWorld) : base(villager, gameWorld)
    {

    }

    public override void Start()
    {
      _villager.ActionQueue.Enqueue(new WalkAction(_villager, _gameWorld, _villager.Point, false));
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
