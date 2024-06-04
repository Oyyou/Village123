using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Input;
using Village123.Shared.Utils;

namespace Village123.Shared.Managers
{
  public class BuildManager
  {
    private Place _place;

    private readonly Texture2D _radiusTexture;
    private List<Point> _radiusPoints = new();

    public BuildManager()
    {
      _radiusTexture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        BaseGame.TileSize,
        BaseGame.TileSize,
        Color.White,
        Color.White,
        1
      );
    }

    public void Build(PlaceData.Place place)
    {
      // Can't build while building
      if (BaseGame.GWM.State == GameStates.Building)
        return;

      BaseGame.GWM.State = GameStates.Building;

      var texture = BaseGame.GWM.GameModel.Content.Load<Texture2D>($"Places/{place.Key}");

      _place = new Place(place, texture, Point.Zero)
      {
        Opacity = 0.75f,
      };
    }

    public void Update(GameTime gameTime)
    {
      if (BaseGame.GWM.State != GameStates.Building)
        return;

      if (GameMouse.IsRightClicked || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        BaseGame.GWM.State = GameStates.Playing;
        _place = null;
        return;
      }

      _place.Colour = Color.Green;
      _place.Point = GameMouse.MapPoint;

      CalculateRadiusPoints(_place.Point, _place.Data.Radius);

      if (!BaseGame.GWM.Map.CanAddPlace(_place.Point))
      {
        _place.Colour = Color.Red;
        return;
      }

      if (GameMouse.IsLeftClicked)
      {
        var place = BaseGame.GWM.PlaceManager.Add(_place.Data, _place.Point);
        place.RadiusPoints = _radiusPoints.ToList();
        BaseGame.GWM.State = GameStates.Playing;
        _place = null;
      }
    }

    private void CalculateRadiusPoints(Point center, int radius)
    {
      _radiusPoints.Clear();
      for (int y = -radius; y <= radius; y++)
      {
        for (int x = -radius; x <= radius; x++)
        {
          if (x == 0 && y == 0)
            continue;

          if (x * x + y * y <= radius * radius)
          {
            _radiusPoints.Add(new Point(center.X + x, center.Y + y));
          }
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (BaseGame.GWM.State != GameStates.Building)
        return;

      spriteBatch.Begin();
      _place.Draw(spriteBatch);
      foreach (var point in _radiusPoints)
      {
        spriteBatch.Draw(_radiusTexture, point.ToVector2() * BaseGame.TileSize, Color.White * 0.75f);
      }
      spriteBatch.End();
    }
  }
}
