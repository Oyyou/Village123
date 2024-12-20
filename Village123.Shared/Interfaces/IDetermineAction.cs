using Village123.Shared.Entities;

namespace Village123.Shared.Interfaces
{
  public interface IDetermineAction
  {
    string Name { get; }
    float Priority(Villager villager);
    bool CanExecute(Villager villager);
    void Execute(Villager villager);
  }
}
