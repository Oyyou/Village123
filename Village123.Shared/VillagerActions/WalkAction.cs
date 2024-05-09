using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  [Serializable]
  public class WalkAction : VillagerAction
  {
    private Map _map;

    public Vector2 Destination { get; init; }

    public WalkAction(Villager villager, Map map, Vector2 destination) : base(villager)
    {
      _map = map;
      Destination = destination;
    }

    public override void Start()
    {
      var path = _map.FindPath(_villager.Position.ToPoint(), Destination.ToPoint());
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
      // Initialize _villager and _map here
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
