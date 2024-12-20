using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using Village123.Shared.Events;
using Village123.Shared.Services;

namespace Village123.Shared.Managers
{
  public class EventManager
  {
    private const string fileName = "events.json";
    private SaveFileService _saveFileService;

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    public List<BaseEvent> Events { get; private set; } = new();

    public EventManager()
    {

    }

    public EventManager(SaveFileService saveFileService)
    {
      _saveFileService = saveFileService;
    }

    #region Serialization
    public void Save()
    {
      _saveFileService.Save(this, fileName);
    }

    public static EventManager Load(SaveFileService saveFileService)
    {
      var manager = saveFileService.Load<EventManager>(fileName);

      if (manager == null)
        return new EventManager(saveFileService);

      manager._saveFileService = saveFileService;
      return manager;
    }
    #endregion

    public void Update(GameTime gameTime)
    {
      for (int i = 0; i < Events.Count; i++)
      {
        Events[i].Update(gameTime);

        if (Events[i].IsComplete)
        {
          Events.RemoveAt(i);
          i--;
        }
      }
    }

    public void Add(BaseEvent newEvent)
    {
      Events.Add(newEvent);
    }
  }
}
