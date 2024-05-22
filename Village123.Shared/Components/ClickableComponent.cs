﻿using Microsoft.Xna.Framework;
using System;
using Village123.Shared.Input;

namespace Village123.Shared.Components
{
  public class ClickableComponent
  {
    private Rectangle _windowRectangle;

    public Func<Rectangle> ClickRectangle { get; set; } = () => new Rectangle();
    public bool IsMouseOver { get; protected set; } = false;
    public bool IsHeld { get; protected set; } = false;
    public bool IsMouseDown { get; protected set; } = false;
    public bool IsMouseClicked { get; protected set; } = false;
    public Func<bool> IsClickable { get; set; } = () => true;
    public Func<float> ClickLayer { get; set; } = () => 0.1f;
    public Action OnHover { get; set; } = null;
    public Action OnHeld { get; set; } = null;
    public Action OnClicked { get; set; } = null;
    public Action OnClickedOutside { get; set; } = null;

    public ClickableComponent()
    {
      _windowRectangle = new Rectangle(
          0,
          0,
          BaseGame.ScreenWidth,
          BaseGame.ScreenHeight
        );
    }

    public virtual void Update(GameTime gameTime)
    {
      IsMouseOver = false;
      IsMouseDown = false;
      IsMouseClicked = false;

      if (GameMouse.Intersects(ClickRectangle()))
      {
        if (GameMouse.ValidObject == this)
        {
          IsMouseOver = true;
          OnHover?.Invoke();

          if (GameMouse.IsLeftPressed)
          {
            IsMouseDown = true;
            IsHeld = true;
          }
          else
          {
            IsHeld = false;
          }

          if (GameMouse.IsLeftClicked)
          {
            IsMouseClicked = true;
            OnClicked?.Invoke();
          }
        }
        GameMouse.AddObject(this);
      }
      else
      {
        GameMouse.RemoveObject(this);

        if (GameMouse.Intersects(_windowRectangle) && GameMouse.IsLeftClicked)
        {
          OnClickedOutside?.Invoke();
        }

        if (!GameMouse.IsLeftPressed)
        {
          IsHeld = false;
        }
      }

      if (IsHeld)
      {
        OnHeld?.Invoke();
      }
    }
  }
}
