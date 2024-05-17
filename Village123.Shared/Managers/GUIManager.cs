using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls;

namespace Village123.Shared.Managers
{
  public class GUIManager
  {
    private readonly GameWorldManager _gwm;

    private readonly BuildPanel _buildPanel;
    private readonly CraftingWindow _craftingWindow;
    private PlaceOptionsPanel _placeOptionsPanel;

    public GUIManager(GameWorldManager gwm)
    {
      _gwm = gwm;

      _buildPanel = new BuildPanel(_gwm);
      _craftingWindow = new CraftingWindow(
        _gwm,
        new Vector2(100, 100),
        BaseGame.ScreenWidth - 200,
        BaseGame.ScreenHeight - 200
      );
    }

    public void Update(GameTime gameTime)
    {
      if (_placeOptionsPanel != null && _placeOptionsPanel.Closed)
      {
        _placeOptionsPanel = null;
      }
      _buildPanel.Update(gameTime);
      _craftingWindow.Update(gameTime);
      _placeOptionsPanel?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
      _buildPanel.Draw(spriteBatch);
      _craftingWindow.Draw(spriteBatch);
      _placeOptionsPanel?.Draw(spriteBatch);
      spriteBatch.End();
    }

    public void HandlePlaceClicked(Place place)
    {
      // if (place.Data.Category == "crafting")
      {
        // _craftingWindow.SetPlace(place.Data);
        _placeOptionsPanel = new PlaceOptionsPanel(_gwm, place, place.Position);
      }
    }
  }
}
