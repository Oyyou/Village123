using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.VillagerActions
{
  public interface IVillagerAction
  {
    void Initialize(Villager villager, GameWorld gameWorld);
    bool Started { get; set; }
    void Start();
    void Update();
    bool IsComplete();
  }
}
