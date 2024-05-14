using System.Collections.Generic;
using Village123.Shared.Entities;
using Village123.Shared.Interfaces;
using Village123.Shared.Models;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Utils
{
  public static class VillagerActionInitializerRegistry
  {
    private static readonly List<IVillagerActionInitializer> _initializers = new();

    public static void RegisterInitializer(IVillagerActionInitializer initializer)
    {
      _initializers.Add(initializer);
    }

    internal static void InitializeActions(Villager villager, GameWorld gameWorld, IVillagerAction action)
    {
      foreach (var initializer in _initializers)
      {
        initializer.InitializeAction(villager, gameWorld, action);
      }
    }
  }
}
