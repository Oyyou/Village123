using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  internal class CraftingWindow : Window
  {
    public CraftingWindow(GameWorldManager gwm, Place place, Vector2 position, int width, int height)
      : base(gwm, position, width, height, "Crafting")
    {
      var items = _gwm.ItemData.GetItemsByCategory(place.Data.Category);

      var buttonWidth = 200;
      var buttonHeight = buttonWidth / 5;
      var texture = TextureHelpers.CreateBorderedTexture(_gwm.GameModel.GraphicsDevice, buttonWidth, buttonHeight, Color.White, Color.Black, 1);

      AddChild(new Label(_font, "Items", new Vector2(20, 40)));
      var itemsStartPosition = new Vector2(20, 70);
      for (int i = 0; i < items.Count; i++)
      {
        var item = items[i];
        var button = new Button(_font, texture, item.Name, itemsStartPosition + new Vector2(0, (texture.Height + 10) * i));
        button.OnClicked = () =>
        {
          button.Selected = true;
          this.RemoveChildrenByTag(item.Name);
          AddChild(new Label(_font, "Resources required", new Vector2(260, 40)), item.Name);
          var rrStartPosition = new Vector2(260, 70);
          for (int i = 0; i < item.RequiredResources.Count; i++)
          {
            var resource = item.RequiredResources.ElementAt(i);
            var label = new Label(_font, $"{resource.Key} x{resource.Value}", rrStartPosition);
            AddChild(label, item.Name);

            rrStartPosition.X += label.Width + 20;
            if (rrStartPosition.X > this.ClickRectangle.Right - 20)
            {
              rrStartPosition = new Vector2(260, rrStartPosition.Y + (label.Height + 10));
            }
          }
        };
        button.OnClickedOutside = () =>
        {
          this.RemoveChildrenByTag(item.Name);
        };
        AddChild(button);
      }

      IsOpen = true;
    }

    public override Action OnLeftClick => () => { };

    public override Action OnMouseOver => () => { };
  }
}
