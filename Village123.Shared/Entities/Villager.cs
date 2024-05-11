using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Village123.Shared.Logging;
using Village123.Shared.Models;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Entities
{
  public enum Genders
  {
    Male,
    Female,
  }

  public class Villager : IEntity
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Genders Gender { get; set; }
    public int? FatherId { get; set; }

    public int? MotherId { get; set; }

    public Point Point { get; set; }

    [JsonIgnore]
    public Vector2 Position => (Point.ToVector2() * BaseGame.TileSize) + PositionOffset;

    [JsonIgnore]
    public Vector2 PositionOffset { get; set; } = Vector2.Zero;

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    /// <summary>
    /// Lumbering, mining, building etc
    /// </summary>
    public Dictionary<string, short> Skills { get; set; } = new();

    /// <summary>
    /// Strength, endurance, dexterity etc
    /// </summary>
    public Dictionary<string, short> Stats { get; set; } = new();

    /// <summary>
    /// Hunger, tiredness, boredom etc
    /// </summary>
    public Dictionary<string, Condition> Conditions { get; set; } = new();

    [JsonIgnore]
    public Villager Father { get; set; }

    [JsonIgnore]
    public Villager Mother { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    public Queue<IVillagerAction> ActionQueue { get; set; } = new();

    public Villager(Texture2D texture)
    {
      Texture = texture;
    }

    public void AddAction(IVillagerAction action)
    {
      ActionQueue.Enqueue(action);
    }

    public void Update()
    {
      if (ActionQueue.Count > 0)
      {
        var currentAction = ActionQueue.Peek();
        if (!currentAction.Started)
        {
          currentAction.Start();
          Logger.WriteLine($"Villager '{FirstName} {LastName}' started {currentAction.Name}");
          currentAction.Started = true;
        }
        currentAction.Update();

        if (currentAction.IsComplete())
        {
          Logger.WriteLine($"Villager '{FirstName} {LastName}' finished {currentAction.Name}");
          currentAction.OnComplete();
          ActionQueue.Dequeue();
        }
      }

      foreach (var condition in Conditions)
      {
        condition.Value.Value += condition.Value.Rate;
        condition.Value.Value = MathHelper.Clamp(condition.Value.Value, 0, 100);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, Color.White);
    }
  }
}
