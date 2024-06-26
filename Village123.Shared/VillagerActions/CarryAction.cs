﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Village123.Shared.Components;
using Village123.Shared.Entities;

namespace Village123.Shared.VillagerActions
{
  public class CarryAction : VillagerAction
  {
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _itemId = null;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private int? _materialId;

    [JsonIgnore]
    private CarriableComponent _carriable;

    public override string Name => "Carry";

    public CarryAction(Villager villager, Item item)
      : base(villager)
    {
      _itemId = item.Id;
      _carriable = item.Carriable;
    }

    public CarryAction(Villager villager, Material material)
      : base(villager)
    {
      _materialId = material.Id;
      _carriable = material.Carriable;
    }

    protected override void OnInitialize()
    {
      //if (_villagerId.HasValue)
      //  _carriable = VillagerManager.GetInstance(BaseGame.GWM).Villagers.Find(v => v.Id == _villagerId.Value).Carriable;

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
      _carriable.GetCarried(_villager);
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
