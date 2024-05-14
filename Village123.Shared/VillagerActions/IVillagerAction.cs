using Village123.Shared.Entities;
using Village123.Shared.Managers;

namespace Village123.Shared.VillagerActions
{
  public interface IVillagerAction
  {
    string Name { get; }
    void Initialize(Villager villager, GameWorldManager gwm);
    bool Started { get; set; }
    void Start();
    void Update();
    bool IsComplete();
    void OnComplete();
  }
}
