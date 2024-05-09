using Village123.Shared.Entities;
using Village123.Shared.Models;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Interfaces
{
  public interface IVillagerActionInitializer
  {
    void InitializeAction(Villager villager, GameWorld gameWorld, IVillagerAction action);
  }
}
