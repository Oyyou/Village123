using Microsoft.Xna.Framework;
using System;
using Village123.Shared.Input;
using Village123.Shared.Interfaces;

namespace Village123.Shared.GUI.Controls.Bases
{
  public abstract class Control : IClickable
  {
    protected Vector2 _position;
    protected Rectangle _clickRectangle;
    protected bool _isMouseOver = false;
    protected Action _onPositionChanged = null;

    public float ClickLayer
    {
      get
      {
        if (Parent != null)
        {
          return Parent.ClickLayer + 0.01f;
        }

        return 0.9f;
      }
    }
    public abstract bool ClickIsVisible { get; }
    public abstract int Width { get; }
    public abstract int Height { get; }

    public Action OnClicked { get; set; }
    public Rectangle ClickRectangle => _clickRectangle;

    public Control Parent { get; set; }

    public Vector2 Position
    {
      get => _position;
      set
      {
        _position = value;
        UpdateClickRectangle();
        _onPositionChanged?.Invoke();
      }
    }

    protected Control(Vector2 position)
    {
      Position = position;
    }

    protected void UpdateClickRectangle()
    {
      _clickRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
    }

    public virtual void Update(GameTime gameTime)
    {
      _isMouseOver = false;

      if (GameMouse.Intersects(ClickRectangle))
      {
        GameMouse.AddObject(this);

        // If this control is what the gameMouse is able to currently click (based off what control is layered at the top)
        if (GameMouse.ValidObject == this)
        {
          _isMouseOver = true;
          if (GameMouse.IsLeftClicked)
          {
            OnClicked?.Invoke();
          }
        }
      }
    }
  }
}
