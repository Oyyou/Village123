using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Village123.Shared.Entities;
using Village123.Shared.Maps;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions
{
  [Serializable]
  public class WalkAction : VillagerAction
  {
    private Map _map;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private List<Point> _path;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private float _timer = 0f;

    public Point Destination { get; init; }

    public bool StandOnDestination { get; init; }

    public override string Name => "Walk";

    public WalkAction() { }

    public WalkAction(Villager villager, GameWorld gameWorld, Point destination, bool standOnDestination) : base(villager, gameWorld)
    {
      Destination = destination;
      StandOnDestination = standOnDestination;
    }

    public override void Start()
    {
      var pf = new Pathfinder(_map);
      _path = StandOnDestination ?
        pf.GetPath(_villager.Point, Destination) :
        pf.GetPathNextTo(_villager.Point, Destination, true, true);

      _conditionManager.SetRate("Energy", -1.1f);
    }

    protected override void OnInitialize()
    {
      _map = _gameWorld.Map;
    }

    public override void Update()
    {
      _timer += 1f;

      if (_timer < 50f)
      {
        return;
      }

      _villager.Point = _path[0];
      // _villager.Position = _path[0].ToVector2() * BaseGame.TileSize;
      _path.RemoveAt(0);

      _timer = 0f;
    }

    public override bool IsComplete()
    {
      return _path.Count == 0;
    }

    public override void OnComplete()
    {
      _conditionManager.ResetCondition("Energy");
    }
  }
}
