using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Managers;
using Village123.Shared.Models;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Windows
{
    internal class CraftingWindow : Window
    {
        private KeyValuePair<string, ItemData.Item> _selectedItem;
        private Dictionary<string, string> _selectedResources;

        public CraftingWindow(
          GameWorldManager gwm,
          Place place,
          Vector2 position,
          int width,
          int height,
          Action<CraftItemModel> craftItem
          )
          : base(gwm, position, width, height, "Crafting")
        {
            var items = _gwm.ItemData.GetItemsByCategory(place.Data.Category);

            var buttonWidth = 115;
            var buttonHeight = 30;
            var buttonTexture = TextureHelpers.CreateBorderedTexture(
              _gwm.GameModel.GraphicsDevice,
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
              _gwm,
              new Rectangle(
                (int)Position.X + sidePadding,
                (int)Position.Y + 70,
                150,
                300
              )
            );
            _itemLists.Add(itemList);


            var x = itemList.Viewport.Right + sidePadding;
            var resourcesList = new ItemList(
              _gwm,
              new Rectangle(
                x,
                itemList.Viewport.Top,
                Width - x + (int)Position.X - sidePadding,
                300
              )
            );

            _itemLists.Add(resourcesList);

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
                    var resourceAreaX = 200;
                    _selectedItem = item;

                    RemoveChildrenByTag("temp");
                    resourcesList.RemoveChildrenByTag("temp");
                    var str = "Resources required - ";
                    _selectedResources = new Dictionary<string, string>();
                    for (int i = 0; i < itemValue.RequiredResources.Count; i++)
                    {
                        var resource = itemValue.RequiredResources.ElementAt(i);
                        str += $"{resource.Key} x{resource.Value} ";
                        _selectedResources.Add(resource.Key, "");
                    }
                    AddChild(new Label(_font, str, new Vector2(resourceAreaX, 40)), "temp");

                    for (int i = 0; i < itemValue.RequiredResources.Count; i++)
                    {
                        var resourceType = itemValue.RequiredResources.ElementAt(i);
                        var control = new ResourceOptions(
                  _gwm,
                  resourceType.Key, // resourceType
                  (resource) => _selectedResources[resourceType.Key] = resource, // onResourceSelected
                  resourcesList.Width - 35 // width
                );

                        resourcesList.AddChild(control, "temp");
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
                    Resources = _selectedResources,
                });
            };

            AddChild(craftButton, "craftButton");
            AddChild(
              new CraftQueue(
                _gwm,
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
                    child.IsVisible = !string.IsNullOrEmpty(_selectedItem.Key) && _selectedResources.All(r => !string.IsNullOrEmpty(r.Value));
                }
            }

            foreach (var itemList in _itemLists)
            {
                itemList.Update(gameTime);
            }
        }
    }
}
