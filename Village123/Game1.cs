﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared;
using Village123.Shared.Input;
using Village123.Shared.Managers;
using Village123.Shared.Models;

namespace Village123
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameModel _gameModel;

    private GameWorldManager _gwm;

    public Game1()
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

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

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
