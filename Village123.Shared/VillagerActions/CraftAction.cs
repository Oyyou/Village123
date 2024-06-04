using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Linq;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  internal class CraftAction : VillagerAction
  {
    private Job _job;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private float _timer;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int _jobId;

    public override string Name => "Craft";

    public CraftAction() { }

    public CraftAction(Villager villager, Job job) : base(villager)
    {
      _job = job;
      _jobId = job.Id;
    }

    public override void Start()
    {

    }

    protected override void OnInitialize()
    {
      _job = BaseGame.GWM.JobManager.Jobs.FirstOrDefault(j => j.Id == _jobId);
    }

    public override void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer >= 0.01f)
      {
        _timer = 0;
        _job.Progress += 5;
      }
    }

    public override bool IsComplete()
    {
      return _job.Progress >= 100;
    }

    public override void OnComplete()
    {
      BaseGame.GWM.JobManager.CompleteJob(_villager, _job);

      // If the project was async this wouldn't work..
      var item = BaseGame.GWM.ItemManager.Items.Last();

      var storage = BaseGame.GWM.PlaceManager.GetPlacesByType("itemStorage")
        .FirstOrDefault(); // TODO: Make smart

      if (storage != null)
      {
        _villager.AddAction(new WalkAction(_villager, item.Point, true));
        _villager.AddAction(new CarryAction(_villager, item.Carriable));
        _villager.AddAction(new WalkAction(_villager, storage.Point, false));
        _villager.AddAction(new StoreAction(_villager, item, storage));
      }
    }
  }
}
