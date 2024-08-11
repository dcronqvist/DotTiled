using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace DotTiled;

internal static partial class Helpers
{
  internal static class Data
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
  }
}
