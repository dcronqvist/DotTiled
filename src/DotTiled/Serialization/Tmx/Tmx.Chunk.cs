using System.Xml;
using DotTiled.Model.Layers;

namespace DotTiled.Serialization.Tmx;

internal partial class Tmx
{
  internal static Chunk ReadChunk(XmlReader reader, DataEncoding? encoding, DataCompression? compression)
  {
    var x = reader.GetRequiredAttributeParseable<int>("x");
    var y = reader.GetRequiredAttributeParseable<int>("y");
    var width = reader.GetRequiredAttributeParseable<uint>("width");
    var height = reader.GetRequiredAttributeParseable<uint>("height");

    var usesTileChildrenInsteadOfRawData = encoding is null;
    if (usesTileChildrenInsteadOfRawData)
    {
      var globalTileIDsWithFlippingFlags = ReadTileChildrenInWrapper("chunk", reader);
      var (globalTileIDs, flippingFlags) = ReadAndClearFlippingFlagsFromGIDs(globalTileIDsWithFlippingFlags);
      return new Chunk { X = x, Y = y, Width = width, Height = height, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags };
    }
    else
    {
      var globalTileIDsWithFlippingFlags = ReadRawData(reader, encoding!.Value, compression);
      var (globalTileIDs, flippingFlags) = ReadAndClearFlippingFlagsFromGIDs(globalTileIDsWithFlippingFlags);
      return new Chunk { X = x, Y = y, Width = width, Height = height, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags };
    }
  }
}
