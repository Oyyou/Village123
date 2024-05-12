using Microsoft.Xna.Framework;

namespace Village123.Shared.Interfaces
{
  public interface IClickable
  {
    Rectangle ClickRectangle { get; }

    float ClickLayer { get; }

    bool ClickIsVisible { get; }
  }
}
