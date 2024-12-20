namespace Village123.Shared.Models.WaitTypes
{
  public interface IWaitType
  {
    void Initialize();
    bool IsComplete();
  }
}
