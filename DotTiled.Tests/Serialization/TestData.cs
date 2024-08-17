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
    ["Serialization/TestData/Map/default_map/default-map", (string f) => TestData.DefaultMap(), Array.Empty<CustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_common_props/map-with-common-props", (string f) => TestData.MapWithCommonProps(), Array.Empty<CustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_custom_type_props/map-with-custom-type-props", (string f) => TestData.MapWithCustomTypeProps(), TestData.MapWithCustomTypePropsCustomTypeDefinitions()],
    ["Serialization/TestData/Map/map_with_embedded_tileset/map-with-embedded-tileset", (string f) => TestData.MapWithEmbeddedTileset(), Array.Empty<CustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_external_tileset/map-with-external-tileset", (string f) => TestData.MapWithExternalTileset(f), Array.Empty<CustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_flippingflags/map-with-flippingflags", (string f) => TestData.MapWithFlippingFlags(f), Array.Empty<CustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_external_tileset_multi/map-external-tileset-multi", (string f) => TestData.MapExternalTilesetMulti(f), Array.Empty<CustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_external_tileset_wangset/map-external-tileset-wangset", (string f) => TestData.MapExternalTilesetWangset(f), Array.Empty<CustomTypeDefinition>()],
    ["Serialization/TestData/Map/map_with_many_layers/map-with-many-layers", (string f) => TestData.MapWithManyLayers(f), Array.Empty<CustomTypeDefinition>()],
  ];

  private static CustomTypeDefinition[] typedefs = [
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
}
