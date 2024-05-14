namespace Village123.Shared.Interfaces
{
  public interface IPersistable<T>
  {
    static T Load() => default;
    void Save();
  }
}
