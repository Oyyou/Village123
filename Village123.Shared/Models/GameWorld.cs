using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village123.Shared.Entities;

namespace Village123.Shared.Models
{
  public class GameWorld
  {
    public Map Map { get; private set; }

    public GameWorld(Map map)
    {
      Map = map;
    }
  }
}
