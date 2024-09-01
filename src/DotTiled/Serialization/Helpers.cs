using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DotTiled.Serialization;

internal static partial class Helpers
{
  internal static Func<string, T> CreateMapper<T>(Func<string, Exception> noMatch, params (string, T)[] mappings)
  {
    return s =>
    {
      foreach (var (key, value) in mappings)
      {
        if (key == s)
          return value;
      }

      throw noMatch(s);
    };
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

  internal static List<IProperty> CreateInstanceOfCustomClass(
    CustomClassDefinition customClassDefinition,
    Func<string, ICustomTypeDefinition> customTypeResolver)
  {
    return customClassDefinition.Members.Select(x =>
    {
      if (x is ClassProperty cp)
      {
        return new ClassProperty
        {
          Name = cp.Name,
          PropertyType = cp.PropertyType,
          Value = CreateInstanceOfCustomClass((CustomClassDefinition)customTypeResolver(cp.PropertyType), customTypeResolver)
        };
      }

      return x.Clone();
    }).ToList();
  }

  internal static IList<IProperty> MergeProperties(IList<IProperty>? baseProperties, IList<IProperty>? overrideProperties)
  {
    if (baseProperties is null)
      return overrideProperties ?? [];

    if (overrideProperties is null)
      return baseProperties;

    var result = baseProperties.Select(x => x.Clone()).ToList();
    foreach (var overrideProp in overrideProperties)
    {
      if (!result.Any(x => x.Name == overrideProp.Name))
      {
        result.Add(overrideProp);
        continue;
      }
      else
      {
        var existingProp = result.First(x => x.Name == overrideProp.Name);
        if (existingProp is ClassProperty classProp)
        {
          classProp.Value = MergeProperties(classProp.Value, ((ClassProperty)overrideProp).Value);
        }
        else
        {
          ReplacePropertyInList(result, overrideProp);
        }
      }
    }

    return result;
  }

  internal static void ReplacePropertyInList(List<IProperty> properties, IProperty property)
  {
    var index = properties.FindIndex(p => p.Name == property.Name);
    if (index == -1)
      properties.Add(property);
    else
      properties[index] = property;
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

  internal static bool StringIsXml(string s) => s.StartsWith("<?xml", StringComparison.InvariantCulture);
}
