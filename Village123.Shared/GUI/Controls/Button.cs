﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Village123.Shared.GUI.Controls.Bases;

namespace Village123.Shared.GUI.Controls
{
  public class Button : Control
  {
    private bool _isMouseOver = false;

    private Label _label;

    public Texture2D Texture { get; init; }

    public Color BackgroundColor { get; set; } = Color.White;
    public Color TextColor
    {
      get => _label.TextColor;
      set => _label.TextColor = value;
    }
    public Color HoverBackgroundColor { get; set; } = Color.Gray;

    public override bool ClickIsVisible => true;

    public override int Width => Texture != null ? Texture.Width : 0;

    public override int Height => Texture != null ? Texture.Height : 0;

    public override Action OnLeftClick => OnClicked;

    public override Action OnMouseOver => () => _isMouseOver = true;

    public override Action OnLeftClickOutside => () => { };

    public Button(SpriteFont font, Texture2D texture, string text, Vector2 position) : base(position)
    {
      Texture = texture;

      _label = new Label(font, text, Vector2.Zero);

      AddChild(_label);
    }

    public override void Update(GameTime gameTime)
    {
      _label.SetPosition(
        new Vector2(
          (Width / 2) - (_label.Width / 2),
          (Height / 2) - (_label.Height / 2)
          )
        );
      _isMouseOver = false;

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      spriteBatch.Draw(
        Texture,
        Position,
        null,
        _isMouseOver ? HoverBackgroundColor : BackgroundColor,
        0f,
        new Vector2(0, 0),
        1f,
        SpriteEffects.None,
        ClickLayer
      );
    }
  }
}
