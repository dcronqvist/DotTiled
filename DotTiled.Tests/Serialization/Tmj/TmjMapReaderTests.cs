namespace DotTiled.Tests;

public partial class TmjMapReaderTests
{
  [Fact]
  public void Test1()
  {
    // Arrange
    var jsonString =
    """
    {
      "backgroundcolor":"#656667",
      "height":4,
      "nextobjectid":1,
      "nextlayerid":1,
      "orientation":"orthogonal",
      "renderorder":"right-down",
      "tileheight":32,
      "tilewidth":32,
      "version":"1",
      "tiledversion":"1.0.3",
      "width":4
    }
    """;

    // Act
    using var tmjMapReader = new TmjMapReader(jsonString);

    // Assert
    var map = tmjMapReader.ReadMap();
  }
}
