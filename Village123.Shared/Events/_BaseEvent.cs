using Microsoft.Xna.Framework;

namespace Village123.Shared.Events
{
  public abstract class BaseEvent
  {
    public bool IsComplete { get; set; } = false;

    public abstract void Update(GameTime gameTime);
  }
}
