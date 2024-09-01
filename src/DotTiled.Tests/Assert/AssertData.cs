namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertData(Data? expected, Data? actual)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    AssertEqual(expected.Encoding, actual.Encoding, nameof(Data.Encoding));
    AssertEqual(expected.Compression, actual.Compression, nameof(Data.Compression));

    // Data
    AssertEqual(expected.GlobalTileIDs, actual.GlobalTileIDs, nameof(Data.GlobalTileIDs));
    AssertEqual(expected.FlippingFlags, actual.FlippingFlags, nameof(Data.FlippingFlags));

    if (expected.Chunks.HasValue)
    {
      AssertEqual(expected.Chunks.Value.Length, actual.Chunks.Value.Length, "Chunks.Length");
      for (var i = 0; i < expected.Chunks.Value.Length; i++)
        AssertChunk(expected.Chunks.Value[i], actual.Chunks.Value[i]);
    }
  }

  private static void AssertChunk(Chunk expected, Chunk actual)
  {
    // Attributes
    AssertEqual(expected.X, actual.X, nameof(Chunk.X));
    AssertEqual(expected.Y, actual.Y, nameof(Chunk.Y));
    AssertEqual(expected.Width, actual.Width, nameof(Chunk.Width));
    AssertEqual(expected.Height, actual.Height, nameof(Chunk.Height));

    // Data
    AssertEqual(expected.GlobalTileIDs, actual.GlobalTileIDs, nameof(Chunk.GlobalTileIDs));
    AssertEqual(expected.FlippingFlags, actual.FlippingFlags, nameof(Chunk.FlippingFlags));
  }
}
