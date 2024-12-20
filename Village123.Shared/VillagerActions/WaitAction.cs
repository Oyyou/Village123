using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using Village123.Shared.Entities;
using Village123.Shared.Models.WaitTypes;

namespace Village123.Shared.VillagerActions
{
  public class WaitAction : VillagerAction
  {
    [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
    public IWaitType WaitType { get; set; }

    public override string Name => "Waiting";

    public WaitAction()
    {

    }

    public WaitAction(Villager villager, IWaitType waitType) : base(villager)
    {
      WaitType = waitType;
    }

    protected override void OnInitialize()
    {
      WaitType?.Initialize();
    }

    public override void Start()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override bool IsComplete()
    {
      return WaitType.IsComplete();
    }

    public override void OnComplete()
    {
    }
  }
}
