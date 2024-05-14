using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.Interfaces
{
  public interface IDetermineAction
  {
    string Name { get; }
    float Priority { get; }
    bool CanExecute(Villager villager, GameWorld gameWorld);
    void Execute(Villager villager, GameWorld gameWorld);
  }
}
