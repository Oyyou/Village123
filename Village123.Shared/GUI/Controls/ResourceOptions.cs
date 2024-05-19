using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Village123.Shared.Content;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Interfaces;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ResourceOptions : Control
  {
    private readonly GameWorldManager _gwm;
    private readonly SpriteFont _font;
    private readonly int _width;

    private Texture2D _backgroundTexture;
    private string _previousSelectedButtonKey = null;
    private string _currentSelectedButtonKey = null;

    #region Overrides
    public override int Width => _backgroundTexture != null ? _backgroundTexture.Width : 0;
    public override int Height => _backgroundTexture != null ? _backgroundTexture.Height : 0;
    public override Action OnLeftClick => () => { };
    public override Action OnMouseOver => () => { };
    public override Action OnLeftClickOutside => () => { };
    #endregion

    public ResourceOptions(GameWorldManager gwm, string resourceType, int width, Vector2 position) : base(position)
    {
      _gwm = gwm;
      _font = _gwm.GameModel.Content.Load<SpriteFont>("Font");
      _width = width;

      var buttonTexture = TextureHelpers.CreateBorderedTexture(
        gwm.GameModel.GraphicsDevice,
        100,
        20,
        Color.White,
        Color.Black,
        1
      );

      var titleLabel = new Label(_font, $"{resourceType} options", new Vector2(5, 5));
      AddChild(titleLabel);

      var startPosition = new Vector2(5, 5 + titleLabel.Height + 10);

      var resourceOptions = gwm.ResourceData.GetResourcesByType(resourceType);
      for (int i = 0; i < resourceOptions.Count; i++)
      {
        var resource = resourceOptions.ElementAt(i);
        var button = new Button(_font, buttonTexture, resource.Value.Name, startPosition);
        button.Key = resource.Key;
        button.OnClicked = () =>
        {
          _currentSelectedButtonKey = button.Key;

        };
        AddChild(button);

        startPosition += new Vector2(button.Width + 5, 0);
        if (startPosition.X > _width - (button.Width + 5))
        {
          startPosition = new Vector2(5, startPosition.Y + button.Height + 5);
        }
      }

      UpdateModifierValues();
      UpdateBackgroundTexture(gwm);
    }

    private void UpdateBackgroundTexture(GameWorldManager gwm)
    {
      var childRectangles = Children
        .Where(c => c.IsVisible)
        .Select(c => c.ClickRectangle)
        .ToList();

      if (childRectangles == null || childRectangles.Count == 0)
      {
        _backgroundTexture = null;
        return;
      }

      var bounds = childRectangles[0];

      foreach (var rect in childRectangles)
      {
        int minX = Math.Min(bounds.Left, rect.Left);
        int minY = Math.Min(bounds.Top, rect.Top);

        int maxX = Math.Max(bounds.Right, rect.Right);
        int maxY = Math.Max(bounds.Bottom, rect.Bottom);

        bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
      }

      _backgroundTexture = TextureHelpers.CreateBorderedTexture(
        gwm.GameModel.GraphicsDevice,
        _width,
        bounds.Height + 20,
        Color.White,
        Color.Black,
        1
      );
    }

    public override void Update(GameTime gameTime)
    {
      ((IClickable)this).UpdateMouse();

      foreach (var child in Children)
      {
        child.IsSelected = child.Key == _currentSelectedButtonKey;
        child.Update(gameTime);
      }

      if (_previousSelectedButtonKey != _currentSelectedButtonKey)
      {
        _previousSelectedButtonKey = _currentSelectedButtonKey;
        RemoveChildrenByTag("modifiers");

        UpdateModifierValues();
        UpdateBackgroundTexture(_gwm);
      }
    }

    private void UpdateModifierValues()
    {
      UpdateBackgroundTexture(_gwm);

      var modifiersStartPosition = new Vector2(5, ClickRectangle.Height - 5);
      var modLabel = new Label(_font, "Modifiers:", modifiersStartPosition);
      AddChild(modLabel, "modifiers");

      var resource = !string.IsNullOrEmpty(_currentSelectedButtonKey) ?
        _gwm.ResourceData.Resources[_currentSelectedButtonKey] :
        null;

      modifiersStartPosition += new Vector2(modLabel.Width + 10, 0);
      for (int i = 0; i < _gwm.ResourceModifiersData.ResourceModifiers.Count; i++)
      {
        var modifier = _gwm.ResourceModifiersData.ResourceModifiers.ElementAt(i);
        var label = new Label(_font, $"{modifier.Key}:", modifiersStartPosition);
        AddChild(label, "modifiers");
        AddChild(new Label(
          _font,
          resource != null && resource.Modifiers.ContainsKey(modifier.Key) ?
            resource.Modifiers[modifier.Key].ToString() :
            $"-",
          modifiersStartPosition + new Vector2(150, 0)), "modifiers");

        modifiersStartPosition += new Vector2(0, label.Height + 5);
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      if (_backgroundTexture != null)
      {
        spriteBatch.Draw(_backgroundTexture, Position, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, ClickLayer);
      }
    }
  }
}
