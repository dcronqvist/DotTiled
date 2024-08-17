using System;

namespace DotTiled.Model.Layers;

public enum DataEncoding
{
  Csv,
  Base64
}

public enum DataCompression
{
  GZip,
  ZLib,
  ZStd
}

[Flags]
public enum FlippingFlags : uint
{
  None = 0,
  FlippedHorizontally = 0x80000000u,
  FlippedVertically = 0x40000000u,
  FlippedDiagonally = 0x20000000u,
  RotatedHexagonal120 = 0x10000000u
}

public class Chunk
{
  // Attributes
  public required int X { get; set; }
  public required int Y { get; set; }
  public required uint Width { get; set; }
  public required uint Height { get; set; }

  // Data
  public required uint[] GlobalTileIDs { get; set; }
  public required FlippingFlags[] FlippingFlags { get; set; }
}

public class Data
{
  // Attributes
  public DataEncoding? Encoding { get; set; }
  public DataCompression? Compression { get; set; }

  // Data
  public uint[]? GlobalTileIDs { get; set; }
  public FlippingFlags[]? FlippingFlags { get; set; }
  public Chunk[]? Chunks { get; set; }
}
