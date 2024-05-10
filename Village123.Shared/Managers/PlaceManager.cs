using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.Data;
using Village123.Shared.Entities;
using Village123.Shared.Models;

namespace Village123.Shared.Managers
{
  public class PlaceManager
  {
    private readonly GameWorld _gameWorld;
    private readonly IdData _idData;
    private readonly PlaceData _placeData;

    public PlaceManager(
      GameWorld gameWorld,
      IdData idData,
      PlaceData placeData
      )
    {
      _gameWorld = gameWorld;
      _idData = idData;
      _placeData = placeData;
    }

    public void Update()
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var place in _placeData.Places)
      {
        place.Draw(spriteBatch);
      }
    }

    public Place Add(string name, Point point)
    {
      var place = new Place(_gameWorld.Content.Load<Texture2D>($"Places/{name}"), point)
      {
        Id = _idData.PlaceId++,
      };

      _placeData.Add(place);

      return place;
    }
  }
}
