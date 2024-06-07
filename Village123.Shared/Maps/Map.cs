using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Village123.Shared.Maps
{
  public class Map
  {
    private Dictionary<Point, KeyValuePair<float, Func<bool>>> _waitingPoints = new Dictionary<Point, KeyValuePair<float, Func<bool>>>();

    public float[,] Data { get; private set; }
    public int[,] EntityData { get; private set; }

    public int Width => Data.GetLength(1);

    public int Height => Data.GetLength(0);

    public Map(float[,] data)
    {
      Data = data;
    }

    public Map(int width, int height)
    {
      Data = new float[height, width];
      EntityData = new int[height, width];

      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          Data[y, x] = 0;
          EntityData[y, x] = 0;
        }
      }
    }

    //public void Add(MappedComponent obj, Func<bool> triggerActive = null)
    //{
    //  if (Collides(obj))
    //  {
    //    return;
    //  }

    //  // If we've got a condition that triggers the object to appear on the map,
    //  //   we add it to a dictionary then check it each frame
    //  if (triggerActive != null)
    //  {
    //    _waitingPoints.Add(obj.Point, new KeyValuePair<char, Func<bool>>(obj.MapChar, triggerActive));
    //    return;
    //  }

    //  for (int y = obj.Y; y < obj.Bottom; y++)
    //  {
    //    for (int x = obj.X; x < obj.Right; x++)
    //    {
    //      Data[y, x] = obj.MapChar;
    //    }
    //  }
    //}

    public void AddObstacle(Point position, Point size)
    {
      var bottom = position.Y + size.Y;
      var right = position.X + size.X;

      for (int y = position.Y; y < bottom; y++)
      {
        for (int x = position.X; x < right; x++)
        {
          Data[y, x] = 1;
        }
      }
    }

    public void AddEntity(Point position, Point size)
    {
      var bottom = position.Y + size.Y;
      var right = position.X + size.X;

      for (int y = position.Y; y < bottom; y++)
      {
        for (int x = position.X; x < right; x++)
        {
          EntityData[y, x] = 1;
        }
      }
    }

    public void RemoveEntity(Point position, Point size)
    {
      var bottom = position.Y + size.Y;
      var right = position.X + size.X;

      for (int y = position.Y; y < bottom; y++)
      {
        for (int x = position.X; x < right; x++)
        {
          EntityData[y, x] = 0;
        }
      }
    }

    public void Update()
    {
      for (int i = 0; i < _waitingPoints.Count; i++)
      {
        var current = _waitingPoints.ElementAt(i);
        if (current.Value.Value())
        {
          var x = current.Key.X;
          var y = current.Key.Y;
          var c = current.Value.Key;

          Data[y, x] = c;
          _waitingPoints.Remove(current.Key);
          i--;
        }
      }
    }

    //public void Remove(MappedComponent obj)
    //{
    //  for (int y = obj.Y; y < obj.Bottom; y++)
    //  {
    //    for (int x = obj.X; x < obj.Right; x++)
    //    {
    //      Data[y, x] = '0';
    //    }
    //  }
    //}

    public IEnumerable<Point> GetEmptyPoints()
    {
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          if (Data[y, x] < 1)
            yield return new Point(x, y);
        }
      }
    }

    //public bool Collides(MappedComponent obj)
    //{
    //  return Collides(new Point(obj.X, obj.Y), new Point(obj.Width, obj.Height));
    //}

    public bool Collides(Point position, Point size)
    {
      if (!FitsOnMap(position, size))
        return true;

      var bottom = position.Y + size.Y;
      var right = position.X + size.X;

      for (int y = position.Y; y < bottom; y++)
      {
        for (int x = position.X; x < right; x++)
        {
          if (Data[y, x] == 1)
            return true;
        }
      }
      return false;
    }

    //public bool FitsOnMap(MappedComponent obj)
    //{
    //  return FitsOnMap(new Point(obj.X, obj.Y), new Point(obj.Width, obj.Height));
    //}

    public bool FitsOnMap(Point position, Point size)
    {
      var bottom = position.Y + size.Y;
      var right = position.X + size.X;

      if (position.X < 0)
        return false;

      if (position.Y < 0)
        return false;

      if (bottom > Height)
        return false;

      if (right > Width)
        return false;

      return true;
    }

    public void WriteMap<T>(T[,] map)
    {
      Console.Clear();

      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          Console.Write(map[y, x]);
        }
        Console.WriteLine();
      }
    }

    public bool IsPassable(int x, int y)
    {
      return Data[y, x] < 1;
    }

    public bool CanAddPlace(Point point)
    {
      if (!FitsOnMap(point, new Point(1, 1)))
      {
        return false;
      }
      return EntityData[point.Y, point.X] < 1 && Data[point.Y, point.X] < 1;

    }

    public bool IsPassable(Point point)
    {
      return IsPassable(point.X, point.Y);
    }
  }
}
