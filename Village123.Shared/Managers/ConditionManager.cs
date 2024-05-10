using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village123.Shared.Entities;

namespace Village123.Shared.Managers
{
  public class ConditionManager
  {
    private Villager _villager;

    public ConditionManager(Villager villager)
    {
      _villager = villager;
    }

    public void SetRate(string condition, float amount)
    {
      if (!_villager.Conditions.ContainsKey(condition))
        return;

      _villager.Conditions[condition].Rate = amount;
    }

    public void ResetCondition(string condition)
    {
      if (!_villager.Conditions.ContainsKey(condition))
        return;

      _villager.Conditions[condition].ResetRate();
    }

    public float GetValue(string condition)
    {
      if (!_villager.Conditions.ContainsKey(condition))
        return 0f; // Not sure what to do here tbh

      return _villager.Conditions[condition].Value;
    }
  }
}
