using System;
using System.IO;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal static Data ReadDataAsChunks(JsonElement element, Optional<DataCompression> compression, DataEncoding encoding)
  {
    var chunks = element.GetValueAsList<Chunk>(e => ReadChunk(e, compression, encoding)).ToArray();
    return new Data
    {
      Chunks = chunks,
      Compression = compression,
      Encoding = encoding
    };
  }

  internal static Chunk ReadChunk(JsonElement element, Optional<DataCompression> compression, DataEncoding encoding)
  {
    var data = element.GetRequiredPropertyCustom<Data>("data", e => ReadDataWithoutChunks(e, compression, encoding));
    var x = element.GetRequiredProperty<int>("x");
    var y = element.GetRequiredProperty<int>("y");
    var width = element.GetRequiredProperty<int>("width");
    var height = element.GetRequiredProperty<int>("height");

    return new Chunk
    {
      X = x,
      Y = y,
      Width = width,
      Height = height,
      GlobalTileIDs = data.GlobalTileIDs.Value,
      FlippingFlags = data.FlippingFlags.Value
    };
  }

  internal static Data ReadDataWithoutChunks(JsonElement element, Optional<DataCompression> compression, DataEncoding encoding)
  {
    if (encoding == DataEncoding.Csv)
    {
      // Array of uint
      var data = element.GetValueAsList<uint>(e => e.GetValueAs<uint>()).ToArray();
      var (globalTileIDs, flippingFlags) = Helpers.ReadAndClearFlippingFlagsFromGIDs(data);
      return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags };
    }
    else if (encoding == DataEncoding.Base64)
    {
      var base64Data = element.GetBytesFromBase64();

      if (!compression.HasValue)
      {
        var data = Helpers.ReadBytesAsInt32Array(base64Data);
        var (globalTileIDs, flippingFlags) = Helpers.ReadAndClearFlippingFlagsFromGIDs(data);
        return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags };
      }

      using var stream = new MemoryStream(base64Data);
      var decompressed = compression.Value switch
      {
        DataCompression.GZip => Helpers.DecompressGZip(stream),
        DataCompression.ZLib => Helpers.DecompressZLib(stream),
        DataCompression.ZStd => throw new NotSupportedException("ZStd compression is not supported."),
        _ => throw new InvalidOperationException($"Unsupported compression '{compression}'.")
      };

      {
        var (globalTileIDs, flippingFlags) = Helpers.ReadAndClearFlippingFlagsFromGIDs(decompressed);
        return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags };
      }
    }

    throw new JsonException($"Unsupported encoding '{encoding}'.");
  }
}
