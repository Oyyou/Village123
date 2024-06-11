using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Village123.Shared.Components;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public class CarryAction : VillagerAction
  {
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _villagerId = null;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _itemId = null;

    [JsonIgnore]
    private CarriableComponent _carriable;

    public override string Name => "Carry";

    public CarryAction(Villager villager, CarriableComponent carriable)
      : base(villager)
    {
      _carriable = carriable;
    }

    protected override void OnInitialize()
    {
      //if (_villagerId.HasValue)
      //  _carriable = VillagerManager.GetInstance(BaseGame.GWM).Villagers.Find(v => v.Id == _villagerId.Value).Carriable;

      if (_itemId.HasValue)
        _carriable = BaseGame.GWM.ItemManager.Items.Find(v => v.Id == _itemId.Value).Carriable;
    }

    public override bool IsComplete()
    {
      return true;
    }

    public override void OnComplete()
    {
    }

    public override void Start()
    {
      _carriable.GetCarried(_villager);
    }

    public override void Update(GameTime gameTime)
    {

    }
  }
}
