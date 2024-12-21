using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Interfaces;
using Village123.Shared.Components;

namespace Village123.Shared.Input
{
  public static class GameMouse
  {
    private static MouseState _currentMouse;
    private static MouseState _previouseMouse;
    private static Matrix _cameraMatrix = Matrix.Identity;

    public static bool ClickEnabled { get; set; } = true;

    #region IClickable related
    /// <summary>
    /// These are objects the mouse is currently hovering over
    /// </summary>
    public static List<ClickableComponent> ClickableComponents = new List<ClickableComponent>();

    /// <summary>
    /// The single object we're able to click
    /// </summary>
    public static ClickableComponent ValidObject
    {
      get
      {
        if (!ClickEnabled) return null;

        var resp = ClickableComponents
          .Where(c => c.IsClickable())
          .OrderBy(c => c.ClickLayer()).LastOrDefault();

        return resp;
      }
    }

    public static void AddObject(ClickableComponent component)
    {
      if (!ClickableComponents.Contains(component))
        ClickableComponents.Add(component);
    }

    public static void RemoveObject(ClickableComponent component)
    {
      ClickableComponents.Remove(component);
    }

    #endregion

    public static bool IsLeftClicked
    {
      get
      {
        return _previouseMouse.LeftButton == ButtonState.Pressed &&
          _currentMouse.LeftButton == ButtonState.Released;
      }
    }

    public static bool IsLeftPressed
    {
      get
      {
        return _currentMouse.LeftButton == ButtonState.Pressed;
      }
    }

    public static bool IsRightPressed
    {
      get
      {
        return _currentMouse.RightButton == ButtonState.Pressed;
      }
    }

    public static bool IsRightClicked
    {
      get
      {
        return _previouseMouse.RightButton == ButtonState.Pressed &&
          _currentMouse.RightButton == ButtonState.Released;
      }
    }

    public static bool IsLeftReleased
    {
      get
      {
        return _currentMouse.LeftButton == ButtonState.Released;
      }
    }

    public static bool IsInWindow(int width, int height)
    {
      return X >= 0 && X <= width &&
        Y >= 0 && Y <= height;
    }

    public static Point Position
    {
      get
      {
        return _currentMouse.Position;
      }
    }

    public static Point MapPoint
    {
      get
      {
        var transformedPosition = Vector2.Transform(Position.ToVector2(), Matrix.Invert(_cameraMatrix));
        return new Point(
            (int)Math.Floor(transformedPosition.X / BaseGame.TileSize),
            (int)Math.Floor(transformedPosition.Y / BaseGame.TileSize)
        );
      }
    }

    public static Vector2 GetPositionWithViewport(Vector2 viewportPosition)
    {
      return Position.ToVector2() - viewportPosition;
    }

    public static Point PositionWithCamera(Matrix _camera)
    {
      if (_camera == Matrix.Identity)
        return Position;

      Vector3 scale;
      _camera.Decompose(out scale, out _, out _);

      var translation = _camera.Translation;

      var scaleX = 1f / scale.X;
      var scaleY = 1f / scale.Y;

      var x = (int)((Position.X - translation.X) * scaleX);
      var y = (int)((Position.Y - translation.Y) * scaleY);

      return Vector2.Transform(new Vector2(X, Y), Matrix.Invert(_camera)).ToPoint();

      return new Point(x, y);
    }

    public static Rectangle RectangleWithCamera(Matrix camera)
    {
      var x = (int)PositionWithCamera(camera).X;
      var y = (int)PositionWithCamera(camera).Y;
      return new Rectangle(x, y, 1, 1);
    }
    public static Rectangle Rectangle
    {
      get
      {
        return new Rectangle(X, Y, 1, 1);
      }
    }

    public static int ScrollWheelValue
    {
      get
      {
        return _currentMouse.ScrollWheelValue;
      }
    }

    public static int X
    {
      get
      {
        return Position.X;
      }
    }

    public static int Y
    {
      get
      {
        return Position.Y;
      }
    }

    public static void Update(GameTime gameTime)
    {
      // Clear();
      _previouseMouse = _currentMouse;
      _currentMouse = Mouse.GetState();
    }

    public static bool Intersects(Rectangle rectangle, Matrix? matrix = null)
    {
      if (matrix != null)
        return RectangleWithCamera(matrix.Value).Intersects(rectangle);

      var transformedPosition = Vector2.Transform(Position.ToVector2(), Matrix.Invert(_cameraMatrix));
      return new Rectangle((int)transformedPosition.X, (int)transformedPosition.Y, 1, 1).Intersects(rectangle);
    }

    public static void SetCameraMatrix(Matrix cameraMatrix)
    {
      _cameraMatrix = cameraMatrix;
    }

    public static void Clear()
    {
      ClickableComponents.Clear();
    }
  }
}
