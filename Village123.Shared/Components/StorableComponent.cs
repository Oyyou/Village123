using Village123.Shared.Entities;

namespace Village123.Shared.Components
{
  public class StorableComponent
  {
    public int PlaceId { get; set; }
    public string TexturePath { get; set; }

    public int? ItemId { get; set; }

    public int? MaterialId { get; set; }

    public bool IsStored { get; set; } = false;

    public StorableComponent()
    {

    }

    public void Store(Place place, string texturePath)
    {
      PlaceId = place.Id;
      TexturePath = texturePath;
      IsStored = true;
    }
  }
}
