using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DotTiled.Model;

namespace DotTiled.Serialization;

internal static partial class Helpers
{
  internal static uint[] ReadMemoryStreamAsInt32Array(Stream stream)
  {
    var finalValues = new List<uint>();
    var int32Bytes = new byte[4];
    while (stream.Read(int32Bytes, 0, 4) == 4)
    {
      var value = BitConverter.ToUInt32(int32Bytes, 0);
      finalValues.Add(value);
    }
    return [.. finalValues];
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

  internal static ImageFormat ParseImageFormatFromSource(string source)
  {
    var extension = Path.GetExtension(source).ToLowerInvariant();
    return extension switch
    {
      ".png" => ImageFormat.Png,
      ".gif" => ImageFormat.Gif,
      ".jpg" => ImageFormat.Jpg,
      ".jpeg" => ImageFormat.Jpg,
      ".bmp" => ImageFormat.Bmp,
      _ => throw new NotSupportedException($"Unsupported image format '{extension}'")
    };
  }

  internal static Dictionary<string, IProperty> MergeProperties(Dictionary<string, IProperty>? baseProperties, Dictionary<string, IProperty>? overrideProperties)
  {
    if (baseProperties is null)
      return overrideProperties ?? new Dictionary<string, IProperty>();

    if (overrideProperties is null)
      return baseProperties;

    var result = baseProperties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Clone());
    foreach (var (key, value) in overrideProperties)
    {
      if (!result.TryGetValue(key, out var baseProp))
      {
        result[key] = value;
        continue;
      }
      else
      {
        if (value is ClassProperty classProp)
        {
          ((ClassProperty)baseProp).Properties = MergeProperties(((ClassProperty)baseProp).Properties, classProp.Properties);
        }
        else
        {
          result[key] = value;
        }
      }
    }

    return result;
  }

  internal static void SetAtMostOnce<T>(ref T? field, T value, string fieldName)
  {
    if (field is not null)
      throw new InvalidOperationException($"{fieldName} already set");

    field = value;
  }

  internal static void SetAtMostOnceUsingCounter<T>(ref T? field, T value, string fieldName, ref int counter)
  {
    if (counter > 0)
      throw new InvalidOperationException($"{fieldName} already set");

    field = value;
    counter++;
  }
}
