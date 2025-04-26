namespace DotTiled.Tests.UnitTests;

public class MapTests
{
  [Fact]
  public void ResolveTilesetForGlobalTileID_NoTilesets_ThrowsException()
  {
    // Arrange
    var map = new Map
    {
      Version = "version",
      Orientation = MapOrientation.Orthogonal,
      Width = 10,
      Height = 10,
      TileWidth = 16,
      TileHeight = 16,
      NextLayerID = 1,
      NextObjectID = 1
    };

    // Act & Assert
    Assert.Throws<ArgumentException>(() => map.ResolveTilesetForGlobalTileID(1, out var _));
  }

  [Fact]
  public void ResolveTilesetForGlobalTileID_GlobalTileIDOutOfRange_ThrowsException()
  {
    // Arrange
    var map = new Map
    {
      Version = "version",
      Orientation = MapOrientation.Orthogonal,
      Width = 10,
      Height = 10,
      TileWidth = 16,
      TileHeight = 16,
      NextLayerID = 1,
      NextObjectID = 1,
      Tilesets = [
        new Tileset
        {
          FirstGID = 1,
          Name = "Tileset1",
          TileWidth = 16,
          TileHeight = 16,
          TileCount = 5,
          Columns = 5
        }
      ]
    };

    // Act & Assert
    Assert.Throws<ArgumentException>(() => map.ResolveTilesetForGlobalTileID(6, out var _));
  }

  [Fact]
  public void ResolveTilesetForGlobalTileID_GlobalTileIDInRangeOfOnlyTileset_ReturnsTileset()
  {
    // Arrange
    var tileset = new Tileset
    {
      FirstGID = 1,
      Name = "Tileset1",
      TileWidth = 16,
      TileHeight = 16,
      TileCount = 5,
      Columns = 5
    };
    var map = new Map
    {
      Version = "version",
      Orientation = MapOrientation.Orthogonal,
      Width = 10,
      Height = 10,
      TileWidth = 16,
      TileHeight = 16,
      NextLayerID = 1,
      NextObjectID = 1,
      Tilesets = [tileset]
    };

    // Act
    var result = map.ResolveTilesetForGlobalTileID(3, out var localTileID);

    // Assert
    Assert.Equal(tileset, result);
    Assert.Equal(2, (int)localTileID); // 3 - 1 = 2 (local tile ID)
  }

  [Fact]
  public void ResolveTilesetForGlobalTileID_GlobalTileIDInRangeOfMultipleTilesets_ReturnsCorrectTileset()
  {
    // Arrange
    var tileset1 = new Tileset
    {
      FirstGID = 1,
      Name = "Tileset1",
      TileWidth = 16,
      TileHeight = 16,
      TileCount = 5,
      Columns = 5
    };
    var tileset2 = new Tileset
    {
      FirstGID = 6,
      Name = "Tileset2",
      TileWidth = 16,
      TileHeight = 16,
      TileCount = 5,
      Columns = 5
    };
    var map = new Map
    {
      Version = "version",
      Orientation = MapOrientation.Orthogonal,
      Width = 10,
      Height = 10,
      TileWidth = 16,
      TileHeight = 16,
      NextLayerID = 1,
      NextObjectID = 1,
      Tilesets = [tileset1, tileset2]
    };

    // Act
    var result = map.ResolveTilesetForGlobalTileID(8, out var localTileID);

    // Assert
    Assert.Equal(tileset2, result);
    Assert.Equal(2, (int)localTileID); // 8 - 6 = 2 (local tile ID)
  }
}
