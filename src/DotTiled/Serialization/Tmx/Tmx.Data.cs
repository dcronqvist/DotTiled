using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

internal partial class Tmx
{
  internal static Data ReadData(XmlReader reader, bool usesChunks)
  {
    var encoding = reader.GetOptionalAttributeEnum<DataEncoding>("encoding", e => e switch
    {
      "csv" => DataEncoding.Csv,
      "base64" => DataEncoding.Base64,
      _ => throw new XmlException("Invalid encoding")
    });
    var compression = reader.GetOptionalAttributeEnum<DataCompression>("compression", c => c switch
    {
      "gzip" => DataCompression.GZip,
      "zlib" => DataCompression.ZLib,
      "zstd" => DataCompression.ZStd,
      _ => throw new XmlException("Invalid compression")
    });

    if (usesChunks)
    {
      var chunks = reader
        .ReadList("data", "chunk", (r) => ReadChunk(r, encoding, compression))
        .ToArray();
      return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = null, Chunks = chunks };
    }

    var usesTileChildrenInsteadOfRawData = encoding is null && compression is null;
    if (usesTileChildrenInsteadOfRawData)
    {
      var tileChildrenGlobalTileIDsWithFlippingFlags = ReadTileChildrenInWrapper("data", reader);
      var (tileChildrenGlobalTileIDs, tileChildrenFlippingFlags) = ReadAndClearFlippingFlagsFromGIDs(tileChildrenGlobalTileIDsWithFlippingFlags);
      return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = tileChildrenGlobalTileIDs, FlippingFlags = tileChildrenFlippingFlags, Chunks = null };
    }

    var rawDataGlobalTileIDsWithFlippingFlags = ReadRawData(reader, encoding!.Value, compression);
    var (rawDataGlobalTileIDs, rawDataFlippingFlags) = ReadAndClearFlippingFlagsFromGIDs(rawDataGlobalTileIDsWithFlippingFlags);
    return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = rawDataGlobalTileIDs, FlippingFlags = rawDataFlippingFlags, Chunks = null };
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

  internal static uint[] ReadTileChildrenInWrapper(string wrapper, XmlReader reader) =>
    reader.ReadList(wrapper, "tile", (r) => r.GetOptionalAttributeParseable<uint>("gid") ?? 0).ToArray();

  internal static uint[] ReadRawData(XmlReader reader, DataEncoding encoding, DataCompression? compression)
  {
    var data = reader.ReadElementContentAsString();
    if (encoding == DataEncoding.Csv)
      return ParseCsvData(data);

    using var bytes = new MemoryStream(Convert.FromBase64String(data));
    if (compression is null)
      return ReadMemoryStreamAsInt32Array(bytes);

    var decompressed = compression switch
    {
      DataCompression.GZip => DecompressGZip(bytes),
      DataCompression.ZLib => DecompressZLib(bytes),
      DataCompression.ZStd => throw new NotSupportedException("ZStd compression is not supported."),
      _ => throw new XmlException("Invalid compression")
    };

    return decompressed;
  }

  internal static uint[] ParseCsvData(string data)
  {
    var values = data
      .Split((char[])['\n', '\r', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
      .Select(uint.Parse)
      .ToArray();
    return values;
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
}
