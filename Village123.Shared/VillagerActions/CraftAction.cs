using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
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

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private List<int> _materialIds;

    public override string Name => "Craft";

    public CraftAction() { }

    public CraftAction(Villager villager, Job job, List<int> materialIds) : base(villager)
    {
      _job = job;
      _jobId = job.Id;
      _materialIds = materialIds;
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
        _job.Progress += 1;
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
        .OrderBy(s => Vector2.Distance(s.Position, item.Position))
        .FirstOrDefault(); // TODO: Make smart

      if (storage != null)
      {
        _villager.AddAction(new WalkAction(_villager, item.Point, true));
        _villager.AddAction(new CarryAction(_villager, item));
        _villager.AddAction(new WalkAction(_villager, storage.Point, false));
        _villager.AddAction(new StoreAction(_villager, item, storage));
      }

      var materials = BaseGame.GWM.MaterialManager.Materials;
      for (int i = 0; i < materials.Count; i++)
      {
        if (!_materialIds.Contains(materials[i].Id))
          continue;

        materials.RemoveAt(i);
        i--;
      }
    }
  }
}
