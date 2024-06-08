using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Village123.Shared.Models
{
  public class ContextMenuItemModel
  {
    public string Label { get; init; }
    public Action OnClick { get; set; }
  }

  public class ContextMenuModel
  {
    public string Name { get; init; }
    public Vector2 Position { get; init; }
    public IEnumerable<ContextMenuItemModel> Items { get; init; }
  }
}
