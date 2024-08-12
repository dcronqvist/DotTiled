using System.Xml;

namespace DotTiled.Tests;

public partial class TmxMapReaderTests
{
  public static IEnumerable<object[]> DeserializeMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data =>
  [
    ["Serialization.TestData.Map.default_map.default-map.tmx", TestData.DefaultMap()]
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data))]
  public void TmxMapReaderReadMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
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
    using var reader = TestData.GetXmlReaderFor(testDataFile);
    Template ResolveTemplate(string source)
    {
      using var xmlTemplateReader = TestData.GetXmlReaderFor($"Serialization.TestData.Template.{source}");
      using var templateReader = new TxTemplateReader(xmlTemplateReader, ResolveTileset, ResolveTemplate, customTypeDefinitions);
      return templateReader.ReadTemplate();
    }
    Tileset ResolveTileset(string source)
    {
      using var xmlTilesetReader = TestData.GetXmlReaderFor($"Serialization.TestData.Tileset.{source}");
      using var tilesetReader = new TsxTilesetReader(xmlTilesetReader, ResolveTemplate, customTypeDefinitions);
      return tilesetReader.ReadTileset();
    }
    using var mapReader = new TmxMapReader(reader, ResolveTileset, ResolveTemplate, customTypeDefinitions);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap, map);
  }
}
