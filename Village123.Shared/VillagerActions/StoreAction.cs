using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Linq;
using Village123.Shared.Components;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public class StoreAction : VillagerAction
  {
    private CarriableComponent _carriable;
    private StorableComponent _storable;
    private Place _storagePlace;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private string _texturePath;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _itemId;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _materialId;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int _storagePlaceId;

    public override string Name => "Store";

    public StoreAction()
    {

    }

    public StoreAction(
      Villager villager,
      Item item,
      Place storagePlace)
      : base(villager)
    {
      _carriable = item.Carriable;
      _storable = item.Storable;
      _texturePath = $"Items/{item.Data.Key}";
      _storagePlace = storagePlace;

      _itemId = item.Id;
      _storagePlaceId = _storagePlace.Id;
    }

    public StoreAction(
      Villager villager,
      Material material,
      Place storagePlace)
      : base(villager)
    {
      _carriable = material.Carriable;
      _storable = material.Storable;
      _texturePath = $"Materials/{material.Data.Key}";
      _storagePlace = storagePlace;

      _materialId = material.Id;
      _storagePlaceId = _storagePlace.Id;
    }

    protected override void OnInitialize()
    {
      _storagePlace = BaseGame.GWM.PlaceManager.Places.FirstOrDefault(p => p.Id == _storagePlaceId);

      if (_itemId.HasValue)
      {
        var item = BaseGame.GWM.ItemManager.Items.Find(i => i.Id == _itemId.Value);
        _carriable = item.Carriable;
        _storable = item.Storable;
      }

      if (_materialId.HasValue)
      {
        var material = BaseGame.GWM.MaterialManager.Materials.Find(m => m.Id == _materialId.Value);
        _carriable = material.Carriable;
        _storable = material.Storable;
      }
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
      _carriable.Drop(_storagePlace.Point);
      _storable.Store(_storagePlace, _texturePath);
    }

    public override void Update(GameTime gameTime)
    {

    }
  }
}
