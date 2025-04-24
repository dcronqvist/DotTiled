namespace DotTiled.Tests.UnitTests;

public class TilesetTests
{
  [Fact]
  public void GetSourceRectangleForLocalTileID_TileIDOutOfRange_ThrowsException()
  {
    // Arrange
    var tileset = new Tileset
    {
      FirstGID = 0,
      Name = "Tileset1",
      TileWidth = 16,
      TileHeight = 16,
      TileCount = 5,
      Columns = 5
    };

    // Act & Assert
    Assert.Throws<ArgumentException>(() => tileset.GetSourceRectangleForLocalTileID(6));
  }

  [Fact]
  public void GetSourceRectangleForLocalTileID_ValidTileIDIsInTilesList_ReturnsCorrectRectangle()
  {
    // Arrange
    var tileset = new Tileset
    {
      FirstGID = 0,
      Name = "Tileset1",
      TileWidth = 16,
      TileHeight = 16,
      TileCount = 2,
      Columns = 2,
      Tiles = [
        new Tile
        {
          ID = 0,
          X = 0,
          Y = 0,
          Width = 16,
          Height = 16,
        },
        new Tile
        {
          ID = 1,
          X = 16,
          Y = 0,
          Width = 16,
          Height = 16,
        }
      ]
    };

    // Act
    var rectangle = tileset.GetSourceRectangleForLocalTileID(1);

    // Assert
    Assert.Equal(16, rectangle.X);
    Assert.Equal(0, rectangle.Y);
    Assert.Equal(16, rectangle.Width);
    Assert.Equal(16, rectangle.Height);
  }

  [Fact]
  public void GetSourceRectangleForLocalTileID_ValidTileIDIsNotInTilesListNoMarginNoSpacing_ReturnsCorrectRectangle()
  {
    // Arrange
    var tileset = new Tileset
    {
      FirstGID = 0,
      Name = "Tileset1",
      TileWidth = 16,
      TileHeight = 16,
      TileCount = 5,
      Columns = 5,
    };

    // Act
    var rectangle = tileset.GetSourceRectangleForLocalTileID(3);

    // Assert
    Assert.Equal(48, rectangle.X);
    Assert.Equal(0, rectangle.Y);
    Assert.Equal(16, rectangle.Width);
    Assert.Equal(16, rectangle.Height);
  }

  [Fact]
  public void GetSourceRectangleForLocalTileID_ValidTileIDIsNotInTilesListWithMarginAndSpacing_ReturnsCorrectRectangle()
  {
    // Arrange
    var tileset = new Tileset
    {
      FirstGID = 0,
      Name = "Tileset1",
      TileWidth = 16,
      TileHeight = 16,
      TileCount = 5,
      Columns = 5,
      Margin = 3,
      Spacing = 1
    };

    // Act
    var rectangle = tileset.GetSourceRectangleForLocalTileID(3);

    // Assert
    Assert.Equal(54, rectangle.X);
    Assert.Equal(3, rectangle.Y);
    Assert.Equal(16, rectangle.Width);
    Assert.Equal(16, rectangle.Height);
  }
}
