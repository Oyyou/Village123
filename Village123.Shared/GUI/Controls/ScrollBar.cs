using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Input;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ScrollBar : Control
  {
    private readonly GameWorldManager _gwm;
    private readonly ItemList _itemList;
    private readonly Texture2D _background;

    private Button _topButton;
    private Button _thumbButton;
    private Button _bottomButton;

    private float _min;
    private float _max;
    private float _speed = 1f;
    private float _remaining;
    private int _previousScrollValue;
    public override int Width => _background != null ? _background.Width : 0;
    public override int Height => _background != null ? _background.Height : 0;

    public float Offset { get; private set; } = 0;

    public ScrollBar(
      GameWorldManager gwm,
      Rectangle parentContainer,
      Vector2 viewportPosition,
      ItemList itemList
      )
    {
      _gwm = gwm;
      _itemList = itemList;
      _viewportPosition = viewportPosition;

      _background = TextureHelpers.CreateBorderedTexture(
        _gwm.GameModel.GraphicsDevice,
        20,
        parentContainer.Height - 10,
        Color.LightGray,
        Color.LightGray,
        1
      );

      ClickableComponent.OnLightHover = () =>
      {
        if (GameMouse.ScrollWheelValue < _previousScrollValue)
          SetBarButtonY(_thumbButton.Position.Y + _speed);

        if (GameMouse.ScrollWheelValue > _previousScrollValue)
          SetBarButtonY(_thumbButton.Position.Y - _speed);

        _previousScrollValue = GameMouse.ScrollWheelValue;
      };

      Position = new Vector2(
        parentContainer.Width - (_background.Width + 5),
        5
      );

      var buttonTexture = TextureHelpers.CreateBorderedTexture(
        _gwm.GameModel.GraphicsDevice,
        _background.Width - 8,
        _background.Width - 8,
        Color.DarkGray,
        Color.DarkGray,
        1
      );

      _topButton = new Button(buttonTexture, new Vector2(4, 4));
      _thumbButton = new Button(buttonTexture, _topButton.DrawPosition + new Vector2(0, buttonTexture.Height + 4));
      _bottomButton = new Button(buttonTexture, new Vector2(4, _background.Height - (buttonTexture.Height + 4)));

      _min = _topButton.ClickRectangle.Bottom + 2;
      _max = _bottomButton.Position.Y - 2 - buttonTexture.Height;

      AddChild(_topButton);
      AddChild(_thumbButton);
      AddChild(_bottomButton);

      _thumbButton.ClickableComponent.OnHeld = ThumbOnHeld;

      CalculateThumbSizeAndSpeed(parentContainer);
    }

    public void CalculateThumbSizeAndSpeed(Rectangle parentContainer)
    {
      float contentHeight = GetContentHeight();
      float parentHeight = parentContainer.Height;

      if(contentHeight < parentHeight)
      {
        this.IsVisible = false;
        return;
      }

      float viewportHeight = parentHeight;
      float availableHeight = _max - _min;
      float thumbHeight = Math.Max(viewportHeight * (viewportHeight / contentHeight), 20);

      thumbHeight = Math.Min(thumbHeight, availableHeight);

      var buttonTexture = TextureHelpers.CreateBorderedTexture(
        _gwm.GameModel.GraphicsDevice,
        _background.Width - 8,
        (int)thumbHeight,
        Color.DarkGray,
        Color.DarkGray,
        1
      );

      _thumbButton.UpdateTexture(buttonTexture);
      _max = _bottomButton.Position.Y - 2 - buttonTexture.Height;

      this.IsVisible = true;

      _speed = (contentHeight - viewportHeight) / (availableHeight - thumbHeight);
    }

    private float GetContentHeight()
    {
      var children = _itemList.Children;
      if (children.Count == 0) return 0;

      float topY = children.First().ClickRectangle.Top - 5;
      float bottomY = children.Last().ClickRectangle.Bottom - 5;

      return bottomY - topY;
    }

    private void SetBarButtonY(float y)
    {
      var newY = MathHelper.Clamp(y, _min, _max);

      var change = newY - _min;
      var percentage = change / (_max - _min);
      Offset = percentage * (GetContentHeight() - Height);

      _thumbButton.Position = new Vector2(_thumbButton.Position.X, newY);
    }

    private void ThumbOnHeld()
    {
      var position = GameMouse.GetPositionWithViewport(ViewportPosition.Value) - this.DrawPosition;

      SetBarButtonY(position.Y - (_thumbButton.ClickRectangle.Height / 2));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!IsVisible) return;

      base.Draw(spriteBatch);

      spriteBatch.Draw(
        _background,
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
