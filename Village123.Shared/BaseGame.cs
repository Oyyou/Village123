using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Village123.Shared.Managers;
using Village123.Shared.Models;

namespace Village123.Shared
{
  public class BaseGame : Game
  {
    public static float XScale { get; protected set; } = 1f;
    public static float YScale { get; protected set; } = 1f;
    public static Vector2 Scale => new Vector2(XScale, YScale);
    public static Matrix ScaleMatrix { get; protected set; }

    protected GraphicsDeviceManager _graphics;
    protected SpriteBatch _spriteBatch;
    protected GameModel _gameModel;

    public const short TileSize = 32;

    public static Random Random = new();
    public static int ScreenWidth = 1280;
    public static int ScreenHeight = 720;

    public static GameWorldManager GWM;

    public BaseGame()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      _graphics.PreferredBackBufferWidth = BaseGame.ScreenWidth;
      _graphics.PreferredBackBufferHeight = BaseGame.ScreenHeight;
      _graphics.ApplyChanges();

      _gameModel = new GameModel(
        _graphics,
        Content,
        _spriteBatch);

      GWM = new GameWorldManager(_gameModel);
      GWM.Load();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);
    }
  }
}
