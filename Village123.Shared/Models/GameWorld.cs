using Village123.Shared.Maps;

namespace Village123.Shared.Models
{
  public class GameWorld
  {
    public Map Map { get; private set; }

    public GameWorld(Map map)
    {
      Map = map;
    }
  }
}
