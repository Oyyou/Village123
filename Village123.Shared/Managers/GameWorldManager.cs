using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

    public void Load(ContentManager content)
    {
      // TODO: Load map
      _map = new Map(20, 20);

      _gameWorld = new GameWorld(
        content,
        _map);

      _idData = IdData.Load(_gameWorld);
      _villagerData = VillagerData.Load(_gameWorld);

      _villagerManager = new VillagerManager(_gameWorld, _idData, _villagerData);
      var v1 = _villagerManager.CreateRandomVillager();
      v1.AddAction(new WalkAction(v1, _gameWorld, new Point(2, 2), true));
      Save();
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

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      _villagerManager.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
