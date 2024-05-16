﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Village123.Shared.Interfaces;

namespace Village123.Shared.GUI.Controls.Bases
{
  public abstract class Control : IClickable
  {
    protected Vector2 _position;
    protected Rectangle _clickRectangle;

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
    public Rectangle ClickRectangle => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

    public Control Parent { get; set; }

    public List<Control> Children { get; set; } = new List<Control>();

    public Vector2 Position
    {
      get
      {
        if (Parent != null)
        {
          return Parent.Position + _position;
        }

        return _position;
      }
    }

    public abstract Action OnLeftClick { get; }

    public abstract Action OnMouseOver { get; }

    protected Control(Vector2 position)
    {
      _position = position;
    }

    public void SetPosition(Vector2 position)
    {
      _position = position;
    }

    public void AddChild(Control control)
    {
      control.Parent = this;
      this.Children.Add(control);
    }

    public virtual void Update(GameTime gameTime)
    {
      ((IClickable)this).UpdateMouse();

      foreach (var child in Children)
      {
        child.Update(gameTime);
      }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      foreach (var child in Children)
      {
        child.Draw(spriteBatch);
      }
    }
  }
}
