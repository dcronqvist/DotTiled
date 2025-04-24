using System;

namespace DotTiled;

/// <summary>
/// Represents a tile layer in a map.
/// </summary>
public class TileLayer : BaseLayer
{
  /// <summary>
  /// The X coordinate of the layer in tiles.
  /// </summary>
  public int X { get; set; } = 0;

  /// <summary>
  /// The Y coordinate of the layer in tiles.
  /// </summary>
  public int Y { get; set; } = 0;

  /// <summary>
  /// The width of the layer in tiles. Always the same as the map width for fixed-size maps.
  /// </summary>
  public required int Width { get; set; }

  /// <summary>
  /// The height of the layer in tiles. Always the same as the map height for fixed-size maps.
  /// </summary>
  public required int Height { get; set; }

  /// <summary>
  /// The tile layer data.
  /// </summary>
  public Optional<Data> Data { get; set; } = Optional<Data>.Empty;

  /// <summary>
  /// Helper method to retrieve the Global Tile ID at a given coordinate in the layer.
  /// </summary>
  /// <param name="x">The X coordinate in the layer</param>
  /// <param name="y">The Y coordinate in the layer</param>
  /// <returns>The Global Tile ID at the given coordinate.</returns>
  /// <exception cref="InvalidOperationException">Thrown when either <see cref="Data"/> or <see cref="Data.GlobalTileIDs"/> are missing values.</exception>
  /// <exception cref="ArgumentException">Thrown when the given coordinate is not within bounds of the layer.</exception>
  public uint GetGlobalTileIDAtCoord(int x, int y)
  {
    if (!Data.HasValue)
      throw new InvalidOperationException("Data is not set.");

    if (x < 0 || x >= Width || y < 0 || y >= Height)
      throw new ArgumentException("Coordinates are out of bounds.");

    if (!Data.Value.GlobalTileIDs.HasValue)
      throw new InvalidOperationException("GlobalTileIDs is not set.");

    return Data.Value.GlobalTileIDs.Value[(y * Width) + x];
  }
}
