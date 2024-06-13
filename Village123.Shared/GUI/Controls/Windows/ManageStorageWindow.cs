using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Components;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Sprites;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Windows
{
  internal class ManageStorageWindow : Window
  {
    private readonly Place _place;

    private string selectedTag = "all";

    private ItemList _inventoryList;
    private List<StorableComponent> _storables = new List<StorableComponent>();

    public ManageStorageWindow(Place place, Vector2 position, int width, int height)
      : base(position, width, height, "Manage storage")
    {
      _place = place;

      var buttonWidth = 115;
      var buttonHeight = 30;
      var buttonTexture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        buttonWidth,
        buttonHeight,
        Color.White,
        Color.Black,
        1
      );

      var sidePadding = 20;

      AddChild(new Label(_font, "Accept items from:", new Vector2(sidePadding, 40)));
      var itemListStartPosition = new Vector2(sidePadding, 70);
      var availablePlacesList = new ItemList(
        new Rectangle(
          (int)Position.X + sidePadding,
          (int)Position.Y + 70,
          150,
          300
        )
      );
      AddItemList(availablePlacesList);

      var places = BaseGame.GWM.PlaceManager
        .GetPlacesByType("workstation")
        .Where(p => _place.RadiusPoints.Contains(p.Point));

      availablePlacesList.AddChild(new Button(_font, buttonTexture, "--All--")
      {
        OnClicked = () =>
        {
          selectedTag = "all";
        }
      });

      foreach (var p in places)
      {
        availablePlacesList.AddChild(new Button(_font, buttonTexture, p.Name)
        {
          OnClicked = () =>
          {
            selectedTag = p.Name;
          },
          IsSelected = () =>
          {
            return p.Name == selectedTag || selectedTag == "all";
          }
        }, p.Name);
      }
      AddChild(new Button(_font, buttonTexture, "Apply", new Vector2(sidePadding + ((availablePlacesList.Width - buttonWidth) / 2), (70 + availablePlacesList.Height) + 10))
      {
        OnClicked = () =>
        {

        }
      });

      _inventoryList = new ItemList(
        new Rectangle(
          (int)Position.X + sidePadding + availablePlacesList.Width + sidePadding,
          (int)Position.Y + 70,
          720,
          300
        )
      );
      AddItemList(_inventoryList);

      var itemButtonTexture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        64,
        64,
        Color.White,
        Color.Black,
        1
      );

      UpdateInventoryItems();

      IsOpen = true;
    }

    private void UpdateInventoryItems()
    {
      var previousStorables = new List<StorableComponent>(_storables);
      _storables = BaseGame.GWM.ItemManager.Items.Select(i => i.Storable)
        .Where(s => s.PlaceId == _place.Id)
        .ToList();
      _storables.AddRange(BaseGame.GWM.MaterialManager.Materials.Select(i => i.Storable).Where(s => s.PlaceId == _place.Id));

      var isDifferent = !_storables.SequenceEqual(previousStorables);

      if (isDifferent)
      {
        _inventoryList.RemoveChildrenByTag("inventoryItem");
        foreach (var item in _storables)
        {
          _inventoryList.AddChild(new Button(new Sprite(TextureHelpers.LoadTexture(item.TexturePath)) { Scale = 2f }), "inventoryItem");
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      ClickableComponent.Update(gameTime);

      UpdateInventoryItems();

      foreach (var child in Children)
      {
        child.Update(gameTime);
      }

      foreach (var itemList in _itemLists)
      {
        itemList.Update(gameTime);
      }
    }
  }
}
