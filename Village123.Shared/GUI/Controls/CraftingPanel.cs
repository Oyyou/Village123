using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Data;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  internal class CraftingPanel : Control
  {
    private static CraftingPanel _instance;
    private static readonly object _lock = new();

    private readonly GameWorldManager _gwm;
    private readonly Texture2D _panelTexture;

    private List<Button> _buttons;
    private Button _closeButton;
    private bool _isOpen = false;

    private CraftingPanel(GameWorldManager gwm) : base(new Vector2(100, 100))
    {
      _gwm = gwm;

      _panelTexture = TextureHelpers.CreateBorderedTexture(
        _gwm.GameModel.GraphicsDevice,
        BaseGame.ScreenWidth - (int)(Position.X * 2),
        BaseGame.ScreenHeight - (int)(Position.Y * 2),
        Color.White,
        Color.Black,
        1
      );
    }

    public static CraftingPanel GetInstance(GameWorldManager gwm)
    {
      lock (_lock)
      {
        _instance ??= new CraftingPanel(gwm);
      }

      return _instance;
    }

    public override bool ClickIsVisible => true;

    public override int Width => _panelTexture != null ? _panelTexture.Width : 0;

    public override int Height => _panelTexture != null ? _panelTexture.Width : 0;

    public void SetPlace(PlaceData.Place place)
    {
      _isOpen = false;
      if (place == null)
      {
        return;
      }

      _isOpen = true;

      var items = _gwm.ItemData.GetItemsByCategory(place.Category);

      var font = _gwm.GameModel.Content.Load<SpriteFont>("Font");
      var buttonWidth = 200;
      var buttonHeight = buttonWidth / 5;
      var texture = TextureHelpers.CreateBorderedTexture(_gwm.GameModel.GraphicsDevice, buttonWidth, buttonHeight, Color.White, Color.Black, 1);

      _buttons = items.Select((item, i) => new Button(font, texture, item.Name, Position + new Vector2(20, ((texture.Height) * i) + 20))).ToList();
    }

    public override void Update(GameTime gameTime)
    {
      if (!_isOpen)
      {
        return;
      }

      base.Update(gameTime);
      foreach (var button in _buttons)
      {
        button.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (!_isOpen)
      {
        return;
      }

      spriteBatch.Draw(_panelTexture, Position, Color.White);
      foreach (var button in _buttons)
      {
        button.Draw(spriteBatch);
      }
    }
  }
}
