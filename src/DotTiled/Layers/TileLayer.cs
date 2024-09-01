namespace DotTiled;

/// <summary>
/// Represents a tile layer in a map.
/// </summary>
public class TileLayer : BaseLayer
{
  /// <summary>
  /// The X coordinate of the layer in tiles.
  /// </summary>
  public uint X { get; set; } = 0;

  /// <summary>
  /// The Y coordinate of the layer in tiles.
  /// </summary>
  public uint Y { get; set; } = 0;

  /// <summary>
  /// The width of the layer in tiles. Always the same as the map width for fixed-size maps.
  /// </summary>
  public required uint Width { get; set; }

  /// <summary>
  /// The height of the layer in tiles. Always the same as the map height for fixed-size maps.
  /// </summary>
  public required uint Height { get; set; }

  /// <summary>
  /// The tile layer data.
  /// </summary>
  public Optional<Data> Data { get; set; } = Optional<Data>.Empty;
}
