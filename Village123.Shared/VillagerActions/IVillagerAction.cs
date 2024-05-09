namespace Village123.Shared.VillagerActions
{
  public interface IVillagerAction
  {
    bool Started { get; set; }
    void Start();
    void Update();
    bool IsComplete();
  }
}
