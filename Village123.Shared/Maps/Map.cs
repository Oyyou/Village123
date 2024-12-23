using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Village123.Shared.Services;
using Village123.Shared.Utils;

namespace Village123.Shared.Maps
{
  public class Map
  {
    private SaveFileService _saveFileService;
    private Dictionary<Point, KeyValuePair<float, Func<bool>>> _waitingPoints = new Dictionary<Point, KeyValuePair<float, Func<bool>>>();

    public float[,] Data { get; set; }
    public int[,] EntityData { get; set; }
    public bool ShowGrid { get; set; } = false;

    public int Width => Data.GetLength(1);

    public int Height => Data.GetLength(0);

    private Texture2D _gridTexture;

    public Map()
    {
      _gridTexture = TextureHelpers.CreateBorderedTexture(BaseGame.GWM.GameModel.GraphicsDevice, BaseGame.TileSize, BaseGame.TileSize, Color.White * 0, Color.Black, 1);
    }

    public Map(int width, int height, SaveFileService saveFileService)
      : this()
    {
      _saveFileService = saveFileService;

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

    public Map(float[,] data)
      : this()
    {
      Data = data;
    }

    public void Save()
    {
      _saveFileService.Save(this, "map.json");
    }

    public static Map Load(SaveFileService saveFileService)
    {
      var map = saveFileService.Load<Map>("map.json");

      if (map == null)
        return new Map(15, 15, saveFileService);

      map._saveFileService = saveFileService;
      return map;
    }

    public Point? FindNearestValidPoint(Point position, int maxDistance)
    {
      for (int distance = maxDistance; distance >= 0; distance--)
      {
        var validPoints = new List<Point>();

        // Loop through points in a square around the position with the given distance
        for (int dy = -distance; dy <= distance; dy++)
        {
          for (int dx = -distance; dx <= distance; dx++)
          {
            // Skip points that are not exactly `distance` away
            if (Math.Abs(dx) + Math.Abs(dy) != distance)
              continue;

            var candidate = new Point(position.X + dx, position.Y + dy);

            // Check if the point is valid
            if (FitsOnMap(candidate, new Point(1, 1)) && IsPassable(candidate))
            {
              validPoints.Add(candidate);
            }
          }
        }

        // If we have valid points at this distance, choose a random one
        if (validPoints.Count > 0)
        {
          return validPoints[BaseGame.Random.Next(validPoints.Count)];
        }
      }

      // Return null if no valid point is found
      return null;
    }

    public void AddObstacle(Point position, Point size, Point pointOffset)
    {
      var bottom = position.Y + size.Y;
      var right = position.X + size.X;

      for (int y = position.Y + pointOffset.Y; y < bottom; y++)
      {
        for (int x = position.X + pointOffset.X; x < right; x++)
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

    public void RemoveData(Point position, Point size)
    {
      var bottom = position.Y + size.Y;
      var right = position.X + size.X;

      for (int y = position.Y; y < bottom; y++)
      {
        for (int x = position.X; x < right; x++)
        {
          Data[y, x] = 0;
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

    public void Draw(SpriteBatch spriteBatch)
    {
      if (Keyboard.GetState().IsKeyDown(Keys.G))
      {
        ShowGrid = !ShowGrid;
      }

      if (!ShowGrid)
        return;

      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          spriteBatch.Draw(_gridTexture, new Vector2(x * _gridTexture.Width, y * _gridTexture.Height), Color.White);
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

      //WriteMapToFile(this.Data, "Test1.txt");
      //WriteMapToFile(this.EntityData, "Test2.txt");

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

    private void WriteMapToFile<T>(T[,] map, string filename)
    {
      using (var writer = new StreamWriter(filename))
      {
        for (int y = 0; y < map.GetLength(1); y++)
        {
          for (int x = 0; x < map.GetLength(0); x++)
          {
            writer.Write(map[y, x]);
          }
          writer.WriteLine();
        }
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

    public bool CanAddPlace(Point point, Point size)
    {
      if (!FitsOnMap(point, size))
      {
        return false;
      }

      for (int y = 0; y < size.Y; y++)
      {
        for (int x = 0; x < size.X; x++)
        {
          int mapX = point.X + x;
          int mapY = point.Y + y;

          if (EntityData[mapY, mapX] >= 1 || Data[mapY, mapX] >= 1)
          {
            return false;
          }
        }
      }

      return true;
    }

    public bool IsPassable(Point point)
    {
      return IsPassable(point.X, point.Y);
    }
  }
}
