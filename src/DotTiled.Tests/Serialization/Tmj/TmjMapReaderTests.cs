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
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    // Arrange
    testDataFile += ".tmj";
    var fileDir = Path.GetDirectoryName(testDataFile);
    var json = TestData.GetRawStringFor(testDataFile);
    Template ResolveTemplate(string source)
    {
      var templateJson = TestData.GetRawStringFor($"{fileDir}/{source}");
      using var templateReader = new TjTemplateReader(templateJson, ResolveTileset, ResolveTemplate, ResolveCustomType);
      return templateReader.ReadTemplate();
    }
    Tileset ResolveTileset(string source)
    {
      var tilesetJson = TestData.GetRawStringFor($"{fileDir}/{source}");
      using var tilesetReader = new TsjTilesetReader(tilesetJson, ResolveTileset, ResolveTemplate, ResolveCustomType);
      return tilesetReader.ReadTileset();
    }
    ICustomTypeDefinition ResolveCustomType(string name)
    {
      return customTypeDefinitions.FirstOrDefault(ctd => ctd.Name == name)!;
    }
    using var mapReader = new TmjMapReader(json, ResolveTileset, ResolveTemplate, ResolveCustomType);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap("tmj"), map);
  }
}
