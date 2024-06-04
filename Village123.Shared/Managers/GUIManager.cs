using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.GUI.Controls.Windows;

namespace Village123.Shared.Managers
{
  public class GUIManager
  {
    private readonly GameWorldManager _gwm;

    private readonly BuildPanel _buildPanel;
    private Window _window;
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

      if (_window != null && !_window.IsOpen)
      {
        _window = null;
      }

      _buildPanel.Update(gameTime);
      _window?.Update(gameTime);
      _placeOptionsPanel?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _buildPanel.Draw(spriteBatch);
      _window?.Draw(spriteBatch);
      _placeOptionsPanel?.Draw(spriteBatch);
    }

    public void HandlePlaceClicked(Place place)
    {
      _placeOptionsPanel = new PlaceOptionsPanel(_gwm, place, place.Position);
    }

    public void HandleCraftClicked(Place place)
    {
      _window = new CraftingWindow(
        _gwm,
        place,
        new Vector2(100, 100),
        BaseGame.ScreenWidth - 200,
        BaseGame.ScreenHeight - 200,
        (item) =>
        {
          JobManager.GetInstance().Add(place, item);
        }
      );
    }

    public void HandleManageStorageClicked(Place place)
    {
      _window = new ManageStorageWindow(
        _gwm,
        place,
        new Vector2(100, 100),
        BaseGame.ScreenWidth - 200,
        BaseGame.ScreenHeight - 200
      );
    }
  }
}
