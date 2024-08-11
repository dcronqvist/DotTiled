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
      throw new NotSupportedException("External templates are not supported in this test.");
    }
    static Tileset ResolveTileset(string source)
    {
      throw new NotSupportedException("External tilesets are not supported in this test.");
    }
    using var mapReader = new TmjMapReader(json, ResolveTileset, ResolveTemplate, []);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap, map);
  }

  public static IEnumerable<object[]> DeserializeMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data =>
  [
    ["Serialization.TestData.Map.map-with-object-template.tmj", TestData.MapWithObjectTemplate("tj")],
    ["Serialization.TestData.Map.map-with-group.tmj", TestData.MapWithGroup()],
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data))]
  public void TmxMapReaderReadMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    CustomTypeDefinition[] customTypeDefinitions = [
      new CustomClassDefinition
      {
        Name = "TestClass",
        ID = 1,
        UseAs = CustomClassUseAs.Property,
        Members = [
          new StringProperty
          {
            Name = "Name",
            Value = ""
          },
          new FloatProperty
          {
            Name = "Amount",
            Value = 0f
          }
        ]
      },
      new CustomClassDefinition
      {
        Name = "Test",
        ID = 2,
        UseAs = CustomClassUseAs.All,
        Members = [
          new ClassProperty
          {
            Name = "Yep",
            PropertyType = "TestClass",
            Properties = []
          }
        ]
      }
    ];

    var json = TestData.GetRawStringFor(testDataFile);
    Template ResolveTemplate(string source)
    {
      var templateJson = TestData.GetRawStringFor($"Serialization.TestData.Template.{source}");
      using var templateReader = new TjTemplateReader(templateJson, ResolveTileset, ResolveTemplate, customTypeDefinitions);
      return templateReader.ReadTemplate();
    }
    Tileset ResolveTileset(string source)
    {
      var tilesetJson = TestData.GetRawStringFor($"Serialization.TestData.Tileset.{source}");
      using var tilesetReader = new TsjTilesetReader(tilesetJson, ResolveTemplate, customTypeDefinitions);
      return tilesetReader.ReadTileset();
    }
    using var mapReader = new TmjMapReader(json, ResolveTileset, ResolveTemplate, customTypeDefinitions);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap, map);
  }
}
