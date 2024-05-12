using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Managers;

namespace Village123.Shared.GUI.Controls
{
  public class BuildPanel
  {
    private GameWorldManager _gwm;

    private List<Button> _buttons;

    public BuildPanel(GameWorldManager gwm)
    {
      _gwm = gwm;

      var categories = _gwm.PlaceCategoryData.Categories;

      var font = gwm.GameModel.Content.Load<SpriteFont>("Font");

      var buttonWidth = Math.Min(200, BaseGame.ScreenWidth / categories.Count);
      var buttonHeight = buttonWidth / 5;

      var texture = CreateBorderedTexture(_gwm.GameModel.GraphicsDevice, buttonWidth, buttonHeight, Color.White, Color.Black, 1);

      _buttons = _gwm.PlaceCategoryData.Categories.Select((c, i) =>
        new Button(c, font, texture) { Position = new Vector2(i * buttonWidth, BaseGame.ScreenHeight - buttonHeight) }
      ).ToList();
    }

    private static Texture2D CreateBorderedTexture(GraphicsDevice graphicsDevice, int width, int height, Color fillColor, Color borderColor, int maxBorderWidth = 5)
    {
      int borderWidth = Math.Min(Math.Min(width, height) / 10, maxBorderWidth);

      var texture = new Texture2D(graphicsDevice, width, height);

      Color[] data = new Color[texture.Width * texture.Height];

      // Fill the entire texture with the fill color
      for (int i = 0; i < data.Length; ++i)
      {
        data[i] = fillColor;
      }

      // Draw the border lines
      for (int x = 0; x < texture.Width; x++)
      {
        for (int y = 0; y < texture.Height; y++)
        {
          if (x < borderWidth || x >= texture.Width - borderWidth ||
              y < borderWidth || y >= texture.Height - borderWidth)
          {
            data[y * texture.Width + x] = borderColor;
          }
        }
      }

      texture.SetData(data);

      return texture;
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
