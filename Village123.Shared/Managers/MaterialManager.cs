using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Village123.Shared.Entities;
using Village123.Shared.Services;
using Village123.Shared.Utils;

namespace Village123.Shared.Managers
{
  public class MaterialManager
  {
    private const string fileName = "materials.json";
    private SaveFileService _saveFileService;

    public List<Material> Materials { get; private set; } = new();

    public MaterialManager()
    {

    }

    public MaterialManager(SaveFileService saveFileService)
    {
      _saveFileService = saveFileService;
    }

    #region Serialization
    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static MaterialManager Load(SaveFileService saveFileService)
    {
      var manager = saveFileService.Load<MaterialManager>(fileName);

      if (manager == null)
      {
        manager = new MaterialManager(saveFileService);
      }
      else
      {
        manager._saveFileService = saveFileService;
      }

      foreach (var material in manager.Materials)
      {
        material.SetData(BaseGame.GWM.MaterialsData.Materials[material.Name]);
        material.Texture = TextureHelpers.LoadTexture($"Materials/{material.Name}");
      }

      return manager;
    }
    #endregion

    public void Add(string resourceName, Point emitterPoint)
    {
      var data = BaseGame.GWM.MaterialsData.Materials[resourceName];
      var texture = TextureHelpers.LoadTexture($"Materials/{resourceName}");

      Materials.Add(new Material(
        BaseGame.GWM.IdManager.MaterialId++, data, texture, emitterPoint
      ));
    }

    public bool IsMaterialAvailable(string materialName, int qty)
    {
      var total = 0;
      foreach (var material in Materials)
      {
        if (material.Data.Key == materialName)
          total++;

        if (total >= qty)
          return true;
      }
      return false;
    }

    public void UpdateMouse()
    {
      foreach (var material in Materials)
      {
        // material.ClickableComponent.Update();
      }
    }

    public void Update(GameTime gameTime)
    {
      foreach (var material in Materials)
      {
        material.Update(gameTime);
      }
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
