using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Linq;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public class HarvestAction : VillagerAction
  {
    private Job _job;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private float _timer;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int _jobId;

    public override string Name => "Harvest";

    public HarvestAction() { }

    public HarvestAction(Villager villager, Job job) : base(villager)
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

      // Calculate elapsed game time based on the game's time scale
      var elapsedRealSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
      var elapsedGameSeconds = elapsedRealSeconds * 2; // 2 game minutes per real second

      // Increment the timer by the elapsed game time in seconds
      _timer += elapsedGameSeconds;

      var harvestTime = _job.HarvestedResource.HarvestTime;

      // Calculate progress based on the percentage of harvest time completed
      _job.Progress = MathHelper.Clamp((_timer / harvestTime) * 100, 0, 100);

      //if (_timer >= 0.01f)
      //{
      //  _timer = 0;
      //  _job.Progress += 1;
      //}
    }

    public override bool IsComplete()
    {
      return _job.Progress >= 100;
    }

    public override void OnComplete()
    {
      BaseGame.GWM.JobManager.CompleteJob(_villager, _job);

      BaseGame.GWM.ResourceManager.RemoveById(_job.HarvestedResource.ResourceId);

      // If the project was async this wouldn't work..
      var material = BaseGame.GWM.MaterialManager.Materials.Last();

      var storage = BaseGame.GWM.PlaceManager.GetPlacesByType("itemStorage")
        .OrderBy(s => Vector2.Distance(s.Position, material.Position))
        .FirstOrDefault(); // TODO: Make smart

      if (storage != null)
      {
        _villager.AddAction(new WalkAction(_villager, material.Point, true));
        _villager.AddAction(new CarryAction(_villager, material));
        _villager.AddAction(new WalkAction(_villager, storage.Point, false));
        _villager.AddAction(new StoreAction(_villager, material, storage));
      }
    }
  }
}
