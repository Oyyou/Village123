using Microsoft.Xna.Framework;
using System;
using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions
{
  [Serializable]
  public class WalkAction : VillagerAction
  {
    private Map _map;

    public Vector2 Destination { get; init; }

    public WalkAction() { }

    public WalkAction(Villager villager, GameWorld gameWorld, Vector2 destination) : base(villager, gameWorld)
    {
      Destination = destination;
    }

    public override void Start()
    {
      var path = _map.FindPath(_villager.Position.ToPoint(), Destination.ToPoint());
    }

    protected override void OnInitialize()
    {
      _map = _gameWorld.Map;
    }

    public override void Update()
    {
      // Logic to update the walking progress
    }

    public override bool IsComplete()
    {
      // Logic to check if the villager has reached the destination
      return false;
    }
  }
}
