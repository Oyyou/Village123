using Village123.Shared.Services;

namespace Village123.Shared.Managers
{
  public class IdManager
  {
    private const string fileName = "ids.json";
    private SaveFileService _saveFileService;

    public int VillagerId { get; set; } = 1;
    public int PlaceId { get; set; } = 1;
    public int JobId { get; set; } = 1;
    public int ItemId { get; set; } = 1;
    public int MaterialId { get; set; } = 1;
    public int ResourceId { get; set; } = 1;

    public IdManager()
    {

    }

    public IdManager(SaveFileService saveFileService)
    {
      _saveFileService = saveFileService;
    }

    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static IdManager Load(SaveFileService saveFileService)
    {
      var manager = saveFileService.Load<IdManager>(fileName);

      if (manager == null)
        return new IdManager(saveFileService);

      manager._saveFileService = saveFileService;
      return manager;
    }
  }
}
