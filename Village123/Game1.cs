using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared;
using Village123.Shared.Input;

namespace Village123
{
  public class Game1 : BaseGame
  {
    protected override void LoadContent()
    {
      base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      XScale = (float)GraphicsDevice.PresentationParameters.BackBufferWidth / 480;
      YScale = (float)GraphicsDevice.PresentationParameters.BackBufferHeight / 270;
      base.Update(gameTime);

      GameMouse.Update(gameTime);

      GWM.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      ScaleMatrix = Matrix.CreateScale(XScale, YScale, 1);

      GWM.Draw(_spriteBatch);

      base.Draw(gameTime);
    }
  }
}
