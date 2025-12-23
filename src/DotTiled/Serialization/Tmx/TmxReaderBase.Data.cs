using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DotTiled.Serialization.Tmx;

public abstract partial class TmxReaderBase
{
  internal Data ReadData(bool usesChunks)
  {
    var encoding = _reader.GetOptionalAttributeEnum<DataEncoding>("encoding", e => e switch
    {
      "csv" => DataEncoding.Csv,
      "base64" => DataEncoding.Base64,
      _ => throw new XmlException("Invalid encoding")
    });
    var compression = _reader.GetOptionalAttributeEnum<DataCompression>("compression", c => c switch
    {
      "gzip" => DataCompression.GZip,
      "zlib" => DataCompression.ZLib,
      "zstd" => DataCompression.ZStd,
      _ => throw new XmlException("Invalid compression")
    });

    if (usesChunks)
    {
      var chunks = _reader
        .ReadList("data", "chunk", (r) => ReadChunk(encoding, compression))
        .ToArray();
      return new Data { Encoding = encoding, Compression = compression, Chunks = chunks };
    }

    var usesTileChildrenInsteadOfRawData = !encoding.HasValue && !compression.HasValue;
    if (usesTileChildrenInsteadOfRawData)
    {
      var tileChildrenGlobalTileIDsWithFlippingFlags = ReadTileChildrenInWrapper("data", _reader);
      var (tileChildrenGlobalTileIDs, tileChildrenFlippingFlags) = ReadAndClearFlippingFlagsFromGIDs(tileChildrenGlobalTileIDsWithFlippingFlags);
      return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = tileChildrenGlobalTileIDs, FlippingFlags = tileChildrenFlippingFlags };
    }

    var rawDataGlobalTileIDsWithFlippingFlags = ReadRawData(_reader, encoding, compression);
    var (rawDataGlobalTileIDs, rawDataFlippingFlags) = ReadAndClearFlippingFlagsFromGIDs(rawDataGlobalTileIDsWithFlippingFlags);
    return new Data { Encoding = encoding, Compression = compression, GlobalTileIDs = rawDataGlobalTileIDs, FlippingFlags = rawDataFlippingFlags };
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
    reader.ReadList(wrapper, "tile", (r) => r.GetOptionalAttributeParseable<uint>("gid").GetValueOr(0)).ToArray();

  internal static uint[] ReadRawData(XmlReader reader, Optional<DataEncoding> encoding, Optional<DataCompression> compression)
  {
    var data = reader.ReadElementContentAsString();
    if (encoding.Value == DataEncoding.Csv)
      return ParseCsvData(data);

    using var bytes = new MemoryStream(Convert.FromBase64String(data));
    if (!compression.HasValue)
      return ReadMemoryStreamAsInt32Array(bytes);

    var decompressed = compression.Value switch
    {
      DataCompression.GZip => Helpers.DecompressGZip(bytes),
      DataCompression.ZLib => Helpers.DecompressZLib(bytes),
      DataCompression.ZStd => Helpers.DecompressZStd(bytes),
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
}
