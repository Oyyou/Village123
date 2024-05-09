using Microsoft.Xna.Framework;
using System.Linq;
using System.Reflection;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;

namespace Village123.Shared.Managers
{
  public class GameWorldManager
  {
    private IdData _idData = new();
    private VillagerData _villagerData = new();

    private GameWorld _gameWorld;

    private Map _map;

    private VillagerManager _villagerManager;

    private void CallPersistableMethod(string methodName, params object[] args)
    {
      var fields = typeof(GameWorldManager).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

      foreach (var field in fields)
      {
        if (field.FieldType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPersistable<>)))
        {
          var method = field.FieldType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
          if (method != null)
          {
            var loadedValue = method.Invoke(null, args.Length > 0 ? args : null);
            field.SetValue(this, loadedValue);
          }
        }
      }
    }

    public void Load()
    {
      // TODO: Load map
      _map = new Map(20, 20);

      _gameWorld = new GameWorld(_map);

      CallPersistableMethod("Load", _gameWorld);

      _villagerManager = new VillagerManager(_idData, _villagerData);
    }

    public void Save()
    {
      CallPersistableMethod("Save");
    }

    public void Update(GameTime gameTime)
    {
      _villagerManager.Update();
    }

    public void Draw(GameTime gameTime)
    {

    }
  }
}
