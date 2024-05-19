using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Interfaces;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  internal class CraftingWindow : Window
  {
    private ItemData.Item _selectedItem = null;
    private Dictionary<string, string> _selectedResources;

    public CraftingWindow(GameWorldManager gwm, Place place, Vector2 position, int width, int height)
      : base(gwm, position, width, height, "Crafting")
    {
      var items = _gwm.ItemData.GetItemsByCategory(place.Data.Category);

      var buttonWidth = 200;
      var buttonHeight = buttonWidth / 5;
      var buttonTexture = TextureHelpers.CreateBorderedTexture(_gwm.GameModel.GraphicsDevice, buttonWidth, buttonHeight, Color.White, Color.Black, 1);

      var sidePadding = 20;
      var gap = 10;

      AddChild(new Label(_font, "Items", new Vector2(sidePadding, 40)));
      var itemsStartPosition = new Vector2(sidePadding, 70);
      for (int i = 0; i < items.Count; i++)
      {
        var item = items[i];
        var button = new Button(_font, buttonTexture, item.Name, itemsStartPosition + new Vector2(0, (buttonTexture.Height + gap) * i));
        button.Key = item;
        button.OnClicked = () =>
        {
          var resourceAreaX = 260;
          _selectedItem = item;

          button.IsSelected = true;
          this.RemoveChildrenByTag("temp");
          AddChild(new Label(_font, "Resources required", new Vector2(resourceAreaX, 40)), "temp");
          var rrStartPosition = new Vector2(resourceAreaX, 70);
          _selectedResources = new Dictionary<string, string>();
          for (int i = 0; i < item.RequiredResources.Count; i++)
          {
            var resource = item.RequiredResources.ElementAt(i);
            var label = new Label(_font, $"{resource.Key} x{resource.Value}", rrStartPosition);
            AddChild(label, "temp");

            rrStartPosition.X += label.Width + gap;
            if (rrStartPosition.X > this.ClickRectangle.Right - sidePadding)
            {
              rrStartPosition = new Vector2(resourceAreaX, rrStartPosition.Y + (label.Height + gap));
            }

            _selectedResources.Add(resource.Key, "");
          }

          rrStartPosition = new Vector2(resourceAreaX, rrStartPosition.Y + (Children.Last().Height + gap));
          for (int i = 0; i < item.RequiredResources.Count; i++)
          {
            var resourceType = item.RequiredResources.ElementAt(i);
            var control = new ResourceOptions(
              _gwm,
              resourceType.Key, // resourceType
              (resource) => _selectedResources[resourceType.Key] = resource, // onResourceSelected
              (ClickRectangle.Width) - (resourceAreaX + sidePadding), // width
              rrStartPosition // position
            );
            AddChild(control, "temp");

            rrStartPosition = new Vector2(resourceAreaX, rrStartPosition.Y + (control.Height + gap));
          }
        };
        AddChild(button);
      }

      var craftButton = new Button(
        _font,
        buttonTexture,
        "Craft",
        new Vector2(
          width - (20 + buttonTexture.Width),
          height - (20 + buttonTexture.Height)
        ));

      AddChild(craftButton, "craftButton");

      IsOpen = true;
    }

    public override Action OnLeftClick => () => { };

    public override Action OnMouseOver => () => { };

    public override void Update(GameTime gameTime)
    {
      ((IClickable)this).UpdateMouse();

      foreach (var child in Children)
      {
        child.IsSelected = child.Key == _selectedItem;
        child.Update(gameTime);

        if (child.Tag == "craftButton")
        {
          child.IsVisible = _selectedItem != null && _selectedResources.All(r => !string.IsNullOrEmpty(r.Value));
        }
      }
    }
  }
}
