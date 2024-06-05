using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class BuildPanel
  {
    private readonly List<Button> _buttons;

    public BuildPanel()
    {
      var categories = BaseGame.GWM.PlaceCategoryData.Categories;

      var font = BaseGame.GWM.GameModel.Content.Load<SpriteFont>("Font");

      var buttonWidth = Math.Min(200, BaseGame.ScreenWidth / categories.Count);
      var buttonHeight = buttonWidth / 5;

      var texture = TextureHelpers.CreateBorderedTexture(BaseGame.GWM.GameModel.GraphicsDevice, buttonWidth, buttonHeight, Color.White, Color.Black, 1);

      _buttons = new List<Button>();
      //_buttons = BaseGame.GWM.PlaceCategoryData.Categories.Select((c, i) =>
      //  new Button(font, texture, c.Value.Name, new Vector2(i * buttonWidth, BaseGame.ScreenHeight - buttonHeight))
      //  {
      //    Layer = 0.8f,
      //  }
      //).ToList();

      _buttons.Add(new Button(
        font,
        texture,
        "Build Wooden Chest",
        new Vector2(0, BaseGame.ScreenHeight - buttonHeight)
      )
      {
        OnClicked = () =>
        {
          BaseGame.GWM.BuildManager.Build(BaseGame.GWM.PlaceData.Places["woodenChest"]);
        }
      });

      _buttons.Add(new Button(
        font,
        texture,
        "Build Anvil",
        _buttons.Last().Position + new Vector2(buttonWidth, 0)
      )
      {
        OnClicked = () =>
        {
          BaseGame.GWM.BuildManager.Build(BaseGame.GWM.PlaceData.Places["anvil"]);
        }
      });

      _buttons.Add(new Button(
        font,
        texture,
        "Build Farm",
        _buttons.Last().Position + new Vector2(buttonWidth, 0)
      )
      {
        OnClicked = () =>
        {
          BaseGame.GWM.BuildManager.Build(BaseGame.GWM.PlaceData.Places["farmPlot"]);
        }
      });
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
      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

      foreach (var button in _buttons)
      {
        button.Draw(spriteBatch);
      }

      spriteBatch.End();
    }
  }
}
