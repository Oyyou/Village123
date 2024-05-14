using Microsoft.Xna.Framework;
using System.Linq;
using System.Reflection;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;

namespace Village123.Shared.Managers
{
  public class GameWorldManager
  {
    private IdData _idData = new();
    private VillagerData _villagerData = new();

    private Map _map;

    private VillagerManager _villagerManager;

    private void CallPersistableMethod(string methodName)
    {
      var fields = typeof(GameWorldManager).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

      foreach (var field in fields)
      {
        if (field.FieldType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPersistable<>)))
        {
          var method = field.FieldType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
          if (method != null)
          {
            var loadedValue = method.Invoke(null, null);
            field.SetValue(this, loadedValue);
          }
        }
      }
    }

    public void Load()
    {
      CallPersistableMethod("Load");

      _map = new Map(20, 20);
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
