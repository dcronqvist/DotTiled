using System.Collections.Generic;
using System.Numerics;

namespace DotTiled;

/// <summary>
/// A polyline object in a map. The existing <see cref="Object.X"/> and <see cref="Object.Y"/> properties are used as
/// the origin of the polyline.
/// </summary>
public class PolylineObject : Object
{
  /// <summary>
  /// The points that make up the polyline. <see cref="Object.X"/> and <see cref="Object.Y"/> are used as the origin of the polyline.
  /// </summary>
  public required List<Vector2> Points { get; set; }
}
