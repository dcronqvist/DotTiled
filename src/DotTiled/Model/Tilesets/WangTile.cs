namespace DotTiled.Model;

/// <summary>
/// Represents a Wang tile in a Wang set.
/// </summary>
public class WangTile
{
  /// <summary>
  /// The tile ID associated with this Wang tile.
  /// </summary>
  public required uint TileID { get; set; }

  /// <summary>
  /// The Wang ID of this Wang tile.
  /// </summary>
  public required byte[] WangID { get; set; }
}
