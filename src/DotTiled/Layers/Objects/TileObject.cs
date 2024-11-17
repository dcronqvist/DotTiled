namespace DotTiled;

/// <summary>
/// A tile object in a map.
/// </summary>
public class TileObject : Object
{
  /// <summary>
  /// A reference to a tile.
  /// </summary>
  public uint GID { get; set; }

  /// <summary>
  /// The flipping flags for the tile.
  /// </summary>
  public FlippingFlags FlippingFlags { get; set; }
}
