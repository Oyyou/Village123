using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared;
using Village123.Shared.Input;
using Village123.Shared.Managers;
using Village123.Shared.Models;

namespace Village123
{
  public class Game1 : BaseGame
  {
    private GameModel _gameModel;
    private GameWorldManager _gwm;

    protected override void LoadContent()
    {
      base.LoadContent();

      _gameModel = new GameModel(
        _graphics,
        Content,
        _spriteBatch);

      _gwm = new GameWorldManager(_gameModel);
    }

    protected override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      GameMouse.Update(gameTime);

      _gwm.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      _gwm.Draw(_spriteBatch);

      base.Draw(gameTime);
    }
  }
}
