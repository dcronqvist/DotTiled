namespace DotTiled.Tests;

public partial class TmxSerializerDataTests
{
  public static void AssertData(Data? actual, Data? expected)
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
        AssertChunk(actual.Chunks[i], expected.Chunks[i]);
    }
  }

  private static void AssertChunk(Chunk actual, Chunk expected)
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
