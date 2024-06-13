using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Village123.Shared.Components;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public class DropAction : VillagerAction
  {
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private Point _point;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _itemId = null;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _materialId;

    [JsonIgnore]
    private CarriableComponent _carriable;

    public override string Name => "Drop";

    public DropAction(Villager villager, Point point, Item item)
      : base(villager)
    {
      _point = point;
      _itemId = item.Id;
      _carriable = item.Carriable;
    }

    public DropAction(Villager villager, Point point, Material material)
      : base(villager)
    {
      _point = point;
      _materialId = material.Id;
      _carriable = material.Carriable;
    }

    protected override void OnInitialize()
    {
      if (_itemId.HasValue)
      {
        _carriable = BaseGame.GWM.ItemManager.Items.Find(v => v.Id == _itemId.Value).Carriable;
      }

      if (_materialId.HasValue)
      {
        _carriable = BaseGame.GWM.MaterialManager.Materials.Find(v => v.Id == _itemId.Value).Carriable;
      }
    }

    public override void Start()
    {
      _carriable.Drop(_point);
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override bool IsComplete()
    {
      return true;
    }

    public override void OnComplete()
    {
    }
  }
}