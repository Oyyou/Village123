using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Managers;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class BuildPanel
  {
    private readonly GameWorldManager _gwm;
    private readonly List<Button> _buttons;

    public BuildPanel(GameWorldManager gwm)
    {
      _gwm = gwm;

      var categories = _gwm.PlaceCategoryData.Categories;

      var font = gwm.GameModel.Content.Load<SpriteFont>("Font");

      var buttonWidth = Math.Min(200, BaseGame.ScreenWidth / categories.Count);
      var buttonHeight = buttonWidth / 5;

      var texture = TextureHelpers.CreateBorderedTexture(_gwm.GameModel.GraphicsDevice, buttonWidth, buttonHeight, Color.White, Color.Black, 1);

      _buttons = _gwm.PlaceCategoryData.Categories.Select((c, i) =>
        new Button(font, texture, c, new Vector2(i * buttonWidth, BaseGame.ScreenHeight - buttonHeight))
      ).ToList();
    }

    public void Update(GameTime gameTime)
    {
      foreach (var button in _buttons)
      {
        button.Update(gameTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var button in _buttons)
      {
        button.Draw(spriteBatch);
      }
    }
  }
}
