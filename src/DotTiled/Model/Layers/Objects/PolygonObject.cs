using System.Collections.Generic;
using System.Numerics;

namespace DotTiled.Model;

public class PolygonObject : Object
{
  // Attributes
  public required List<Vector2> Points { get; set; }
}
