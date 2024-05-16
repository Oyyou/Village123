using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Village123.Shared.GUI.Controls;

namespace Village123.Shared.Managers
{
  public class GUIManager
  {
    private readonly GameWorldManager _gwm;

    private readonly BuildPanel _buildPanel;

    public GUIManager(GameWorldManager gwm)
    {
      _gwm = gwm;

      _buildPanel = new BuildPanel(_gwm);
    }

    public void Update(GameTime gameTime)
    {
      _buildPanel.Update(gameTime);
      CraftingPanel.GetInstance(_gwm).Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
      _buildPanel.Draw(spriteBatch);
      CraftingPanel.GetInstance(_gwm).Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
