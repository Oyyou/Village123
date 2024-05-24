using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Components;
using Village123.Shared.Interfaces;

namespace Village123.Shared.GUI.Controls.Bases
{
  public abstract class Control
  {
    protected bool _isEnabled = true;

    public ClickableComponent ClickableComponent { get; protected set; }

    public bool IsEnabled => Parent != null ? (Parent.IsEnabled && _isEnabled) : _isEnabled;
    public float Layer { get; set; } = 0.9f;

    public virtual int Width { get; }
    public virtual int Height { get; }

    public bool IsVisible { get; set; } = true;
    public Func<bool> IsSelected { get; set; } = () => false;
    public object Key { get; set; } = new();

    public Rectangle ClickRectangle => ClickableComponent.ClickRectangle();

    public Action OnClicked
    {
      get
      {
        return ClickableComponent.OnClicked;
      }
      set
      {
        ClickableComponent.OnClicked = value;
      }
    }

    public Action OnClickedOutside
    {
      get
      {
        return ClickableComponent.OnClickedOutside;
      }
      set
      {
        ClickableComponent.OnClickedOutside = value;
      }
    }
    public Action OnUpdated { get; set; }

    public Vector2? _viewportPosition = null;
    public Vector2? ViewportPosition => Parent != null ? Parent.ViewportPosition : _viewportPosition;

    public Control Parent { get; set; }
    public Vector2? ChildrenOffset {  get; set; }
    public List<Control> Children { get; set; } = new List<Control>();
    public string Tag { get; set; }

    public Vector2 Position { get; set; }

    public Vector2 DrawPosition
    {
      get
      {
        if (Parent != null)
        {
          return Parent.DrawPosition + Position + (Parent.ChildrenOffset.HasValue ? Parent.ChildrenOffset.Value : Vector2.Zero);
        }

        return Position;
      }
    }

    protected Control()
    {
      ClickableComponent = new ClickableComponent()
      {
        ClickLayer = () => Parent != null ? Parent.ClickableComponent.ClickLayer() + 0.01f : Layer,
        IsClickable = () => Parent != null ? Parent.ClickableComponent.IsClickable() : true,
        ClickRectangle = () =>
        {
          var position = DrawPosition;
          if (ViewportPosition.HasValue)
          {
            position += ViewportPosition.Value;
          }
          return new Rectangle((int)position.X, (int)position.Y, Width, Height);
        },
      };
    }

    public virtual void AddChild(Control control, string tag = "")
    {
      control.Parent = this;
      control.Tag = tag;
      this.Children.Add(control);

      OnAddChild(control);
    }

    protected virtual void OnAddChild(Control control)
    {

    }

    public void RemoveChildrenByTag(string tag)
    {
      this.Children = this.Children.Where(c => c.Tag != tag).ToList();
    }

    public virtual void Update(GameTime gameTime)
    {
      if (!IsVisible) return;

      ClickableComponent?.Update(gameTime);

      foreach (var child in Children)
      {
        child.Update(gameTime);
      }

      OnUpdated?.Invoke();
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      if (!IsVisible) return;

      foreach (var child in Children)
      {
        child.Draw(spriteBatch);
      }
    }
  }
}
