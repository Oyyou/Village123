using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;
using static Village123.Shared.Data.ItemData;

namespace Village123.Shared.Managers
{
  public class ResourceManager
  {
    private const string fileName = "resources.json";

    public List<Resource> Resources { get; private set; } = new();

    public ResourceManager()
    {

    }

    #region Serialization
    public void Save()
    {
      var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(fileName, jsonString);
    }

    public static ResourceManager Load()
    {
      var manager = new ResourceManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        manager = JsonConvert.DeserializeObject<ResourceManager>(jsonString)!;
      }

      foreach (var resource in manager.Resources)
      {
        resource.SetData(BaseGame.GWM.ResourceData.Resources[resource.Name]);
        resource.Texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>($"Resources/{resource.Name}");
      }

      return manager;
    }
    #endregion

    public void AddResource(string resourceName, Point emitterPoint)
    {
      var data = BaseGame.GWM.ResourceData.Resources[resourceName];
      var texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>($"Resources/{resourceName}");

      Resources.Add(new Resource(
        BaseGame.GWM.IdManager.ResourceId++, data, texture, emitterPoint
      ));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var resource in Resources)
      {
        resource.Draw(spriteBatch);
      }
    }
  }
}
