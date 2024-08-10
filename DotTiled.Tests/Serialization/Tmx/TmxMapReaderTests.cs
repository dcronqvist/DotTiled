using System.Xml;

namespace DotTiled.Tests;

public partial class TmxMapReaderTests
{
  [Fact]
  public void TmxMapReaderConstructor_XmlReaderIsNull_ThrowsArgumentNullException()
  {
    // Arrange
    XmlReader xmlReader = null!;
    Func<string, Tileset> externalTilesetResolver = (_) => new Tileset();
    Func<string, Template> externalTemplateResolver = (_) => new Template { Object = new RectangleObject { } };

    // Act
    Action act = () =>
    {
      using var _ = new TmxMapReader(xmlReader, externalTilesetResolver, externalTemplateResolver);
    };

    // Assert
    Assert.Throws<ArgumentNullException>(act);
  }

  [Fact]
  public void TmxMapReaderConstructor_ExternalTilesetResolverIsNull_ThrowsArgumentNullException()
  {
    // Arrange
    using var stringReader = new StringReader("<map></map>");
    using var xmlReader = XmlReader.Create(stringReader);
    Func<string, Tileset> externalTilesetResolver = null!;
    Func<string, Template> externalTemplateResolver = (_) => new Template { Object = new RectangleObject { } };

    // Act
    Action act = () =>
    {
      using var _ = new TmxMapReader(xmlReader, externalTilesetResolver, externalTemplateResolver);
    };

    // Assert
    Assert.Throws<ArgumentNullException>(act);
  }

  [Fact]
  public void TmxMapReaderConstructor_ExternalTemplateResolverIsNull_ThrowsArgumentNullException()
  {
    // Arrange
    using var stringReader = new StringReader("<map></map>");
    using var xmlReader = XmlReader.Create(stringReader);
    Func<string, Tileset> externalTilesetResolver = (_) => new Tileset();
    Func<string, Template> externalTemplateResolver = null!;

    // Act
    Action act = () =>
    {
      using var _ = new TmxMapReader(xmlReader, externalTilesetResolver, externalTemplateResolver);
    };

    // Assert
    Assert.Throws<ArgumentNullException>(act);
  }

  [Fact]
  public void TmxMapReaderConstructor_NoneNull_DoesNotThrow()
  {
    // Arrange
    using var stringReader = new StringReader("<map></map>");
    using var xmlReader = XmlReader.Create(stringReader);
    Func<string, Tileset> externalTilesetResolver = (_) => new Tileset();
    Func<string, Template> externalTemplateResolver = (_) => new Template { Object = new RectangleObject { } };

    // Act
    using var tmxMapReader = new TmxMapReader(xmlReader, externalTilesetResolver, externalTemplateResolver);

    // Assert
    Assert.NotNull(tmxMapReader);
  }

  public static IEnumerable<object[]> DeserializeMap_ValidXmlNoExternalTilesets_ReturnsMapWithoutThrowing_Data =>
  [
    ["Serialization.TestData.Map.empty-map-csv.tmx", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Csv, null)],
    ["Serialization.TestData.Map.empty-map-base64.tmx", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Base64, null)],
    ["Serialization.TestData.Map.empty-map-base64-gzip.tmx", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.GZip)],
    ["Serialization.TestData.Map.empty-map-base64-zlib.tmx", TestData.EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.ZLib)],
    ["Serialization.TestData.Map.simple-tileset-embed.tmx", TestData.SimpleMapWithEmbeddedTileset()],
    ["Serialization.TestData.Map.empty-map-properties.tmx", TestData.EmptyMapWithProperties()],
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidXmlNoExternalTilesets_ReturnsMapWithoutThrowing_Data))]
  public void TmxMapReaderReadMap_ValidXmlNoExternalTilesets_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    using var reader = TestData.GetXmlReaderFor(testDataFile);
    static Template ResolveTemplate(string source)
    {
      using var xmlTemplateReader = TestData.GetXmlReaderFor($"Serialization.TestData.Template.{source}");
      using var templateReader = new TxTemplateReader(xmlTemplateReader, ResolveTileset, ResolveTemplate);
      return templateReader.ReadTemplate();
    }
    static Tileset ResolveTileset(string source)
    {
      using var xmlTilesetReader = TestData.GetXmlReaderFor($"Serialization.TestData.Tileset.{source}");
      using var tilesetReader = new TsxTilesetReader(xmlTilesetReader, ResolveTemplate);
      return tilesetReader.ReadTileset();
    }
    using var mapReader = new TmxMapReader(reader, ResolveTileset, ResolveTemplate);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap, map);
  }

  public static IEnumerable<object[]> DeserializeMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data =>
  [
    ["Serialization.TestData.Map.map-with-object-template.tmx", TestData.MapWithObjectTemplate("tx")],
    ["Serialization.TestData.Map.map-with-group.tmx", TestData.MapWithGroup()],
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data))]
  public void TmxMapReaderReadMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    using var reader = TestData.GetXmlReaderFor(testDataFile);
    static Template ResolveTemplate(string source)
    {
      using var xmlTemplateReader = TestData.GetXmlReaderFor($"Serialization.TestData.Template.{source}");
      using var templateReader = new TxTemplateReader(xmlTemplateReader, ResolveTileset, ResolveTemplate);
      return templateReader.ReadTemplate();
    }
    static Tileset ResolveTileset(string source)
    {
      using var xmlTilesetReader = TestData.GetXmlReaderFor($"Serialization.TestData.Tileset.{source}");
      using var tilesetReader = new TsxTilesetReader(xmlTilesetReader, ResolveTemplate);
      return tilesetReader.ReadTileset();
    }
    using var mapReader = new TmxMapReader(reader, ResolveTileset, ResolveTemplate);

    // Act
    var map = mapReader.ReadMap();

    // Assert
    Assert.NotNull(map);
    DotTiledAssert.AssertMap(expectedMap, map);
  }
}
