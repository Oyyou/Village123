using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.GUI.Controls.Windows;
using Village123.Shared.Models;

namespace Village123.Shared.Managers
{
  public class GUIManager
  {
    private readonly BuildPanel _buildPanel;
    private Window _window;
    private PlaceOptionsPanel _placeOptionsPanel;
    private ContextMenu _contextMenu;

    public GUIManager()
    {
      _buildPanel = new BuildPanel();
    }

    public void Update(GameTime gameTime)
    {
      if (BaseGame.GWM.State != GameStates.Playing)
      {
        return;
      }

      if (_placeOptionsPanel != null && _placeOptionsPanel.Closed)
      {
        _placeOptionsPanel = null;
      }

      if (_contextMenu != null && _contextMenu.Closed)
      {
        _contextMenu = null;
      }

      if (_window != null && !_window.IsOpen)
      {
        _window = null;
      }

      _buildPanel.Update(gameTime);
      _window?.Update(gameTime);
      _placeOptionsPanel?.Update(gameTime);
      _contextMenu?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (BaseGame.GWM.State != GameStates.Playing)
      {
        return;
      }

      _buildPanel.Draw(spriteBatch);
      _window?.Draw(spriteBatch);
      _placeOptionsPanel?.Draw(spriteBatch);
      _contextMenu?.Draw(spriteBatch);
    }

    public void OpenContextMenu(ContextMenuModel model)
    {
      _contextMenu = new ContextMenu(model);
    }

    public void HandlePlaceClicked(Place place)
    {
      _placeOptionsPanel = new PlaceOptionsPanel(place, place.Position);
    }

    public void HandleCraftClicked(Place place)
    {
      _window = new CraftingWindow(
        place,
        new Vector2(100, 100),
        BaseGame.ScreenWidth - 200,
        BaseGame.ScreenHeight - 200,
        (item) =>
        {
          BaseGame.GWM.JobManager.Add(place, item);
        }
      );
    }

    public void HandleManageStorageClicked(Place place)
    {
      _window = new ManageStorageWindow(
        place,
        new Vector2(100, 100),
        BaseGame.ScreenWidth - 200,
        BaseGame.ScreenHeight - 200
      );
    }
  }
}
