using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Village123.Shared.Entities;
using Village123.Shared.Models;
using Village123.Shared.Utils;

namespace Village123.Shared.Managers
{
  public class ItemManager
  {
    private const string fileName = "items.json";

    public List<Item> Items { get; private set; } = new();

    private ItemManager() { }

    #region Serialization
    public void Save()
    {
      var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(fileName, jsonString);
    }

    public static ItemManager Load()
    {
      var manager = new ItemManager();

      if (File.Exists(fileName))
      {
        var jsonString = File.ReadAllText(fileName);
        manager = JsonConvert.DeserializeObject<ItemManager>(jsonString)!;
      }

      foreach (var item in manager.Items)
      {
        item.SetData(BaseGame.GWM.ItemData.Items[item.Name]);
        item.Texture = TextureHelpers.LoadTexture($"Items/{item.Name}");
      }

      return manager;
    }
    #endregion

    public void AddCraftedItem(ProducedItemModel item, Point craftedPoint)
    {
      var name = item.ItemName;

      var data = BaseGame.GWM.ItemData.Items[name];
      var texture = TextureHelpers.LoadTexture($"Items/{name}");

      var spawnPoint = FindNearestAvailableSpot(craftedPoint);

      if (spawnPoint != null)
      {
        Items.Add(new Item(BaseGame.GWM.IdManager.ItemId++, data, texture, spawnPoint.Value));
        BaseGame.GWM.Map.AddEntity(spawnPoint.Value, new Point(1, 1));
      }
      else
      {
        Console.WriteLine("No available spot found to place the crafted item.");
      }
    }

    private Point? FindNearestAvailableSpot(Point craftedPoint)
    {
      var directions = new Point[]
      {
        new Point(0, 1),  // Down
        new Point(1, 0),  // Right
        new Point(0, -1), // Up
        new Point(-1, 0), // Left
        new Point(1, 1),  // Down-Right
        new Point(1, -1), // Up-Right
        new Point(-1, 1), // Down-Left
        new Point(-1, -1) // Up-Left
      };

      var queue = new Queue<Point>();
      var visited = new HashSet<Point>();

      queue.Enqueue(craftedPoint);
      visited.Add(craftedPoint);

      while (queue.Count > 0)
      {
        var current = queue.Dequeue();

        if (BaseGame.GWM.Map.CanAddPlace(current))
        {
          return current;
        }

        foreach (var direction in directions)
        {
          var next = new Point(current.X + direction.X, current.Y + direction.Y);

          if (!visited.Contains(next) && BaseGame.GWM.Map.FitsOnMap(next, new Point(1, 1)))
          {
            queue.Enqueue(next);
            visited.Add(next);
          }
        }
      }

      return null; // No available spot found
    }

    public void Update(GameTime gameTime)
    {
      foreach (var item in Items)
      {
        item.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var item in Items)
      {
        item.Draw(spriteBatch);
      }
    }
  }
}
