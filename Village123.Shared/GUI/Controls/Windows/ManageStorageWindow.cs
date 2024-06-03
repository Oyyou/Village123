using Microsoft.Xna.Framework;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Windows
{
  internal class ManageStorageWindow : Window
  {
    public ManageStorageWindow(GameWorldManager gwm, Place place, Vector2 position, int width, int height)
      : base(gwm, position, width, height, "Manage storage")
    {
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

      AddChild(new Label(_font, "Accept items from...", new Vector2(sidePadding, 40)));
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

      var places = PlaceManager.GetInstance(_gwm)
        .GetPlacesByType("workstation")
        .Where(p => place.RadiusPoints.Contains(p.Point));

      itemList.AddChild(new Button(_font, buttonTexture, "All")
      {

      });

      foreach (var p in places)
      {
        itemList.AddChild(new Button(_font, buttonTexture, p.Name)
        {

        });
      }

      IsOpen = true;
    }
  }
}
