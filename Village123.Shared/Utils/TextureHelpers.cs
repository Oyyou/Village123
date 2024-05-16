using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Village123.Shared.Utils
{
  public static class TextureHelpers
  {
    public static Texture2D CreateBorderedTexture(GraphicsDevice graphicsDevice, int width, int height, Color fillColor, Color borderColor, int maxBorderWidth = 5)
    {
      int borderWidth = Math.Min(Math.Min(width, height) / 10, maxBorderWidth);

      var texture = new Texture2D(graphicsDevice, width, height);

      var data = new Color[texture.Width * texture.Height];

      // Fill the entire texture with the fill color
      for (int i = 0; i < data.Length; ++i)
      {
        data[i] = fillColor;
      }

      // Draw the border lines
      for (int x = 0; x < texture.Width; x++)
      {
        for (int y = 0; y < texture.Height; y++)
        {
          if (x < borderWidth || x >= texture.Width - borderWidth ||
              y < borderWidth || y >= texture.Height - borderWidth)
          {
            data[y * texture.Width + x] = borderColor;
          }
        }
      }

      texture.SetData(data);

      return texture;
    }
  }
}
