using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.Managers;

namespace Village123
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameWorldManager _gwm;

    public Game1()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      _gwm = new GameWorldManager();
      _gwm.Load();
    }

    protected override void Update(GameTime gameTime)
    {
      _gwm.Update(gameTime);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      base.Draw(gameTime);
    }
  }
}
