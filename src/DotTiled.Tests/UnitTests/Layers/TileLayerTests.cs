namespace DotTiled.Tests.UnitTests;

public class TileLayerTests
{
  [Fact]
  public void GetGlobalTileIDAtCoord_NoDataInLayer_Throws()
  {
    // Arrange
    var tileLayer = new TileLayer
    {
      ID = 1,
      Width = 10,
      Height = 10
    };

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => tileLayer.GetGlobalTileIDAtCoord(0, 0));
  }

  [Fact]
  public void GetGlobalTileIDAtCoord_LayerHasChunksCoordinateNotInAnyChunk_ReturnsZero()
  {
    // Arrange
    var tileLayer = new TileLayer
    {
      ID = 1,
      Width = 10,
      Height = 10,
      Data = new Data
      {
        Chunks = new Optional<Chunk[]>([
          new Chunk
          {
            X = 0,
            Y = 0,
            Width = 5,
            Height = 5,
            GlobalTileIDs = [1, 2, 3, 4, 5,
                             6, 7, 8, 9, 10,
                             11,12,13,14,15,
                             16,17,18,19,20,
                             21,22,23,24,25],
            FlippingFlags = GenerateFlippingFlagsArray(5, 5, (x, y) => FlippingFlags.None)
          }
        ])
      }
    };

    // Act
    var globalTileID = tileLayer.GetGlobalTileIDAtCoord(6, 6);

    // Assert
    Assert.Equal(0u, globalTileID);
  }

  [Fact]
  public void GetGlobalTileIDAtCoord_LayerHasChunksCoordinateInChunk_ReturnsCorrectTileID()
  {
    // Arrange
    var tileLayer = new TileLayer
    {
      ID = 1,
      Width = 10,
      Height = 10,
      Data = new Data
      {
        Chunks = new Optional<Chunk[]>([
          new Chunk
          {
            X = 0,
            Y = 0,
            Width = 5,
            Height = 5,
            GlobalTileIDs = [1, 2, 3, 4, 5,
                             6, 7, 8, 9, 10,
                             11,12,13,14,15,
                             16,17,18,19,20,
                             21,22,23,24,25],
            FlippingFlags = GenerateFlippingFlagsArray(5, 5, (x, y) => FlippingFlags.None)
          }
        ])
      }
    };

    // Act
    var globalTileID = tileLayer.GetGlobalTileIDAtCoord(2, 2);

    // Assert
    Assert.Equal(13u, globalTileID);
  }

  [Fact]
  public void GetGlobalTileIDAtCoord_NoChunksCoordinateOutOfBounds_Throws()
  {
    // Arrange
    var tileLayer = new TileLayer
    {
      ID = 1,
      Width = 10,
      Height = 10,
      Data = new Data
      {
        GlobalTileIDs = new Optional<uint[]>(Enumerable.Range(1, 100).Select(i => (uint)i).ToArray())
      }
    };

    // Act & Assert
    Assert.Throws<ArgumentException>(() => tileLayer.GetGlobalTileIDAtCoord(10, 10));
  }

  [Fact]
  public void GetGlobalTileIDAtCoord_NoChunksValidCoordinate_ReturnsCorrectTileID()
  {
    // Arrange
    var tileLayer = new TileLayer
    {
      ID = 1,
      Width = 10,
      Height = 10,
      Data = new Data
      {
        GlobalTileIDs = new Optional<uint[]>(Enumerable.Range(1, 100).Select(i => (uint)i).ToArray())
      }
    };

    // Act
    var globalTileID = tileLayer.GetGlobalTileIDAtCoord(3, 4);

    // Assert
    Assert.Equal(44u, globalTileID);
  }

  [Fact]
  public void GetGlobalTileIDAtCoord_NoChunksMissingGlobalTileIDs_Throws()
  {
    // Arrange
    var tileLayer = new TileLayer
    {
      ID = 1,
      Width = 10,
      Height = 10,
      Data = new Data()
    };

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => tileLayer.GetGlobalTileIDAtCoord(0, 0));
  }

  private static FlippingFlags[] GenerateFlippingFlagsArray(int width, int height, Func<int, int, FlippingFlags> flagGenerator)
  {
    var flags = new FlippingFlags[width * height];
    for (var y = 0; y < height; y++)
    {
      for (var x = 0; x < width; x++)
      {
        flags[(y * width) + x] = flagGenerator(x, y);
      }
    }
    return flags;
  }
}
