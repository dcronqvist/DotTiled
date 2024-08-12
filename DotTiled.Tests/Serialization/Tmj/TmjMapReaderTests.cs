namespace DotTiled.Tests;

public partial class TmjMapReaderTests
{
  public static IEnumerable<object[]> Maps => TestData.MapsThatHaveTmxAndTmj;
  [Theory]
  [MemberData(nameof(Maps))]
  public void TmxMapReaderReadMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(
    string testDataFile,
    Map expectedMap,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Arrange
    testDataFile += ".tmj";
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
