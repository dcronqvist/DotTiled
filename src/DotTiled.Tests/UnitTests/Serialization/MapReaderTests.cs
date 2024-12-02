using DotTiled.Serialization;

namespace DotTiled.Tests;

public partial class MapReaderTests
{
  public static IEnumerable<object[]> Maps => TestData.MapTests;
  [Theory]
  [MemberData(nameof(Maps))]
  public void MapReaderReadMap_ValidFilesExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(
    string testDataFile,
    Func<string, Map> expectedMap,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    // Arrange
    string[] fileFormats = [".tmx", ".tmj"];

    foreach (var fileFormat in fileFormats)
    {
      var testDataFileWithFormat = testDataFile + fileFormat;
      var fileDir = Path.GetDirectoryName(testDataFileWithFormat);
      var mapString = TestData.GetRawStringFor(testDataFileWithFormat);
      Template ResolveTemplate(string source)
      {
        var templateString = TestData.GetRawStringFor($"{fileDir}/{source}");
        using var templateReader = new TemplateReader(templateString, ResolveTileset, ResolveTemplate, ResolveCustomType);
        return templateReader.ReadTemplate();
      }
      Tileset ResolveTileset(string source)
      {
        var tilesetString = TestData.GetRawStringFor($"{fileDir}/{source}");
        using var tilesetReader = new TilesetReader(tilesetString, ResolveTileset, ResolveTemplate, ResolveCustomType);
        return tilesetReader.ReadTileset();
      }
      Optional<ICustomTypeDefinition> ResolveCustomType(string name)
      {
        if (customTypeDefinitions.FirstOrDefault(ctd => ctd.Name == name) is ICustomTypeDefinition ctd)
        {
          return new Optional<ICustomTypeDefinition>(ctd);
        }

        return Optional<ICustomTypeDefinition>.Empty;
      }
      using var mapReader = new MapReader(mapString, ResolveTileset, ResolveTemplate, ResolveCustomType);

      // Act
      var map = mapReader.ReadMap();

      // Assert
      Assert.NotNull(map);
      DotTiledAssert.AssertMap(expectedMap(fileFormat[1..]), map);
    }
  }
}
