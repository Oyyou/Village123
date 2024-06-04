using Microsoft.Xna.Framework;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public interface IVillagerAction
  {
    string Name { get; }
    void Initialize(Villager villager);
    bool Started { get; set; }
    void Start();
    void Update(GameTime gameTime);
    bool IsComplete();
    void OnComplete();
  }
}
