﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;

namespace Village123.Shared.Managers
{
  public class MaterialManager
  {
    private const string fileName = "materials.json";

    public List<Material> Materials { get; private set; } = new();

    public MaterialManager()
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

    public static MaterialManager Load()
    {
      var manager = new MaterialManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        manager = JsonConvert.DeserializeObject<MaterialManager>(jsonString)!;
      }

      foreach (var material in manager.Materials)
      {
        material.SetData(BaseGame.GWM.MaterialsData.Materials[material.Name]);
        material.Texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>($"Materials/{material.Name}");
      }

      return manager;
    }
    #endregion

    public void Add(string resourceName, Point emitterPoint)
    {
      var data = BaseGame.GWM.MaterialsData.Materials[resourceName];
      var texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>($"Materials/{resourceName}");

      Materials.Add(new Material(
        BaseGame.GWM.IdManager.ResourceId++, data, texture, emitterPoint
      ));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var material in Materials)
      {
        material.Draw(spriteBatch);
      }
    }
  }
}