using System.Collections.Generic;
using Village123.Shared.Data;

namespace Village123.Shared.Models
{
  public class CraftItemModel
  {
    public KeyValuePair<string, ItemData.Item> Item { get; set; }
    public Dictionary<string, string> Materials { get; set; }
  }
}
