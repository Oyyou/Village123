using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Events;
using Village123.Shared.Interfaces;
using Village123.Shared.Managers;
using Village123.Shared.Maps;
using Village123.Shared.Models.WaitTypes;

namespace Village123.Shared.VillagerActions.DetermineActions
{
  public class DetermineTalkAction : IDetermineAction
  {
    private int _jobId;

    public string Name => "Talk";

    public float Priority(Villager villager)
    {
      var offset = 0f;
      var social = villager.Conditions["Social"].Value;
      switch (social)
      {
        case < 25:
          offset = 2f;
          break;
        case < 50:
          offset = 1f;
          break;
      }

      return 3f - offset;
    }

    public bool CanExecute(Villager villager)
    {
      if (villager.ActionQueue.Count > 0)
        return false;

      if (villager.GetCooldown("CanTalk").Duration > 0)
        return false;

      return GetValidTalkReceivers(villager).Any();
    }

    public void Execute(Villager villager)
    {
      // The person this villager is going to speak to
      var talkReceiver = GetValidTalkReceivers(villager).FirstOrDefault();
      if (talkReceiver == null)
        return;

      // TODO: Find a social spot to talk at (bar or whatever)
      // TODO: Decide on the points to go to before doing the rest

      var pf = new Pathfinder(BaseGame.GWM.Map);
      var path = pf.GetPathNextTo(villager.Point, talkReceiver.Point, false, true);

      if (!path.Any())
      {
        return;
      }

      var walkAction1 = new WalkAction(villager, path.Last(), true);
      var walkAction2 = new WalkAction(talkReceiver, talkReceiver.Point, true);

      villager.AddAction(walkAction1);
      villager.AddAction(new WaitAction(villager, new WaitForVillagerToArrive(talkReceiver, walkAction2.Destination)));
      villager.AddAction(new TalkAction(villager, talkReceiver));

      talkReceiver.AddAction(walkAction2);
      talkReceiver.AddAction(new WaitAction(talkReceiver, new WaitForVillagerToArrive(villager, walkAction1.Destination)));
      talkReceiver.AddAction(new TalkAction(talkReceiver, villager));

      BaseGame.GWM.EventManager.Add(new ConversationEvent(villager, talkReceiver));
    }

    private IEnumerable<Villager> GetValidTalkReceivers(Villager villager)
    {
      return BaseGame.GWM.VillagerManager.Villagers
          .Where(v =>
              v.Id != villager.Id &&
              v.GetCooldown("CanTalk").Duration <= 0 &&
              !v.ActionQueue.OfType<TalkAction>().Any());
    }
  }
}
