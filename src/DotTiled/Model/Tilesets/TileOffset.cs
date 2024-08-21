namespace DotTiled.Model;

/// <summary>
/// Is used to specify an offset in pixels in tilesets, to be applied when drawing a tile from the related tileset.
/// </summary>
public class TileOffset
{
  /// <summary>
  /// The horizontal offset in pixels.
  /// </summary>
  public float X { get; set; } = 0f;

  /// <summary>
  /// The vertical offset in pixels.
  /// </summary>
  public float Y { get; set; } = 0f;
}
