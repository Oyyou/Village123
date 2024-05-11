using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Village123.Shared.Entities
{
  public class Job : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public Point Point { get; set; }
    public List<int> WorkerIds { get; set; } = new();
    public int MaxWorkers { get; set; } = 1;
    public List<string> RequiredEquipment { get; set; } = new();
    public List<string> RequiredResources { get; set; } = new();
  }
}
