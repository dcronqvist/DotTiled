using DotTiled.Serialization.Tmx;

namespace DotTiled.Tests;

public partial class TmxMapReaderTests
{
  public static IEnumerable<object[]> Maps => TestData.MapTests;
  [Theory]
  [MemberData(nameof(Maps))]
  public void TmxMapReaderReadMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(
    string testDataFile,
    Func<string, Map> expectedMap,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    // Arrange
    testDataFile += ".tmx";
    var fileDir = Path.GetDirectoryName(testDataFile);
    using var reader = TestData.GetXmlReaderFor(testDataFile);
    Template ResolveTemplate(string source)
    {
      using var xmlTemplateReader = TestData.GetXmlReaderFor($"{fileDir}/{source}");
      using var templateReader = new TxTemplateReader(xmlTemplateReader, ResolveTileset, ResolveTemplate, ResolveCustomType);
      return templateReader.ReadTemplate();
    }
    Tileset ResolveTileset(string source)
    {
      using var xmlTilesetReader = TestData.GetXmlReaderFor($"{fileDir}/{source}");
      using var tilesetReader = new TsxTilesetReader(xmlTilesetReader, ResolveTileset, ResolveTemplate, ResolveCustomType);
      return tilesetReader.ReadTileset();
    }
    Optional<ICustomTypeDefinition> ResolveCustomType(string name)
    {
      if (customTypeDefinitions.FirstOrDefault(ctd => ctd.Name == name) is ICustomTypeDefinition ctd)
      {
        return new Optional<ICustomTypeDefinition>(ctd);
      }

      return Optional.Empty;
    }
    using var mapReader = new TmxMapReader(reader, ResolveTileset, ResolveTemplate, ResolveCustomType);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap("tmx"), map);
  }
}
