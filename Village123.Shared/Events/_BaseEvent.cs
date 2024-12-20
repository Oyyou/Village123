using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village123.Shared.Events
{
  public abstract class BaseEvent
  {
    public bool IsComplete { get; set; } = false;

    public abstract void Update(GameTime gameTime);
  }
}
