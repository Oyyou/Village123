using Village123.Shared.Entities;
using Village123.Shared.Managers;

namespace Village123.Shared.Interfaces
{
  public interface IDetermineAction
  {
    string Name { get; }
    float Priority { get; }
    bool CanExecute(Villager villager, GameWorldManager gwm);
    void Execute(Villager villager, GameWorldManager gwm);
  }
}
