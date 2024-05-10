using Village123.Shared.Models;

namespace Village123.Shared.Interfaces
{
  public interface IPersistable<T>
  {
    static T Load(GameWorld gameWorld) => default;
    void Save();
  }
}
