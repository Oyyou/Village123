﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ResourceOptions : Control
  {
    private readonly SpriteFont _font;
    private readonly Action<string> _onResourceSelected;
    private readonly int _width;

    private Texture2D _backgroundTexture;
    private object _previousSelectedResource = null;
    private object _currentSelectedResource = null;

    #region Overrides
    public override int Width => _backgroundTexture != null ? _backgroundTexture.Width : 0;
    public override int Height => _backgroundTexture != null ? _backgroundTexture.Height : 0;
    #endregion

    public ResourceOptions(string resourceType, Action<string> onResourceSelected, int width)
      : base()
    {
      _font = BaseGame.GWM.GameModel.Content.Load<SpriteFont>("Font");
      _onResourceSelected = onResourceSelected;
      _width = width;

      var buttonTexture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        100,
        20,
        Color.White,
        Color.Black,
        1
      );

      var titleLabel = new Label(_font, $"{resourceType} options", new Vector2(5, 5));
      AddChild(titleLabel);

      var startPosition = new Vector2(5, 5 + titleLabel.Height + 10);

      var resourceOptions = BaseGame.GWM.ResourceData.GetResourcesByType(resourceType);
      for (int i = 0; i < resourceOptions.Count; i++)
      {
        var resource = resourceOptions.ElementAt(i);
        var button = new Button(_font, buttonTexture, resource.Value.Name, startPosition);
        button.Key = resource.Value;
        button.OnClicked = () =>
        {
          _currentSelectedResource = button.Key;
          _onResourceSelected(resource.Key);
        };
        button.IsSelected = () => button.Key == _currentSelectedResource;
        AddChild(button);

        startPosition += new Vector2(button.Width + 5, 0);
        if (startPosition.X > _width - (button.Width + 5))
        {
          startPosition = new Vector2(5, startPosition.Y + button.Height + 5);
        }
      }

      UpdateModifierValues();
      UpdateBackgroundTexture();
    }

    private void UpdateBackgroundTexture()
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
        BaseGame.GWM.GameModel.GraphicsDevice,
        _width,
        bounds.Height + 20,
        Color.White,
        Color.Black,
        1
      );
    }

    public override void Update(GameTime gameTime)
    {
      ClickableComponent.Update(gameTime);

      foreach (var child in Children)
      {
        child.Update(gameTime);
      }

      if (_previousSelectedResource != _currentSelectedResource)
      {
        _previousSelectedResource = _currentSelectedResource;
        RemoveChildrenByTag("modifiers");

        UpdateModifierValues();
        UpdateBackgroundTexture();
      }
    }

    private void UpdateModifierValues()
    {
      UpdateBackgroundTexture();

      var modifiersStartPosition = new Vector2(5, ClickRectangle.Height - 5);
      var modLabel = new Label(_font, "Modifiers:", modifiersStartPosition);
      AddChild(modLabel, "modifiers");

      var resource = _currentSelectedResource != null ?
        (ResourceData.Resource)_currentSelectedResource :
        null;

      modifiersStartPosition += new Vector2(modLabel.Width + 10, 0);
      for (int i = 0; i < BaseGame.GWM.ResourceModifiersData.ResourceModifiers.Count; i++)
      {
        var modifier = BaseGame.GWM.ResourceModifiersData.ResourceModifiers.ElementAt(i);
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
        spriteBatch.Draw(
          _backgroundTexture,
          DrawPosition,
          null,
          Color.White,
          0f,
          new Vector2(0, 0),
          1f,
          SpriteEffects.None,
          ClickableComponent.ClickLayer()
        );
      }
    }
  }
}
