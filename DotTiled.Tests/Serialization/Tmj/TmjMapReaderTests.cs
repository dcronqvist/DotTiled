namespace DotTiled.Tests;

public partial class TmjMapReaderTests
{
  public static IEnumerable<object[]> DeserializeMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data =>
  [
    ["Serialization.TestData.Map.default_map.default-map.tmj", TestData.DefaultMap()]
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
