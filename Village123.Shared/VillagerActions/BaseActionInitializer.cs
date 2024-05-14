using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions
{
  public class BaseActionInitializer : IVillagerActionInitializer
  {
    public void InitializeAction(Villager villager, GameWorld gameWord, IVillagerAction action)
    {
      if (action is WalkAction walkAction)
      {
        walkAction.Initialize(villager, gameWord.Map);
      }
    }
  }
}
