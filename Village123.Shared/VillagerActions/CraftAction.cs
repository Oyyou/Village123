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
      _job = JobManager.GetInstance(_gwm).Jobs.FirstOrDefault(j => j.Id == _jobId);
    }

    public override void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if (_timer >= 0.025f)
      {
        _timer = 0;
        _job.Progress++;
      }
    }

    public override bool IsComplete()
    {
      return _job.Progress >= 100;
    }

    public override void OnComplete()
    {
      JobManager.GetInstance(_gwm).CompleteJob(_villager, _job);
    }
  }
}
