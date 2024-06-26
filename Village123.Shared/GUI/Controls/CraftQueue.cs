﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Village123.Shared.Entities;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  internal class CraftQueue : Control
  {
    private readonly Place _place;
    private readonly Texture2D _backgroundTexture;

    public override int Width => _backgroundTexture.Width;
    public override int Height => _backgroundTexture.Height;

    public CraftQueue(Place place, int width, int height)
    {
      _place = place;

      _backgroundTexture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        width,
        height,
        Color.White,
        Color.Black,
        1
      );
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var id in _place.JobIds)
      {
        var job = BaseGame.GWM.JobManager.Jobs.FirstOrDefault(j => j.Id == id);
        if (!Children.Any(c => (int)c.Key == job.Id))
        {
          var texture = TextureHelpers.CreateBorderedTexture(
            BaseGame.GWM.GameModel.GraphicsDevice,
            Height - 4,
            Height - 4,
            Color.White,
            Color.Black,
            1
          );

          AddChild(new Button(texture, new Vector2(2, 2)) { Key = job.Id });
        }
      }

      base.Update(gameTime);
    }

    protected override void OnAddChild(Control control)
    {
      for (int i = 0; i < Children.Count; i++)
      {
        var child = Children[i];
        child.Position = new Vector2(2 + ((child.Width + 2) * i), 2);
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
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

      base.Draw(spriteBatch);
    }
  }
}
