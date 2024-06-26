﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;
using Village123.Shared.Utils;

namespace Village123.Shared.Managers
{
  public class ResourceManager
  {
    private const string fileName = "resoruces.json";
    public List<Resource> Resources { get; private set; } = new();

    private ResourceManager() { }

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

      foreach (var value in manager.Resources)
      {
        value.SetData(BaseGame.GWM.ResourceData.Resources[value.Name]);
        value.Texture = TextureHelpers.LoadTexture($"Resources/{value.Name}");
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
