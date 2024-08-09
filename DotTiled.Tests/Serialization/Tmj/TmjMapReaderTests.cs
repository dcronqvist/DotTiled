namespace DotTiled.Tests;

public partial class TmjMapReaderTests
{
  public static IEnumerable<object[]> DeserializeMap_ValidTmjNoExternalTilesets_ReturnsMapWithoutThrowing_Data =>
  [
    ["Serialization.TestData.Map.empty-map-csv.tmj", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Csv, null)],
    ["Serialization.TestData.Map.empty-map-base64.tmj", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Base64, null)],
    ["Serialization.TestData.Map.empty-map-base64-gzip.tmj", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.GZip)],
    ["Serialization.TestData.Map.empty-map-base64-zlib.tmj", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.ZLib)],
    ["Serialization.TestData.Map.simple-tileset-embed.tmj", TestData.SimpleMapWithEmbeddedTileset()],
    ["Serialization.TestData.Map.empty-map-properties.tmj", TestData.EmptyMapWithProperties()],
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidTmjNoExternalTilesets_ReturnsMapWithoutThrowing_Data))]
  public void TmxMapReaderReadMap_ValidTmjNoExternalTilesets_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    var json = TestData.GetRawStringFor(testDataFile);
    static Template ResolveTemplate(string source)
    {
      var templateJson = TestData.GetRawStringFor($"Serialization.TestData.Template.{source}");
      //var templateReader = new TmjTemplateReader(templateJson, ResolveTemplate);
      return null;
    }
    static Tileset ResolveTileset(string source)
    {
      var tilesetJson = TestData.GetXmlReaderFor($"Serialization.TestData.Tileset.{source}");
      //var tilesetReader = new TmjTilesetReader(tilesetJson, ResolveTileset, ResolveTemplate);
      return null;
    }
    using var mapReader = new TmjMapReader(json, ResolveTileset, ResolveTemplate);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap, map);
  }

  public static IEnumerable<object[]> DeserializeMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data =>
  [
    ["Serialization.TestData.Map.map-with-object-template.tmj", TestData.MapWithObjectTemplate()],
    ["Serialization.TestData.Map.map-with-group.tmj", TestData.MapWithGroup()],
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data))]
  public void TmxMapReaderReadMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    var json = TestData.GetRawStringFor(testDataFile);
    static Template ResolveTemplate(string source)
    {
      var templateJson = TestData.GetRawStringFor($"Serialization.TestData.Template.{source}");
      //var templateReader = new TmjTemplateReader(templateJson, ResolveTemplate);
      return null;
    }
    static Tileset ResolveTileset(string source)
    {
      var tilesetJson = TestData.GetXmlReaderFor($"Serialization.TestData.Tileset.{source}");
      //var tilesetReader = new TmjTilesetReader(tilesetJson, ResolveTileset, ResolveTemplate);
      return null;
    }
    using var mapReader = new TmjMapReader(json, ResolveTileset, ResolveTemplate);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap, map);
  }
}
