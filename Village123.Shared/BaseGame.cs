using System;

namespace Village123.Shared
{
  public class BaseGame
  {
    public const short TileSize = 32;

    public static Random Random = new();
    public static int ScreenWidth = 1280;
    public static int ScreenHeight = 800;
  }
}
