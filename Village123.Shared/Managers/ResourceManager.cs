using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Village123.Shared.Entities;
using Village123.Shared.Services;
using Village123.Shared.Utils;

namespace Village123.Shared.Managers
{
  public class ResourceManager
  {
    private const string fileName = "resources.json";
    private SaveFileService _saveFileService;

    public List<Resource> Resources { get; private set; } = new();

    private ResourceManager() { }

    public ResourceManager(SaveFileService saveFileService)
    {
      _saveFileService = saveFileService;
    }

    #region Serialization
    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static ResourceManager Load(SaveFileService saveFileService)
    {
      var manager = saveFileService.Load<ResourceManager>(fileName);

      if (manager == null)
      {
        manager = new ResourceManager(saveFileService);
      }
      else
      {
        manager._saveFileService = saveFileService;
      }

      foreach (var resource in manager.Resources)
      {
        resource.SetData(BaseGame.GWM.ResourceData.Resources[resource.Name]);
        resource.Texture = TextureHelpers.LoadTexture($"Resources/{resource.Name}");

        // BaseGame.GWM.Map.AddObstacle(resource.Point, resource.Data.Size, resource.Data.PointOffset);
      }

      return manager;
    }
    #endregion

    public void UpdateMouse()
    {
      foreach (var resource in Resources)
      {
        resource.ClickableComponent.Update();
      }
    }

    public void Update(GameTime gameTime)
    {
      foreach (var resource in Resources)
      {
        resource.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var resource in Resources)
      {
        resource.Draw(spriteBatch);
      }
    }

    public Resource Add(string resourceName, Point point)
    {
      var data = BaseGame.GWM.ResourceData.Resources[resourceName];
      var id = BaseGame.GWM.IdManager.ResourceId++;
      var resource = new Resource(id, data, TextureHelpers.LoadTexture($"Resources/{resourceName}"), point)
      {
        Name = resourceName,
      };

      Resources.Add(resource);

      BaseGame.GWM.Map.AddObstacle(point, resource.Data.Size, resource.Data.PointOffset);

      return resource;
    }

    public void Destroy(Place place)
    {
      place.StartDestruction();
      // TODO: Add destruction job
    }

    public void CancelDestruction(Place place)
    {
      place.CancelDestruction();
      // TODO: Remove destruction job
    }

    public void RemoveById(int resourceId)
    {
      var resource = Resources.Find(r => r.Id == resourceId);
      if (resource == null)
      {
        return;
      }

      Resources.Remove(resource);
      BaseGame.GWM.Map.RemoveData(resource.Point, resource.Data.Size);
    }
  }
}
