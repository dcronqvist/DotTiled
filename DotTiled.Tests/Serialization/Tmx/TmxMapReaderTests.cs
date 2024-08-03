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
    ["Serialization.Tmx.TestData.Map.empty-map-csv.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Csv, null)],
    ["Serialization.Tmx.TestData.Map.empty-map-base64.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Base64, null)],
    ["Serialization.Tmx.TestData.Map.empty-map-base64-gzip.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.GZip)],
    ["Serialization.Tmx.TestData.Map.empty-map-base64-zlib.tmx", EmptyMapWithEncodingAndCompression(DataEncoding.Base64, DataCompression.ZLib)],
    ["Serialization.Tmx.TestData.Map.simple-tileset-embed.tmx", SimpleMapWithEmbeddedTileset()],
    ["Serialization.Tmx.TestData.Map.empty-map-properties.tmx", EmptyMapWithProperties()],
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidXmlNoExternalTilesets_ReturnsMapWithoutThrowing_Data))]
  public void TmxMapReaderReadMap_ValidXmlNoExternalTilesets_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    using var reader = TmxMapReaderTestData.GetXmlReaderFor(testDataFile);
    static Template ResolveTemplate(string source)
    {
      using var xmlTemplateReader = TmxMapReaderTestData.GetXmlReaderFor($"Serialization.Tmx.TestData.Template.{source}");
      using var templateReader = new TxTemplateReader(xmlTemplateReader, ResolveTileset, ResolveTemplate);
      return templateReader.ReadTemplate();
    }
    static Tileset ResolveTileset(string source)
    {
      using var xmlTilesetReader = TmxMapReaderTestData.GetXmlReaderFor($"Serialization.Tmx.TestData.Tileset.{source}");
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
    ["Serialization.Tmx.TestData.Map.map-with-object-template.tmx", MapWithObjectTemplate()],
    ["Serialization.Tmx.TestData.Map.map-with-group.tmx", MapWithGroup()],
  ];

  [Theory]
  [MemberData(nameof(DeserializeMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected_Data))]
  public void TmxMapReaderReadMap_ValidXmlExternalTilesetsAndTemplates_ReturnsMapThatEqualsExpected(string testDataFile, Map expectedMap)
  {
    // Arrange
    using var reader = TmxMapReaderTestData.GetXmlReaderFor(testDataFile);
    static Template ResolveTemplate(string source)
    {
      using var xmlTemplateReader = TmxMapReaderTestData.GetXmlReaderFor($"Serialization.Tmx.TestData.Template.{source}");
      using var templateReader = new TxTemplateReader(xmlTemplateReader, ResolveTileset, ResolveTemplate);
      return templateReader.ReadTemplate();
    }
    static Tileset ResolveTileset(string source)
    {
      using var xmlTilesetReader = TmxMapReaderTestData.GetXmlReaderFor($"Serialization.Tmx.TestData.Tileset.{source}");
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
