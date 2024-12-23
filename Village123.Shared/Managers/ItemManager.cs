﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Village123.Shared.Entities;
using Village123.Shared.Models;
using Village123.Shared.Services;
using Village123.Shared.Utils;

namespace Village123.Shared.Managers
{
  public class ItemManager
  {
    private const string fileName = "items.json";
    private SaveFileService _saveFileService;

    public List<Item> Items { get; private set; } = new();

    private ItemManager() { }

    public ItemManager(SaveFileService saveFileService)
    {
      _saveFileService = saveFileService;
    }

    #region Serialization
    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static ItemManager Load(SaveFileService saveFileService)
    {
      var manager = new ItemManager();

      if (manager == null)
      {
        manager = new ItemManager(saveFileService);
      }
      else
      {
        manager._saveFileService = saveFileService;
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

    public void UpdateMouse()
    {
      foreach (var item in Items)
      {
        // item.ClickableComponent.Update();
      }
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
