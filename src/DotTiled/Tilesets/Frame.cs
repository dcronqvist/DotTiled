namespace DotTiled;

/// <summary>
/// A single frame of an animated tile.
/// </summary>
public class Frame
{
  /// <summary>
  /// The local tile ID within the parent tileset.
  /// </summary>
  public required uint TileID { get; set; }

  /// <summary>
  /// How long (in milliseconds) this frame should be displayed before advancing to the next frame.
  /// </summary>
  public required int Duration { get; set; }
}
