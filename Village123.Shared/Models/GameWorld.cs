using Microsoft.Xna.Framework.Content;
using Village123.Shared.Maps;

namespace Village123.Shared.Models
{
  public class GameWorld
  {
    public ContentManager Content { get; private set; }

    public Map Map { get; private set; }

    public GameWorld(
      ContentManager content,
      Map map)
    {
      Content = content;
      Map = map;
    }
  }
}
