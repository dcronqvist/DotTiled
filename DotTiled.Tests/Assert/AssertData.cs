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
    Assert.Equal(expected.Encoding, actual.Encoding);
    Assert.Equal(expected.Compression, actual.Compression);

    // Data
    Assert.Equal(expected.GlobalTileIDs, actual.GlobalTileIDs);
    Assert.Equal(expected.FlippingFlags, actual.FlippingFlags);

    if (expected.Chunks is not null)
    {
      Assert.NotNull(actual.Chunks);
      Assert.Equal(expected.Chunks.Length, actual.Chunks.Length);
      for (var i = 0; i < expected.Chunks.Length; i++)
        AssertChunk(expected.Chunks[i], actual.Chunks[i]);
    }
  }

  private static void AssertChunk(Chunk expected, Chunk actual)
  {
    // Attributes
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);

    // Data
    Assert.Equal(expected.GlobalTileIDs, actual.GlobalTileIDs);
    Assert.Equal(expected.FlippingFlags, actual.FlippingFlags);
  }
}
