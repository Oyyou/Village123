﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ItemList : Control
  {
    public readonly Rectangle Viewport;
    private readonly Texture2D _backgroundTexture;
    private readonly Texture2D _forgroundTexture;

    private readonly ScrollBar _scrollBar;

    private float _viewY = 0f;

    public override int Width => Viewport.Width;
    public override int Height => Viewport.Height;

    public ItemList(Rectangle viewport)
    {
      Viewport = viewport;
      _viewportPosition = new Vector2(
        Viewport.X,
        Viewport.Y
      );

      _backgroundTexture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        Viewport.Width,
        Viewport.Height,
        Color.White,
        Color.Black,
        1
      );

      _forgroundTexture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        Viewport.Width,
        Viewport.Height,
        new Color(0, 0, 0, 0),
        Color.Black,
        1
      );

      _scrollBar = new ScrollBar(
        new Rectangle(0, 0, Viewport.Width, Viewport.Height),
        new(Viewport.X, Viewport.Y),
        this
      );

      _scrollBar.OnUpdated = () => _viewY = -_scrollBar.Offset;
    }

    protected override void OnAddChild(Control control)
    {
      var position = new Vector2(5, 5);
      var children = this.Children.Where(c => !(c is ScrollBar)).ToList();
      for (int i = 0; i < children.Count; i++)
      {
        var child = children[i];

        child.Position = position;

        position += new Vector2(child.Width + 5, 0);

        if (position.X > _scrollBar.DrawPosition.X - 5)
        {
          position = new Vector2(5, position.Y + child.Height + 5);
        }
      }

      _scrollBar.CalculateThumbSizeAndSpeed(new Rectangle(0, 0, Viewport.Width, Viewport.Height));
    }

    public override void Update(GameTime gameTime)
    {
      _scrollBar.Update(gameTime);
      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      var originalVp = BaseGame.GWM.GameModel.GraphicsDevice.Viewport;

      BaseGame.GWM.GameModel.GraphicsDevice.Viewport = new Viewport(Viewport);

      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

      spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.Red);
      _scrollBar.Draw(spriteBatch);

      spriteBatch.End();

      var matrix = Matrix.CreateTranslation(0, _viewY, 0);
      spriteBatch.Begin(
        SpriteSortMode.FrontToBack,
        samplerState: SamplerState.PointClamp, // Upscale pixels
        transformMatrix: matrix
      );

      foreach (var child in Children)
      {
        if (child is ScrollBar)
        {
          continue;
        }
        child.ClickableComponent.Camera = matrix;
        child.Draw(spriteBatch);
      }

      spriteBatch.End();

      // Draws a boarder on top layer to ensure children are below
      spriteBatch.Begin();
      spriteBatch.Draw(_forgroundTexture, new Vector2(0, 0), Color.White);
      spriteBatch.End();

      BaseGame.GWM.GameModel.GraphicsDevice.Viewport = originalVp;
    }
  }
}
