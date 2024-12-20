using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Entities;

namespace Village123.Shared.Events
{
  public class ConversationEvent : BaseEvent
  {
    private List<Villager> _villagers;

    public List<int> VillagerIds { get; set; } = new List<int>();

    public ConversationEvent()
    {
      Initialise();
    }

    public ConversationEvent(params Villager[] villagers)
    {
      _villagers = new List<Villager>(villagers);

      VillagerIds = _villagers.Select(v => v.Id).ToList();
    }

    public void Initialise()
    {
      _villagers = BaseGame.GWM.VillagerManager.Villagers.Where(v => VillagerIds.Contains(v.Id)).ToList();
    }

    public override void Update(GameTime gameTime)
    {

    }
  }
}
