using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
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

    public Vector2 Position { get; set; }

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
    public Dictionary<string, short> Conditions { get; set; } = new();

    [JsonIgnore]
    public Villager Father { get; set; }

    [JsonIgnore]
    public Villager Mother { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private Queue<IVillagerAction> actionQueue { get; set; } = new();

    public void AddAction(IVillagerAction action)
    {
      actionQueue.Enqueue(action);
    }

    public void Update()
    {
      if (actionQueue.Count > 0)
      {
        var currentAction = actionQueue.Peek();
        if(!currentAction.Started)
        {
          currentAction.Start();
          currentAction.Started = true;
        }
        currentAction.Update();

        if (currentAction.IsComplete())
        {
          actionQueue.Dequeue();
        }
      }
    }
  }
}
