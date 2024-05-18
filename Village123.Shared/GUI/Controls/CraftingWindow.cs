using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Village123.Shared.Data;
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
      var startPosition = new Vector2(20, 40);
      for (int i = 0; i < items.Count; i++)
      {
        var item = items[i];
        AddChild(new Button(_font, texture, item.Name, startPosition + new Vector2(0, ((texture.Height) * i) + 20)), "temp");
      }
      IsOpen = true;
    }

    public override Action OnLeftClick => () => { };

    public override Action OnMouseOver => () => { };
  }
}
