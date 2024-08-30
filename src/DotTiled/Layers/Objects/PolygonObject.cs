using System.Collections.Generic;
using System.Numerics;

namespace DotTiled;

/// <summary>
/// A polygon object in a map. The existing <see cref="Object.X"/> and <see cref="Object.Y"/> properties are used as
/// the origin of the polygon.
/// </summary>
public class PolygonObject : Object
{
  /// <summary>
  /// The points that make up the polygon.
  /// <see cref="Object.X"/> and <see cref="Object.Y"/> are used as the origin of the polygon.
  /// </summary>
  public required List<Vector2> Points { get; set; }
}
