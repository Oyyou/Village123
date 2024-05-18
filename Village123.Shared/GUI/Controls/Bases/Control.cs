using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Interfaces;

namespace Village123.Shared.GUI.Controls.Bases
{
  public abstract class Control : IClickable
  {
    protected Vector2 _position;
    protected Rectangle _clickRectangle;
    protected bool _isEnabled = true;

    public bool IsEnabled => Parent != null ? (Parent.IsEnabled && _isEnabled) : _isEnabled;

    public float Layer { get; set; } = 0.9f;

    public float ClickLayer
    {
      get
      {
        if (Parent != null)
        {
          return Parent.ClickLayer + 0.01f;
        }

        return Layer;
      }
    }
    protected virtual bool IsClickable => true;
    public bool ClickIsVisible => Parent != null ? (Parent.ClickIsVisible && IsClickable) : IsClickable;
    public abstract int Width { get; }
    public abstract int Height { get; }

    public bool IsVisible { get; set; } = true;

    public Action OnClicked { get; set; }
    public Action OnClickedOutside { get; set; }
    public Rectangle ClickRectangle => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

    public Control Parent { get; set; }
    public string Tag { get; set; }

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

    public abstract Action OnLeftClickOutside { get; }

    protected Control(Vector2 position)
    {
      _position = position;
    }

    public void SetPosition(Vector2 position)
    {
      _position = position;
    }

    public void AddChild(Control control, string tag = "")
    {
      control.Parent = this;
      control.Tag = tag;
      this.Children.Add(control);
    }

    protected void RemoveChildrenByTag(string tag)
    {
      this.Children = this.Children.Where(c => c.Tag != tag).ToList();
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
