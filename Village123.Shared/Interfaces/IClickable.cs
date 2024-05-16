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
    Action OnLeftClickOutside { get; }
    Action OnMouseOver { get; }

    public void UpdateMouse()
    {
      if (GameMouse.Intersects(ClickRectangle))
      {
        if (GameMouse.ValidObject == this)
        {
          OnMouseOver?.Invoke();

          if (GameMouse.IsLeftClicked)
          {
            OnLeftClick?.Invoke();
          }
        }

        GameMouse.AddObject(this);
      }
      else
      {
        GameMouse.RemoveObject(this);

        if (GameMouse.IsLeftClicked)
        {
          OnLeftClickOutside?.Invoke();
        }
      }
    }
  }
}
