using Newtonsoft.Json;

namespace Village123.Shared.Models
{
  public class Condition
  {
    public float Value { get; set; }
    public float Rate { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
    public float _defaultRate;

    public Condition(float value, float rate)
    {
      Value = value;
      Rate = rate;

      _defaultRate = Rate;
    }

    public void ResetRate()
    {
      Rate = _defaultRate;
    }
  }
}
