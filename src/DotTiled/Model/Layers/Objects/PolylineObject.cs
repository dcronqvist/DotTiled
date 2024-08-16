using System.Collections.Generic;
using System.Numerics;

namespace DotTiled.Model.Layers.Objects;

public class PolylineObject : Object
{
  // Attributes
  public required List<Vector2> Points { get; set; }
}
