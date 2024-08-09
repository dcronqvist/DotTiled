using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static Data ReadDataAsChunks(JsonElement element, DataCompression? compression, DataEncoding encoding)
  {
    var chunks = element.GetValueAsList<Chunk>(e => ReadChunk(e, compression, encoding)).ToArray();
    return new Data
    {
      Chunks = chunks,
      Compression = compression,
      Encoding = encoding,
      FlippingFlags = null,
      GlobalTileIDs = null
    };
  }

  internal static Chunk ReadChunk(JsonElement element, DataCompression? compression, DataEncoding encoding)
  {
    var data = ReadDataWithoutChunks(element, compression, encoding);

    var x = element.GetRequiredProperty<int>("x");
    var y = element.GetRequiredProperty<int>("y");
    var width = element.GetRequiredProperty<uint>("width");
    var height = element.GetRequiredProperty<uint>("height");

    return new Chunk
    {
      X = x,
      Y = y,
      Width = width,
      Height = height,
      GlobalTileIDs = data.GlobalTileIDs!,
      FlippingFlags = data.FlippingFlags!
    };
  }

  internal static Data ReadDataWithoutChunks(JsonElement element, DataCompression? compression, DataEncoding encoding)
  {
    if (encoding == DataEncoding.Csv)
    {
      // Array of uint
      var data = element.GetValueAsList<uint>(e => e.GetValueAs<uint>()).ToArray();
      var (globalTileIDs, flippingFlags) = ReadAndClearFlippingFlagsFromGIDs(data);
      return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags, Chunks = null };
    }
    else if (encoding == DataEncoding.Base64)
    {
      var base64Data = element.GetBytesFromBase64();

      if (compression == null)
      {
        var data = ReadBytesAsInt32Array(base64Data);
        var (globalTileIDs, flippingFlags) = ReadAndClearFlippingFlagsFromGIDs(data);
        return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags, Chunks = null };
      }

      using var stream = new MemoryStream(base64Data);
      var decompressed = compression switch
      {
        DataCompression.GZip => DecompressGZip(stream),
        DataCompression.ZLib => DecompressZLib(stream),
        _ => throw new JsonException($"Unsupported compression '{compression}'.")
      };

      {
        var (globalTileIDs, flippingFlags) = ReadAndClearFlippingFlagsFromGIDs(decompressed);
        return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = globalTileIDs, FlippingFlags = flippingFlags, Chunks = null };
      }
    }

    throw new JsonException($"Unsupported encoding '{encoding}'.");
  }

  internal static uint[] ReadMemoryStreamAsInt32Array(Stream stream)
  {
    var finalValues = new List<uint>();
    var int32Bytes = new byte[4];
    while (stream.Read(int32Bytes, 0, 4) == 4)
    {
      var value = BitConverter.ToUInt32(int32Bytes, 0);
      finalValues.Add(value);
    }
    return finalValues.ToArray();
  }

  internal static uint[] DecompressGZip(MemoryStream stream)
  {
    using var decompressedStream = new GZipStream(stream, CompressionMode.Decompress);
    return ReadMemoryStreamAsInt32Array(decompressedStream);
  }

  internal static uint[] DecompressZLib(MemoryStream stream)
  {
    using var decompressedStream = new ZLibStream(stream, CompressionMode.Decompress);
    return ReadMemoryStreamAsInt32Array(decompressedStream);
  }

  internal static uint[] ReadBytesAsInt32Array(byte[] bytes)
  {
    var intArray = new uint[bytes.Length / 4];
    for (var i = 0; i < intArray.Length; i++)
    {
      intArray[i] = BitConverter.ToUInt32(bytes, i * 4);
    }

    return intArray;
  }

  internal static (uint[] GlobalTileIDs, FlippingFlags[] FlippingFlags) ReadAndClearFlippingFlagsFromGIDs(uint[] globalTileIDs)
  {
    var clearedGlobalTileIDs = new uint[globalTileIDs.Length];
    var flippingFlags = new FlippingFlags[globalTileIDs.Length];
    for (var i = 0; i < globalTileIDs.Length; i++)
    {
      var gid = globalTileIDs[i];
      var flags = gid & 0xF0000000u;
      flippingFlags[i] = (FlippingFlags)flags;
      clearedGlobalTileIDs[i] = gid & 0x0FFFFFFFu;
    }

    return (clearedGlobalTileIDs, flippingFlags);
  }
}
