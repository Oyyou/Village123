using Microsoft.Xna.Framework;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public class TalkAction : VillagerAction
  {
    private Villager _receivingVillager;
    private float timer;

    public override string Name => "Talking";

    public bool ForceComplete { get; set; } = false;

    public TalkAction(Villager villager, Villager receivingVillager) : base(villager)
    {
      _receivingVillager = receivingVillager;
    }

    public override void Start()
    {
      _conditionManager.SetRate("Social", 0.25f);
    }

    protected override void OnInitialize()
    {

    }

    public override void Update(GameTime gameTime)
    {
      timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override bool IsComplete()
    {
      return timer >= 10 || ForceComplete;
    }

    public override void OnComplete()
    {
      _conditionManager.ResetCondition("Social");
      _villager.SetCooldown("CanTalk", 15);
    }
  }
}
