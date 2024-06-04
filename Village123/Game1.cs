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
      base.Update(gameTime);

      GameMouse.Update(gameTime);

      GWM.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      GWM.Draw(_spriteBatch);

      base.Draw(gameTime);
    }
  }
}
