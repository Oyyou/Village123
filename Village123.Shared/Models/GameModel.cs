using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Village123.Shared.Models
{
  public class GameModel
  {
    public readonly GraphicsDeviceManager GraphicsDeviceManager;

    public readonly ContentManager Content;

    public readonly SpriteBatch SpriteBatch;

    public GraphicsDevice GraphicsDevice
    {
      get
      {
        return GraphicsDeviceManager.GraphicsDevice;
      }
    }

    public GameModel(GraphicsDeviceManager graphicsDeviceManager, ContentManager content, SpriteBatch spriteBatch)
    {
      GraphicsDeviceManager = graphicsDeviceManager;
      Content = content;
      SpriteBatch = spriteBatch;
    }
  }
}
