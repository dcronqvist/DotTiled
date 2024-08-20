using System;

namespace DotTiled.Model;

/// <summary>
/// Specifies the encoding used to encode the tile layer data.
/// </summary>
public enum DataEncoding
{
  /// <summary>
  /// The data is stored as comma-separated values.
  /// </summary>
  Csv,

  /// <summary>
  /// The data is stored as base64-encoded binary data.
  /// </summary>
  Base64
}

/// <summary>
/// Specifies the compression algorithm used to compress the tile layer data.
/// </summary>
public enum DataCompression
{
  /// <summary>
  /// GZip compression.
  /// </summary>
  GZip,

  /// <summary>
  /// ZLib compression.
  /// </summary>
  ZLib,

  /// <summary>
  /// ZStandard compression. Currently not supported by DotTiled and will throw an exception if encountered.
  /// </summary>
  ZStd
}

/// <summary>
/// The flipping flags for a tile. These can be used to check how a tile is flipped or rotated. Uses the
/// <see href="https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-flagsattribute">FlagsAttribute, for which there is plenty of documentation.</see>
/// </summary>
[Flags]
public enum FlippingFlags : uint
{
  /// <summary>
  /// No flipping.
  /// </summary>
  None = 0,

  /// <summary>
  /// The tile is flipped horizontally.
  /// </summary>
  FlippedHorizontally = 0x80000000u,

  /// <summary>
  /// The tile is flipped vertically.
  /// </summary>
  FlippedVertically = 0x40000000u,

  /// <summary>
  /// The tile is flipped diagonally.
  /// </summary>
  FlippedDiagonally = 0x20000000u,

  /// <summary>
  /// In hexagonal maps, the tile is rotated 120 degrees clockwise.
  /// </summary>
  RotatedHexagonal120 = 0x10000000u
}

/// <summary>
/// Represents part of a tile layer of a map that is infinite.
/// </summary>
public class Chunk
{
  /// <summary>
  /// The X coordinate of the chunk in tiles.
  /// </summary>
  public required int X { get; set; }

  /// <summary>
  /// The Y coordinate of the chunk in tiles.
  /// </summary>
  public required int Y { get; set; }

  /// <summary>
  /// The width of the chunk in tiles.
  /// </summary>
  public required uint Width { get; set; }

  /// <summary>
  /// The height of the chunk in tiles.
  /// </summary>
  public required uint Height { get; set; }

  /// <summary>
  /// The parsed chunk data, as a list of tile GIDs.
  /// To get an actual tile ID, you map it to a local tile ID using the correct tileset. Please refer to
  /// <see href="https://doc.mapeditor.org/en/stable/reference/global-tile-ids/#mapping-a-gid-to-a-local-tile-id">the documentation on how to do this</see>.
  /// </summary>
  public required uint[] GlobalTileIDs { get; set; }

  /// <summary>
  /// The parsed flipping flags for each tile in the chunk. Appear in the same order as the tiles in the layer in <see cref="GlobalTileIDs"/>.
  /// </summary>
  public required FlippingFlags[] FlippingFlags { get; set; }
}

/// <summary>
/// Represents the data of a tile layer.
/// </summary>
public class Data
{
  /// <summary>
  /// The encoding used to encode the tile layer data.
  /// </summary>
  public DataEncoding? Encoding { get; set; }

  /// <summary>
  /// The compression method used to compress the tile layer data.
  /// </summary>
  public DataCompression? Compression { get; set; }

  /// <summary>
  /// The parsed tile layer data, as a list of tile GIDs.
  /// To get an actual tile ID, you map it to a local tile ID using the correct tileset. Please refer to
  /// <see href="https://doc.mapeditor.org/en/stable/reference/global-tile-ids/#mapping-a-gid-to-a-local-tile-id">the documentation on how to do this</see>.
  /// </summary>
  public uint[]? GlobalTileIDs { get; set; }

  /// <summary>
  /// The parsed flipping flags for each tile in the layer. Appear in the same order as the tiles in the layer in <see cref="GlobalTileIDs"/>.
  /// </summary>
  public FlippingFlags[]? FlippingFlags { get; set; }

  /// <summary>
  /// If the map is infinite, it will instead contain a list of chunks.
  /// </summary>
  public Chunk[]? Chunks { get; set; }
}
