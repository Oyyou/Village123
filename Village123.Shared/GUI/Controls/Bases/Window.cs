using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls.Bases
{
  public abstract class Window : Control
  {
    protected readonly GameWorldManager _gwm;
    protected readonly Texture2D _windowTexture;
    protected readonly SpriteFont _font;
    protected List<ItemList> _itemLists = new();

    public bool IsOpen = false;

    public override int Width => _windowTexture.Width;
    public override int Height => _windowTexture.Height;

    public Window(GameWorldManager gwm, Vector2 position, int width, int height, string title)
      : base()
    {
      _gwm = gwm;
      Position = position;

      ClickableComponent.IsClickable = () => IsOpen;
      ClickableComponent.OnClickedOutside = () => IsOpen = false;

      _windowTexture = TextureHelpers.CreateBorderedTexture(
        gwm.GameModel.GraphicsDevice,
        width,
        height,
        Color.White,
        Color.Black,
        1
      );
      _font = gwm.GameModel.Content.Load<SpriteFont>("Font");

      AddChild(GetCloseButton(gwm));
      AddChild(new Label(_font, title, new Vector2(10, 10)));
    }

    private Button GetCloseButton(GameWorldManager gwm)
    {
      var closeButtonTexture = TextureHelpers.CreateBorderedTexture(gwm.GameModel.GraphicsDevice, 30, 30, Color.White, Color.Black, 1);
      var padding = 10;
      var button = new Button(
        _font,
        closeButtonTexture,
        "X",
        new Vector2(ClickRectangle.Width - (closeButtonTexture.Width + padding), padding)
      )
      {
        OnClicked = () => IsOpen = false,
      };

      return button;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      foreach (var itemList in _itemLists)
      {
        itemList.Update(gameTime);
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
      base.Draw(spriteBatch);
      spriteBatch.Draw(
        _windowTexture,
        DrawPosition,
        null,
        Color.White,
        0f,
        new Vector2(0, 0),
        1f,
        SpriteEffects.None,
        ClickableComponent.ClickLayer()
      );
      spriteBatch.End();

      foreach (var itemList in _itemLists)
      {
        itemList.Draw(spriteBatch);
      }
    }
  }
}
