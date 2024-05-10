using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions
{
  public class SleepAction : VillagerAction
  {
    public override string Name => "Sleep";

    public SleepAction() { }

    public SleepAction(Villager villager, GameWorld gameWorld) : base(villager, gameWorld)
    {

    }

    public override void Start()
    {
      _conditionManager.SetRate("Energy", 1f);
    }

    protected override void OnInitialize()
    {
    }

    public override void Update()
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
