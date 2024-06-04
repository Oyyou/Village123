using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.Managers;

namespace Village123.Shared.VillagerActions
{
  public class StoreAction : VillagerAction
  {
    private Item _item;
    private Place _place;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int _itemId;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int _placeId;

    public override string Name => "Store";

    public StoreAction()
    {

    }

    public StoreAction(Villager villager, GameWorldManager gwm, Item item, Place place)
      : base(villager, gwm)
    {
      _item = item;
      _place = place;

      _itemId = _item.Id;
      _placeId = _place.Id;
    }

    protected override void OnInitialize()
    {
      _item = ItemManager.GetInstance(_gwm).Items.FirstOrDefault(i => i.Id == _itemId);
      _place = PlaceManager.GetInstance(_gwm).Places.FirstOrDefault(p => p.Id == _placeId);

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
      _item.Carriable.BeingCarried = false;
      _item.StorageId = _place.Id;
    }

    public override void Update(GameTime gameTime)
    {

    }
  }
}
