using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Village123.Shared.Entities;
using Village123.Shared.Maps;

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

    public WalkAction(Villager villager, Point destination, bool standOnDestination)
      : base(villager)
    {
      Destination = destination;
      StandOnDestination = standOnDestination;
    }

    public override void Start()
    {
      var pf = new Pathfinder(_map);
      _path = StandOnDestination ?
        pf.GetPath(_villager.Point, Destination) :
        pf.GetPathNextTo(_villager.Point, Destination, false, true);

      _conditionManager.SetRate("Energy", -0.15f);
    }

    protected override void OnInitialize()
    {
      _map = BaseGame.GWM.Map;
    }

    public override void Update(GameTime gameTime)
    {
      _timer += 1f;

      if (_path.Count > 0)
      {
        var nextPoint = _path[0];
        _villager.PositionOffset = new Vector2(
          (nextPoint.X - _villager.Point.X) * _timer,
          (nextPoint.Y - _villager.Point.Y) * _timer
        );
      }

      if (_timer < BaseGame.TileSize)
      {
        return;
      }

      _villager.Point = _path[0];
      _villager.PositionOffset = Vector2.Zero;
      _path.RemoveAt(0);

      _timer = 0f;
    }

    public override bool IsComplete()
    {
      return _path.Count == 0;
    }

    public override void OnComplete()
    {
      _villager.PositionOffset = Vector2.Zero;
      _conditionManager.ResetCondition("Energy");
    }
  }
}
