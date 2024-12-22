﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Input;
using Village123.Shared.Services;
using Village123.Shared.Sprites;
using Village123.Shared.Utils;

namespace Village123.Shared.Managers
{
  public class PlaceManager
  {
    private const string fileName = "places.json";
    private SaveFileService _saveFileService;

    private MouseState _previousMouse;
    private MouseState _currentMouse;

    private Texture2D _greyBackground;
    private Sprite _buildingInside = null;

    public List<Place> Places { get; private set; } = new();

    private PlaceManager() { }

    public PlaceManager(SaveFileService saveFileService)
    {
      _saveFileService = saveFileService;
    }

    #region Serialization
    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static PlaceManager Load(SaveFileService saveFileService)
    {
      var manager = saveFileService.Load<PlaceManager>(fileName);

      if (manager == null)
      {
        manager = new PlaceManager(saveFileService);
      }
      else
      {
        manager._saveFileService = saveFileService;
      }

      foreach (var place in manager.Places)
      {
        place.SetData(BaseGame.GWM.PlaceData.Places[place.Key]);

        var texturePath = $"Places/{place.Data.Key}";
        if (place.Data.Type == "building")
        {
          texturePath = $"Places/{place.Data.Key}/{place.Data.Key}_front";
        }

        place.Texture = TextureHelpers.LoadTexture(texturePath);
      }

      return manager;
    }
    #endregion

    public void EnterBuilding(Place building)
    {
      BaseGame.GWM.State = GameStates.InBuilding;

      var texturePath = $"Places/{building.Data.Key}/{building.Data.Key}_inside";
      var texture = TextureHelpers.LoadTexture(texturePath);
      _buildingInside = new Sprite(
        texture,
        new Vector2(
          (BaseGame.ScreenWidth / 2) - (texture.Width / 2),
          (BaseGame.ScreenHeight / 2) - (texture.Height / 2)));

      _greyBackground = TextureHelpers.CreateBorderedTexture(BaseGame.GWM.GameModel.GraphicsDevice, BaseGame.ScreenWidth, BaseGame.ScreenHeight, Color.Black * 0.6f, Color.Black, 2);
    }

    public void LeaveBuilding()
    {
      BaseGame.GWM.State = GameStates.Playing;

      _buildingInside = null;
    }

    public void UpdateMouse()
    {
      if (BaseGame.GWM.State != GameStates.Playing)
      {
        return;
      }

      foreach (var place in Places)
      {
        place.ClickableComponent.Update();
      }
    }

    public void Update(GameTime gameTime)
    {
      if (BaseGame.GWM.State == GameStates.InBuilding)
      {
        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
          LeaveBuilding();
        }

        var mousePressed = _previousMouse.LeftButton == ButtonState.Pressed &&
          _currentMouse.LeftButton == ButtonState.Released;

        if (mousePressed && !GameMouse.Rectangle.Intersects(_buildingInside.Rectangle))
        {
          LeaveBuilding();
        }

        return;
      }

      foreach (var place in Places)
      {
        place.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var place in Places)
      {
        place.Draw(spriteBatch);
      }
    }

    public void DrawInside(SpriteBatch spriteBatch)
    {
      if (BaseGame.GWM.State != GameStates.InBuilding)
      {
        return;
      }

      spriteBatch.Draw(_greyBackground, new Vector2(0, 0), Color.White);
      _buildingInside.Draw(spriteBatch);
    }

    public Place Add(PlaceData.Place data, Point point)
    {
      var texturePath = $"Places/{data.Key}";

      if (data.Type == "building")
      {
        texturePath = $"Places/{data.Key}/{data.Key}_front";
      }

      var id = BaseGame.GWM.IdManager.PlaceId++;
      var place = new Place(data, TextureHelpers.LoadTexture(texturePath), point)
      {
        Id = id,
        Name = $"{data.Name} {id}",
      };

      Places.Add(place);
      if (data.Type == "building")
      {
        BaseGame.GWM.Map.AddObstacle(point + data.Offset, data.Size, Point.Zero);
      }
      else
      {
        BaseGame.GWM.Map.AddEntity(point + data.Offset, place.Data.Size);
      }

      return place;
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

    public IEnumerable<Place> GetPlacesByType(string type) => Places.Where(p => p.Data.Type == type);
  }
}
