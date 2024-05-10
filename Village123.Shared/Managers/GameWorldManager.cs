using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Reflection;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Maps;
using Village123.Shared.Models;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Managers
{
  public class GameWorldManager
  {
    private IdData _idData = new();
    private VillagerData _villagerData = new();

    private GameWorld _gameWorld;

    private Map _map;

    private VillagerManager _villagerManager;

    public void Load()
    {
      // TODO: Load map
      _map = new Map(20, 20);

      _gameWorld = new GameWorld(_map);

      _idData = IdData.Load(_gameWorld);
      _villagerData = VillagerData.Load(_gameWorld);

      _villagerManager = new VillagerManager(_gameWorld, _idData, _villagerData);
    }

    public void Save()
    {
      _idData.Save();
      _villagerData.Save();
    }

    public void Update(GameTime gameTime)
    {
      if (Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        this.Save();
      }

      _villagerManager.Update();
    }

    public void Draw(GameTime gameTime)
    {

    }
  }
}
