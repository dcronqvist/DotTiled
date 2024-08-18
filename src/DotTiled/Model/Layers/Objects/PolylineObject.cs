using System.Collections.Generic;
using System.Numerics;

namespace DotTiled.Model;

public class PolylineObject : Object
{
  // Attributes
  public required List<Vector2> Points { get; set; }
}
