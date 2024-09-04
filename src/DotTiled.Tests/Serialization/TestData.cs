using System.Xml;

namespace DotTiled.Tests;

public static partial class TestData
{
  public static XmlReader GetXmlReaderFor(string testDataFile)
  {
    var fullyQualifiedTestDataFile = $"DotTiled.Tests.{ConvertPathToAssemblyResourcePath(testDataFile)}";
    using var stream = typeof(TestData).Assembly.GetManifestResourceStream(fullyQualifiedTestDataFile)
                       ?? throw new ArgumentException($"Test data file '{fullyQualifiedTestDataFile}' not found");

    using var stringReader = new StreamReader(stream);
    var xml = stringReader.ReadToEnd();
    var xmlStringReader = new StringReader(xml);
    return XmlReader.Create(xmlStringReader);
  }

  public static string GetRawStringFor(string testDataFile)
  {
    var fullyQualifiedTestDataFile = $"DotTiled.Tests.{ConvertPathToAssemblyResourcePath(testDataFile)}";
    using var stream = typeof(TestData).Assembly.GetManifestResourceStream(fullyQualifiedTestDataFile)
                       ?? throw new ArgumentException($"Test data file '{fullyQualifiedTestDataFile}' not found");

    using var stringReader = new StreamReader(stream);
    return stringReader.ReadToEnd();
  }

  private static string ConvertPathToAssemblyResourcePath(string path) =>
    path.Replace("/", ".").Replace("\\", ".").Replace(" ", "_");

  public static IEnumerable<object[]> MapTests =>
  [
    ["Serialization/TestData/Map/default_map/default-map", (string f) => DefaultMap(), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_common_props/map-with-common-props", (string f) => MapWithCommonProps(), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_custom_type_props/map-with-custom-type-props", (string f) => MapWithCustomTypeProps(), MapWithCustomTypePropsCustomTypeDefinitions()],
    ["Serialization/TestData/Map/map_with_embedded_tileset/map-with-embedded-tileset", (string f) => MapWithEmbeddedTileset(), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_external_tileset/map-with-external-tileset", (string f) => MapWithExternalTileset(f), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_flippingflags/map-with-flippingflags", (string f) => MapWithFlippingFlags(f), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_external_tileset_multi/map-external-tileset-multi", (string f) => MapExternalTilesetMulti(f), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_external_tileset_wangset/map-external-tileset-wangset", (string f) => MapExternalTilesetWangset(f), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_many_layers/map-with-many-layers", (string f) => MapWithManyLayers(f), Array.Empty<ICustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_deep_props/map-with-deep-props", (string f) => MapWithDeepProps(), MapWithDeepPropsCustomTypeDefinitions()],
    ["Serialization/TestData/Map/map_with_class/map-with-class", (string f) => MapWithClass(), MapWithClassCustomTypeDefinitions()],
  ];
}
