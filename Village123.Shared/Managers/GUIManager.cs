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
    private CraftingWindow _craftingWindow;
    private PlaceOptionsPanel _placeOptionsPanel;

    public GUIManager(GameWorldManager gwm)
    {
      _gwm = gwm;

      _buildPanel = new BuildPanel(_gwm);
    }

    public void Update(GameTime gameTime)
    {
      if (_placeOptionsPanel != null && _placeOptionsPanel.Closed)
      {
        _placeOptionsPanel = null;
      }

      if (_craftingWindow != null && !_craftingWindow.IsOpen)
      {
        _craftingWindow = null;
      }

      _buildPanel.Update(gameTime);
      _craftingWindow?.Update(gameTime);
      _placeOptionsPanel?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _buildPanel.Draw(spriteBatch);
      _craftingWindow?.Draw(spriteBatch);
      _placeOptionsPanel?.Draw(spriteBatch);
    }

    public void HandlePlaceClicked(Place place)
    {
      _placeOptionsPanel = new PlaceOptionsPanel(_gwm, place, place.Position);
    }

    public void HandleCraftClicked(Place place)
    {
      _craftingWindow = new CraftingWindow(
        _gwm,
        place,
        new Vector2(100, 100),
        BaseGame.ScreenWidth - 200,
        BaseGame.ScreenHeight - 200
      );
    }
  }
}
