using System.Collections.Generic;

namespace Village123.Shared.Models
{
  public class ProducedItemModel
  {
    public string ItemName { get; set; }
    public Dictionary<string, string> Materials { get; set; }
  }
}
