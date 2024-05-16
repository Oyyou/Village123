using Microsoft.Xna.Framework;
using System;
using Village123.Shared.GUI.Controls;
using Village123.Shared.Input;

namespace Village123.Shared.Interfaces
{
  public interface IClickable
  {
    Rectangle ClickRectangle { get; }
    float ClickLayer { get; }
    bool ClickIsVisible { get; }
    Action OnLeftClick { get; }
    Action OnIsMouseOver { get; }

    public void UpdateMouse()
    {
      if (GameMouse.Intersects(ClickRectangle))
      {
        GameMouse.AddObject(this);
        OnIsMouseOver?.Invoke();

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
