using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Managers;

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

    public CraftAction(Villager villager, GameWorldManager gwm, Job job) : base(villager, gwm)
    {
      _job = job;
      _jobId = job.Id;
    }

    public override void Start()
    {

    }

    protected override void OnInitialize()
    {
      _job = JobManager.GetInstance().Jobs.FirstOrDefault(j => j.Id == _jobId);
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
      JobManager.GetInstance().CompleteJob(_villager, _job);

      // If the project was async this wouldn't work..
      var item = ItemManager.GetInstance(_gwm).Items.Last();

      var storage = PlaceManager.GetInstance(_gwm).GetPlacesByType("itemStorage")
        .FirstOrDefault(); // TODO: Make smart

      if (storage != null)
      {
        _villager.AddAction(new WalkAction(_villager, _gwm, item.Point, true));
        _villager.AddAction(new CarryAction(_villager, _gwm, item.Carriable));
        _villager.AddAction(new WalkAction(_villager, _gwm, storage.Point, false));
        _villager.AddAction(new StoreAction(_villager, _gwm, item, storage));
      }
    }
  }
}
