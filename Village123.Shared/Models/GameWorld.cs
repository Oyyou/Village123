using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Village123.Shared.Entities;
using Village123.Shared.Maps;

namespace Village123.Shared.Models
{
  public class GameWorld
  {
    public ContentManager Content { get; private set; }

    public Map Map { get; private set; }

    public List<Place> Places { get; private set; } = new();

    public GameWorld(
      ContentManager content,
      Map map)
    {
      Content = content;
      Map = map;
    }

    public void SetPlaces(List<Place> places)
    {
      this.Places = places;
    }
  }
}
