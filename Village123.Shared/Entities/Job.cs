using Microsoft.Xna.Framework;

namespace Village123.Shared.Entities
{
  public class Job : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public Point Point { get; set; }
  }
}
