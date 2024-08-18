using DotTiled.Model;
using DotTiled.Model;
using DotTiled.Model;
using DotTiled.Serialization.Tmj;

namespace DotTiled.Tests;

public partial class TmjMapReaderTests
{
  public static IEnumerable<object[]> Maps => TestData.MapTests;
  [Theory]
  [MemberData(nameof(Maps))]
  public void TmxMapReaderReadMap_ValidTmjExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(
    string testDataFile,
    Func<string, Map> expectedMap,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Arrange
    testDataFile += ".tmj";
    var fileDir = Path.GetDirectoryName(testDataFile);
    var json = TestData.GetRawStringFor(testDataFile);
    Template ResolveTemplate(string source)
    {
      var templateJson = TestData.GetRawStringFor($"{fileDir}/{source}");
      using var templateReader = new TjTemplateReader(templateJson, ResolveTileset, ResolveTemplate, customTypeDefinitions);
      return templateReader.ReadTemplate();
    }
    Tileset ResolveTileset(string source)
    {
      var tilesetJson = TestData.GetRawStringFor($"{fileDir}/{source}");
      using var tilesetReader = new TsjTilesetReader(tilesetJson, ResolveTemplate, customTypeDefinitions);
      return tilesetReader.ReadTileset();
    }
    using var mapReader = new TmjMapReader(json, ResolveTileset, ResolveTemplate, customTypeDefinitions);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap("tmj"), map);
  }
}
