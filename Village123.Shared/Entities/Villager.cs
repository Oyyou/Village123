using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Village123.Shared.Logging;
using Village123.Shared.Models;
using Village123.Shared.Models.AdditionalInfo;
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

    public Color Colour { get; set; }

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

    public List<int> JobIds { get; set; } = new();

    [JsonIgnore]
    public Villager Father { get; set; }

    [JsonIgnore]
    public Villager Mother { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    public Queue<IVillagerAction> ActionQueue { get; set; } = new();

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    public Dictionary<string, BaseAdditionalInfo> AdditionalInfo { get; set; } = new();

    public Villager(Texture2D texture)
    {
      Texture = texture;
    }

    public void AddAction(IVillagerAction action)
    {
      ActionQueue.Enqueue(action);
    }

    #region AdditionalData
    /// <summary>
    /// Retrieves the additional info of the specified type, or creates and adds it if not found.
    /// </summary>
    public T GetAdditionalInfo<T>(string key, Func<T> createFunc) where T : BaseAdditionalInfo
    {
      if (AdditionalInfo.TryGetValue(key, out var info) && info is T typedInfo)
      {
        return typedInfo;
      }

      var newInfo = createFunc();
      AdditionalInfo[key] = newInfo;
      return newInfo;
    }

    public CooldownAdditionalInfo GetCooldown(string key)
    {
      return GetAdditionalInfo(key, () => new CooldownAdditionalInfo());
    }
    public void SetCooldown(string key, float duration)
    {
      var cooldownInfo = GetAdditionalInfo(key, () => new CooldownAdditionalInfo());
      cooldownInfo.Duration = duration;
    }
    #endregion

    public void Update(GameTime gameTime)
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
        currentAction.Update(gameTime);

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

      var keysToRemove = new List<string>();
      foreach (var kvp in AdditionalInfo)
      {
        if (kvp.Value is CooldownAdditionalInfo cooldown)
        {
          cooldown.Duration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
          if (cooldown.Duration <= 0)
          {
            keysToRemove.Add(kvp.Key);
          }
        }
      }

      foreach (var key in keysToRemove)
      {
        AdditionalInfo.Remove(key);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, Colour);
    }
  }
}
