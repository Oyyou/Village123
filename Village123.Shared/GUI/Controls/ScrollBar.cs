using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Input;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ScrollBar : Control
  {
    private readonly GameWorldManager _gwm;
    private readonly Rectangle _viewport;
    private readonly Texture2D _background;

    private Button _topButton;
    private Button _thumbButton;
    private Button _bottomButton;

    private float _min;
    private float _max;
    private float _speed = 10f;
    private float _remaining;
    private int _previousScrollValue;

    public ScrollBar(
      GameWorldManager gwm,
      Rectangle parentContainer,
      Rectangle viewport
      )
    {
      _gwm = gwm;
      _viewport = viewport;

      _background = TextureHelpers.CreateBorderedTexture(
        _gwm.GameModel.GraphicsDevice,
        20,
        parentContainer.Height - 10,
        Color.LightGray,
        Color.LightGray,
        1
      );

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

      var viewportPosition = new Vector2(viewport.X, viewport.Y);
      _topButton.ViewportPosition = viewportPosition;
      _thumbButton.ViewportPosition = viewportPosition;
      _bottomButton.ViewportPosition = viewportPosition;

      AddChild(_topButton);
      AddChild(_thumbButton);
      AddChild(_bottomButton);
    }

    private void SetBarButtonY(float y)
    {
      var newY = MathHelper.Clamp(y, _min, _max);

      var change = newY - _min;
      var percentage = change / _speed;
      var offset = _remaining * percentage;

      _thumbButton.Position = new Vector2(_thumbButton.Position.X, newY);
      // this.Parent.ChildrenOffset = new Vector2(0, -offset);
    }

    private void ThumbOnHeld()
    {
      var position = GameMouse.GetPositionWithViewport(ViewportPosition.Value) - this.DrawPosition;

      SetBarButtonY(position.Y - (_thumbButton.ClickRectangle.Height / 2));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
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
