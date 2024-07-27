namespace DotTiled.Tests;

public partial class TmxSerializerMapTests
{
  private static void AssertMap(Map actual, Map expected)
  {
    // Attributes
    Assert.Equal(expected.Version, actual.Version);
    Assert.Equal(expected.TiledVersion, actual.TiledVersion);
    Assert.Equal(expected.Class, actual.Class);
    Assert.Equal(expected.Orientation, actual.Orientation);
    Assert.Equal(expected.RenderOrder, actual.RenderOrder);
    Assert.Equal(expected.CompressionLevel, actual.CompressionLevel);
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);
    Assert.Equal(expected.TileWidth, actual.TileWidth);
    Assert.Equal(expected.TileHeight, actual.TileHeight);
    Assert.Equal(expected.HexSideLength, actual.HexSideLength);
    Assert.Equal(expected.StaggerAxis, actual.StaggerAxis);
    Assert.Equal(expected.StaggerIndex, actual.StaggerIndex);
    Assert.Equal(expected.ParallaxOriginX, actual.ParallaxOriginX);
    Assert.Equal(expected.ParallaxOriginY, actual.ParallaxOriginY);
    Assert.Equal(expected.BackgroundColor, actual.BackgroundColor);
    Assert.Equal(expected.NextLayerID, actual.NextLayerID);
    Assert.Equal(expected.NextObjectID, actual.NextObjectID);
    Assert.Equal(expected.Infinite, actual.Infinite);

    TmxSerializerPropertiesTests.AssertProperties(actual.Properties, expected.Properties);

    Assert.NotNull(actual.Tilesets);
    Assert.Equal(expected.Tilesets.Count, actual.Tilesets.Count);
    for (var i = 0; i < expected.Tilesets.Count; i++)
      TmxSerializerTilesetTests.AssertTileset(actual.Tilesets[i], expected.Tilesets[i]);

    Assert.NotNull(actual.Layers);
    Assert.Equal(expected.Layers.Count, actual.Layers.Count);
    for (var i = 0; i < expected.Layers.Count; i++)
      TmxSerializerLayerTests.AssertLayer(actual.Layers[i], expected.Layers[i]);
  }

  public static IEnumerable<object[]> DeserializeMap_ValidXmlNoExternalTilesets_ReturnsMapWithoutThrowing_Data =>
    [
      ["TmxSerializer.TestData.Map.empty-map-csv.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Csv, null)],
      ["TmxSerializer.TestData.Map.empty-map-base64.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Base64, null)],
      ["TmxSerializer.TestData.Map.empty-map-base64-gzip.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.GZip)],
      ["TmxSerializer.TestData.Map.empty-map-base64-zlib.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.ZLib)],
      ["TmxSerializer.TestData.Map.simple-tileset-embed.tmx", SimpleMapWithEmbeddedTileset()],
      ["TmxSerializer.TestData.Map.empty-map-properties.tmx", EmptyMapWithProperties()],
    ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidXmlNoExternalTilesets_ReturnsMapWithoutThrowing_Data))]
  public void DeserializeMap_ValidXmlNoExternalTilesets_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    using var reader = TmxSerializerTestData.GetReaderFor(testDataFile);
    var testDataFileText = TmxSerializerTestData.GetRawStringFor(testDataFile);
    Func<string, Tileset> externalTilesetResolver = (string s) => throw new NotSupportedException("External tilesets are not supported in this test");
    var tmxSerializer = new TmxSerializer(externalTilesetResolver);

    // Act
    var map = tmxSerializer.DeserializeMap(reader);
    var raw = tmxSerializer.DeserializeMap(testDataFileText);

    // Assert
    Assert.NotNull(map);
    AssertMap(map, expectedMap);

    Assert.NotNull(raw);
    AssertMap(raw, expectedMap);
  }
}
