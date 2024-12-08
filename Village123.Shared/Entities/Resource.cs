using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Village123.Shared.Components;
using Village123.Shared.Data;
using Village123.Shared.Models;

namespace Village123.Shared.Entities
{
  public class Resource : IEntity
  {
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    private Dictionary<string, int> _cancallableActions = new();

    public int Id { get; set; }
    public string Name { get; set; }
    public Point Point { get; set; }

    [JsonIgnore]
    public Vector2 Position => Point.ToVector2() * BaseGame.TileSize;

    [JsonIgnore]
    public Texture2D Texture { get; set; }

    [JsonIgnore]
    public ResourceData.Resource Data { get; private set; }

    [JsonIgnore]
    public ClickableComponent ClickableComponent { get; protected set; }

    public Resource() { }

    public Resource(int id, ResourceData.Resource data, Texture2D texture, Point point)
    {
      Id = id;
      Texture = texture;
      Point = point;

      Name = Path.GetFileName(Texture.Name);

      SetData(data);
    }

    public void SetData(ResourceData.Resource data)
    {
      Data = data;

      var tileSize = BaseGame.TileSize;
      var a = data.PointOffset.X * tileSize;
      var b = data.PointOffset.Y * tileSize;

      var x = Position.X + a;
      var y = Position.Y + b;
      var width = (Data.Size.X * BaseGame.TileSize);
      var height = (Data.Size.Y * BaseGame.TileSize) - b;

      ClickableComponent = new ClickableComponent()
      {
        ClickRectangle = () => new(
          (int)x,
          (int)y,
          width,
          height
        ),
        OnClicked = () => HandleOnClicked(data),
      };
    }

    private void HandleOnClicked(ResourceData.Resource data)
    {
      if (!BaseGame.GWM.ResourceTypeData.ResourceTypes.ContainsKey(Data.Type))
        return;

      var resrouceType = BaseGame.GWM.ResourceTypeData.ResourceTypes[Data.Type];

      var model = new ContextMenuModel()
      {
        Name = resrouceType.Name,
        Position = Position,
        Items = resrouceType.Actions.Select(action => new ContextMenuItemModel()
        {
          Label = $"{action.Key}{(_cancallableActions.ContainsKey(action.Key) ? $" - cancel" : string.Empty)}",
          OnClick = () =>
          {
            if (action.Value.CanCancel)
            {
              if (!_cancallableActions.ContainsKey(action.Key))
              {
                var job = BaseGame.GWM.JobManager.AddJob(Point + Data.PointOffset, new HarvestedResourceModel()
                {
                  ResourceId = Id,
                  ResourceName = Data.Drop,
                  HarvestTime = data.HarvestTime,
                });
                _cancallableActions.Add(action.Key, job.Id);
              }
              else
              {
                var jobId = _cancallableActions[action.Key];
                _cancallableActions.Remove(action.Key);
                BaseGame.GWM.JobManager.RemoveJobById(jobId);
              }
            }
          },
        }),
      };

      BaseGame.GWM.GUIManager.OpenContextMenu(model);
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.1f);
    }
  }
}
