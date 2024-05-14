using System;
using System.Collections.Generic;

namespace Village123.Shared.Entities
{
  public class Map
  {
    private float[,] data;

    public Map(int width, int height)
    {
      data = new float[height, width];

      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          data[y, x] = 0.5f;
        }
      }
    }

    public List<Point> FindPath(Microsoft.Xna.Framework.Point startPoint, Microsoft.Xna.Framework.Point endPoint)
    {
      var start = new Point(startPoint);
      var end = new Point(endPoint);

      // Initialize the open and closed lists
      var openList = new List<Point>();
      var closedList = new HashSet<Point>();

      // Add the starting point to the open list
      openList.Add(start);

      while (openList.Count > 0)
      {
        // Get the point with the lowest f cost from the open list
        var current = openList[0];
        for (int i = 1; i < openList.Count; i++)
        {
          if (openList[i].FCost < current.FCost || (openList[i].FCost == current.FCost && openList[i].HCost < current.HCost))
          {
            current = openList[i];
          }
        }

        // Remove the current point from the open list and add it to the closed list
        openList.Remove(current);
        closedList.Add(current);

        // If we have reached the end point, reconstruct and return the path
        if (current == end)
        {
          return RetracePath(start, end);
        }

        // Get the neighbors of the current point
        var neighbors = GetNeighbors(current);

        foreach (var neighbor in neighbors)
        {
          // Skip if the neighbor is in the closed list or is not walkable
          if (closedList.Contains(neighbor) || !IsWalkable(neighbor))
          {
            continue;
          }

          // Calculate the new g cost to this neighbor
          float newCostToNeighbor = current.GCost + GetDistance(current, neighbor);

          // If this new path is shorter or the neighbor is not in the open list, update the neighbor
          if (newCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
          {
            neighbor.GCost = newCostToNeighbor;
            neighbor.HCost = GetDistance(neighbor, end);
            neighbor.Parent = current;

            // If the neighbor is not in the open list, add it
            if (!openList.Contains(neighbor))
            {
              openList.Add(neighbor);
            }
          }
        }
      }

      // No path found
      return null;
    }

    private List<Point> RetracePath(Point start, Point end)
    {
      var path = new List<Point>();
      Point current = end;

      while (current != start)
      {
        path.Add(current);
        current = current.Parent;
      }

      path.Reverse();
      return path;
    }

    private List<Point> GetNeighbors(Point point)
    {
      var neighbors = new List<Point>();

      // Add adjacent points (up, down, left, right)
      neighbors.Add(new Point(point.X, point.Y + 1)); // Up
      neighbors.Add(new Point(point.X, point.Y - 1)); // Down
      neighbors.Add(new Point(point.X - 1, point.Y)); // Left
      neighbors.Add(new Point(point.X + 1, point.Y)); // Right

      return neighbors;
    }

    private bool IsWalkable(Point point)
    {
      // Check if the point is within the map boundaries and if it's walkable based on its value
      return point.X >= 0 && point.X < data.GetLength(1) &&
             point.Y >= 0 && point.Y < data.GetLength(0) &&
             data[point.Y, point.X] != 1f; // Adjusted to consider values other than 0.5 as walkable
    }

    private float GetDistance(Point from, Point to)
    {
      // Use Manhattan distance as a heuristic (you can use other distance measures as well)
      return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    }

    public class Point
    {
      public int X { get; }
      public int Y { get; }
      public float GCost { get; set; } // Cost from the start point
      public float HCost { get; set; } // Cost to the end point (heuristic)
      public float FCost => GCost + HCost; // Total cost (GCost + HCost)
      public Point Parent { get; set; } // Parent point in the path

      public Point(int x, int y)
      {
        X = x;
        Y = y;
        GCost = 0;
        HCost = 0;
        Parent = null;
      }

      public Point(Microsoft.Xna.Framework.Point point)
        : this(point.X, point.Y)
      {

      }
    }
  }
}
