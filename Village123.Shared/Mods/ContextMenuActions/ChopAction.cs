using System;
using Village123.Shared.Components;

namespace Village123.Shared.Mods.ContextMenuActions
{
  public class ChopAction : IContextMenuAction
  {
    public string Label => "Chop";

    public Action<ContextMenuComponent> OnClick => (contextMenuComponent) =>
    {
      // BaseGame.GWM.JobManager.AddJob(contextMenuComponent.Point, );
    };

    public Action<ContextMenuComponent> OnCancel => (contextMenuComponent) =>
    {

    };
  }
}
