using Microsoft.Xna.Framework;
using Village123.Shared.Entities;

namespace Village123.Shared.Components
{
  public class CarriableComponent
  {
    public int? ItemId { get; set; }

    public int? VillagerId { get; set; }

    /// <summary>
    /// The villager carrying
    /// </summary>
    public int CarrierId { get; set; }

    public bool BeingCarried { get; set; } = false;

    public Vector2 Position { get; set; } = Vector2.Zero;

    public CarriableComponent()
    {

    }

    public CarriableComponent(Item item)
    {
      ItemId = item.Id;
    }

    public CarriableComponent(Villager villager)
    {
      VillagerId = villager.Id;
    }

    public void GetCarried(Villager carrier)
    {
      CarrierId = carrier.Id;
      BeingCarried = true;
    }

    public void Update()
    {
      if (!BeingCarried)
      {
        return;
      }
      Position = BaseGame.GWM.VillagerManager.Villagers.Find(v => v.Id == CarrierId).Position;
    }
  }
}
