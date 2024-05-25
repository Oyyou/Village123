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

    private readonly Button _topButton;
    private readonly Button _thumbButton;
    private readonly Button _bottomButton;

    private float _speed = 1f;
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
      _thumbButton = new Button(buttonTexture, _topButton.DrawPosition + new Vector2(0, buttonTexture.Height + 2));
      _bottomButton = new Button(buttonTexture, new Vector2(4, _background.Height - (buttonTexture.Height + 4)));


      _topButton.OnClicked = () => SetBarButtonY(_thumbButton.Position.Y - _speed);
      _bottomButton.OnClicked = () => SetBarButtonY(_thumbButton.Position.Y + _speed);

      AddChild(_topButton);
      AddChild(_thumbButton);
      AddChild(_bottomButton);

      _thumbButton.ClickableComponent.OnHeld = ThumbOnHeld;

      CalculateThumbSizeAndSpeed(parentContainer);
    }

    public void CalculateThumbSizeAndSpeed(Rectangle parentContainer)
    {
      var contentHeight = GetContentHeight();
      var viewportHeight = parentContainer.Height;

      if(contentHeight < viewportHeight)
      {
        this.IsVisible = false;
        return;
      }

      var viewableRatio = viewportHeight / contentHeight;
      var scrollBarArea = (_bottomButton.ClickRectangle.Top - _topButton.ClickRectangle.Bottom) - 8;
      var thumbHeight = scrollBarArea * viewableRatio;

      var buttonTexture = TextureHelpers.CreateBorderedTexture(
        _gwm.GameModel.GraphicsDevice,
        _background.Width - 8,
        (int)thumbHeight,
        Color.DarkGray,
        Color.DarkGray,
        1
      );

      _thumbButton.UpdateTexture(buttonTexture);

      this.IsVisible = true;

      var scrollTrackSpace = contentHeight - viewportHeight;
      var scrollThumbSpace = viewportHeight - thumbHeight;
      _speed = (scrollTrackSpace / scrollThumbSpace) * 10;
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
      var min = _topButton.Position.Y + _topButton.Height + 2;
      var max = (_bottomButton.Position.Y - 2) - _thumbButton.Height;

      var newY = MathHelper.Clamp(y, min, max);

      var change = newY - min;
      var percentage = change / (max - min);
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
