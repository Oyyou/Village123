using Microsoft.Xna.Framework;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public class SleepAction : VillagerAction
  {
    public override string Name => "Sleep";

    public SleepAction() { }

    public SleepAction(Villager villager) : base(villager)
    {

    }

    public override void Start()
    {
      _conditionManager.SetRate("Energy", 0.25f);
    }

    protected override void OnInitialize()
    {
    }

    public override void Update(GameTime gameTime)
    {

    }

    public override bool IsComplete()
    {
      return _conditionManager.GetValue("Energy") >= 100;
    }

    public override void OnComplete()
    {
      _conditionManager.ResetCondition("Energy");
    }
  }
}
