using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Village123.Shared.Models;

namespace Village123.Shared.Entities
{
  public class Job : IEntity
  {
    public int Id { get; set; }
    public HarvestedResourceModel HarvestedResource { get; set; } = null;
    public ProducedItemModel ProducedItem { get; set; } = null;
    public string Type { get; set; }
    public Point Point { get; set; }
    public List<int> WorkerIds { get; set; } = new();
    public int MaxWorkers { get; set; } = 1;
    public int Progress {  get; set; } = 0;
    // public List<string> RequiredEquipment { get; set; } = new();
  }
}
