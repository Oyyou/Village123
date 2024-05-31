using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Input;

namespace Village123.Shared.Managers
{
  public class BuildManager
  {
    private GameWorldManager _gwm;

    private Place _place;

    public BuildManager(GameWorldManager gwm)
    {
      _gwm = gwm;
    }

    public void Build(PlaceData.Place place)
    {
      // Can't build while building
      if (_gwm.State == GameStates.Building)
        return;

      _gwm.State = GameStates.Building;

      var texture = _gwm.GameModel.Content.Load<Texture2D>($"Places/{place.Key}");

      _place = new Place(place, texture, Point.Zero)
      {
        Opacity = 0.75f,
      };
    }

    public void Update(GameTime gameTime)
    {
      if (_gwm.State != GameStates.Building)
        return;

      if (GameMouse.IsRightClicked || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        _gwm.State = GameStates.Playing;
        _place = null;
        return;
      }

      _place.Colour = Color.Green;
      _place.Point = GameMouse.MapPoint;

      if (!_gwm.Map.CanAddPlace(_place.Point))
      {
        _place.Colour = Color.Red;
        return;
      }

      if (GameMouse.IsLeftClicked)
      {
        PlaceManager.GetInstance(_gwm).Add(_place.Data, _place.Point);
        _gwm.State = GameStates.Playing;
        _place = null;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (_gwm.State != GameStates.Building)
        return;

      spriteBatch.Begin();
      _place.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
