using Microsoft.Xna.Framework;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Windows
{
  internal class ManageStorageWindow : Window
  {
    private string selectedTag = "all";
    public ManageStorageWindow(Place place, Vector2 position, int width, int height)
      : base(position, width, height, "Manage storage")
    {
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

      AddChild(new Label(_font, "Accept items from...", new Vector2(sidePadding, 40)));
      var itemListStartPosition = new Vector2(sidePadding, 70);
      var itemList = new ItemList(
        new Rectangle(
          (int)Position.X + sidePadding,
          (int)Position.Y + 70,
          150,
          300
        )
      );
      _itemLists.Add(itemList);

      var places = BaseGame.GWM.PlaceManager
        .GetPlacesByType("workstation")
        .Where(p => place.RadiusPoints.Contains(p.Point));

      itemList.AddChild(new Button(_font, buttonTexture, "--All--")
      {
        OnClicked = () =>
        {
          selectedTag = "all";
        }
      });

      foreach (var p in places)
      {
        itemList.AddChild(new Button(_font, buttonTexture, p.Name)
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
      AddChild(new Button(_font, buttonTexture, "Apply", new Vector2(sidePadding + ((itemList.Width - buttonWidth) / 2), (70 + itemList.Height) + 10))
      {
        OnClicked = () =>
        {

        }
      });

      IsOpen = true;
    }

    public override void Update(GameTime gameTime)
    {
      ClickableComponent.Update(gameTime);

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
