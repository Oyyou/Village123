using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Models;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Windows
{
  internal class CraftingWindow : Window
  {
    private KeyValuePair<string, ItemData.Item> _selectedItem;
    private Dictionary<string, string> _selectedOptions;

    public CraftingWindow(
      Place place,
      Vector2 position,
      int width,
      int height,
      Action<CraftItemModel> craftItem
      )
      : base(position, width, height, "Crafting")
    {
      var items = BaseGame.GWM.ItemData.GetItemsByCategory(place.Data.Category);

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

      AddChild(new Label(_font, "Items", new Vector2(sidePadding, 40)));
      var itemListStartPosition = new Vector2(sidePadding, 70);
      var itemList = new ItemList(
        new Rectangle(
          (int)Position.X + sidePadding,
          (int)Position.Y + 70,
          150,
          300
        )
      );
      AddItemList(itemList);


      var x = itemList.Viewport.Right + sidePadding;
      var optionsList = new ItemList(
        new Rectangle(
          x,
          itemList.Viewport.Top,
          Width - x + (int)Position.X - sidePadding,
          300
        )
      );

      AddItemList(optionsList);

      for (int i = 0; i < items.Count; i++)
      {
        var item = items.ElementAt(i);
        var itemValue = item.Value;
        var button = new Button(
          _font,
          buttonTexture,
          item.Value.Name
        );
        button.IsSelected = () => button.Key == _selectedItem.Value;
        button.Key = itemValue;
        button.OnClicked = () =>
        {
          var materialAreaX = 200;
          _selectedItem = item;

          RemoveChildrenByTag("temp");
          optionsList.RemoveChildrenByTag("temp");
          var str = "Materials required - ";
          _selectedOptions = new Dictionary<string, string>();
          for (int i = 0; i < itemValue.RequiredMaterials.Count; i++)
          {
            var option = itemValue.RequiredMaterials.ElementAt(i);
            str += $"{option.Key} x{option.Value} ";
            _selectedOptions.Add(option.Key, "");
          }
          AddChild(new Label(_font, str, new Vector2(materialAreaX, 40)), "temp");

          for (int i = 0; i < itemValue.RequiredMaterials.Count; i++)
          {
            var optionType = itemValue.RequiredMaterials.ElementAt(i);
            var control = new MaterialOptions(
              optionType.Key,
              (option) => _selectedOptions[optionType.Key] = option, // onOptionSelected
              optionsList.Width - 35 // width
            );

            optionsList.AddChild(control, "temp");
          }
        };

        itemList.AddChild(button);
      }

      var craftButton = new Button(
        _font,
        buttonTexture,
        "Craft",
        new Vector2(
          width - (20 + buttonTexture.Width),
          height - (20 + buttonTexture.Height)
        ));
      craftButton.OnClicked = () =>
      {
        craftItem(new CraftItemModel()
        {
          Item = _selectedItem,
          Materials = _selectedOptions,
        });
      };

      AddChild(craftButton, "craftButton");
      AddChild(
        new CraftQueue(
          place,
          ClickRectangle.Width - sidePadding * 2,
          50
        )
        {
          Position = new Vector2(
            20,
            itemListStartPosition.Y + itemList.Height + 20
          ),
        }
      );

      IsOpen = true;
    }

    public override void Update(GameTime gameTime)
    {
      ClickableComponent.Update(gameTime);

      foreach (var child in Children)
      {
        child.Update(gameTime);

        if (child.Tag == "craftButton")
        {
          child.IsVisible = !string.IsNullOrEmpty(_selectedItem.Key) && _selectedOptions.All(r => !string.IsNullOrEmpty(r.Value));
        }
      }

      foreach (var itemList in _itemLists)
      {
        itemList.Update(gameTime);
      }
    }
  }
}
