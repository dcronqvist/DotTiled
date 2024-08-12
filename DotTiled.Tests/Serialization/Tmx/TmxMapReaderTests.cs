using System.Xml;

namespace DotTiled.Tests;

public partial class TmxMapReaderTests
{
  public static IEnumerable<object[]> Maps => TestData.MapTests;
  [Theory]
  [MemberData(nameof(Maps))]
  public void TmxMapReaderReadMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(
    string testDataFile,
    Map expectedMap,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Arrange
    testDataFile += ".tmx";
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
