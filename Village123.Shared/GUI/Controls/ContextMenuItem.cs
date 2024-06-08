using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Village123.Shared.GUI.Controls.Bases;
using Village123.Shared.Utils;

namespace Village123.Shared.GUI.Controls
{
  public class ContextMenuItem : Control
  {
    public ContextMenuItem(string label)
    {
      var font = BaseGame.GWM.GameModel.Content.Load<SpriteFont>("Font");
      var texture = TextureHelpers.CreateBorderedTexture(
        BaseGame.GWM.GameModel.GraphicsDevice,
        100,
        20,
        Color.White,
        Color.Black,
        1
      );

      var button = new Button(font, texture, label);

      this.AddChild(button);
    }
  }
}
