using Microsoft.Xna.Framework;

namespace Village123.Shared.Maps
{
  public interface IMappableObject
  {
    bool IsSolid { get; set; }

    Rectangle Rectangle { get; set; }

    public int X
    {
      get
      {
        return Rectangle.X;
      }
    }

    public int Y
    {
      get
      {
        return Rectangle.Y;
      }
    }

    public int Width
    {
      get
      {
        return Rectangle.Width;
      }
    }

    public int Height
    {
      get
      {
        return Rectangle.Height;
      }
    }

    public int Right
    {
      get
      {
        return Rectangle.Right;
      }
    }

    public int Bottom
    {
      get
      {
        return Rectangle.Bottom;
      }
    }
  }
}
