using Microsoft.Xna.Framework;
using System;
using Village123.Shared.Input;

namespace Village123.Shared.Interfaces
{
  public interface IClickable
  {
    Rectangle ClickRectangle { get; }
    float ClickLayer { get; }
    bool ClickIsVisible { get; }
    Action OnLeftClick { get; }
    Action OnMouseOver { get; }

    public void UpdateMouse()
    {
      if (GameMouse.Intersects(ClickRectangle))
      {
        GameMouse.AddObject(this);
        OnMouseOver?.Invoke();

        if (GameMouse.ValidObject == this)
        {
          if (GameMouse.IsLeftClicked)
          {
            OnLeftClick?.Invoke();
          }
        }
      }
    }
  }
}
