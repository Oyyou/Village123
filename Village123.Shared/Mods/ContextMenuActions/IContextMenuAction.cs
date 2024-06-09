using System;
using Village123.Shared.Components;

namespace Village123.Shared.Mods.ContextMenuActions
{
  public interface IContextMenuAction
  {
    string Label { get; }
    Action<ContextMenuComponent> OnClick { get; }
    Action<ContextMenuComponent> OnCancel { get; }
  }
}
