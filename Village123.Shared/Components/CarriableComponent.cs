using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using Village123.Shared.Entities;

namespace Village123.Shared.Components
{
  public class CarriableComponent
  {
    /// <summary>
    /// The item being carried
    /// </summary>
    public int? ItemId { get; set; }

    /// <summary>
    /// The villager being carried
    /// </summary>
    public int? VillagerId { get; set; }

    /// <summary>
    /// The material being carried
    /// </summary>
    public int? MaterialId { get; set; }

    /// <summary>
    /// The villager carrying
    /// </summary>
    public int? CarrierId { get; set; }

    public bool BeingCarried { get; set; } = false;

    public Vector2 Position { get; set; } = Vector2.Zero;

    [JsonIgnore]
    public Action OnPickup { get; set; } = null;

    [JsonIgnore]
    public Action<Point> OnDrop { get; set; } = null;

    public CarriableComponent()
    {

    }

    public CarriableComponent(Material material)
    {
      MaterialId = material.Id;
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

      OnPickup?.Invoke();
    }

    public void Drop(Point point)
    {
      CarrierId = null;
      BeingCarried = false;

      OnDrop?.Invoke(point);
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
