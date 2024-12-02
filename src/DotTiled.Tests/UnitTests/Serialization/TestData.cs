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

  private static string GetMapPath(string mapName) => $"TestData/Maps/{mapName.Replace('-', '_')}/{mapName}";

  public static IEnumerable<object[]> MapTests =>
  [
    [GetMapPath("default-map"), (string f) => DefaultMap(), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-common-props"), (string f) => MapWithCommonProps(), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-custom-type-props"), (string f) => MapWithCustomTypeProps(), MapWithCustomTypePropsCustomTypeDefinitions()],
    [GetMapPath("map-with-custom-type-props-without-defs"), (string f) => MapWithCustomTypePropsWithoutDefs(), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-embedded-tileset"), (string f) => MapWithEmbeddedTileset(), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-external-tileset"), (string f) => MapWithExternalTileset(f), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-flippingflags"), (string f) => MapWithFlippingFlags(f), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-external-tileset-multi"), (string f) => MapExternalTilesetMulti(f), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-external-tileset-wangset"), (string f) => MapExternalTilesetWangset(f), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-many-layers"), (string f) => MapWithManyLayers(f), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-multiline-string-prop"), (string f) => MapWithMultilineStringProp(), Array.Empty<ICustomTypeDefinition>()],
    [GetMapPath("map-with-deep-props"), (string f) => MapWithDeepProps(), MapWithDeepPropsCustomTypeDefinitions()],
    [GetMapPath("map-with-class"), (string f) => MapWithClass(), MapWithClassCustomTypeDefinitions()],
    [GetMapPath("map-with-class-and-props"), (string f) => MapWithClassAndProps(), MapWithClassAndPropsCustomTypeDefinitions()],
    [GetMapPath("map-override-object-bug"), (string f) => MapOverrideObjectBug(f), MapOverrideObjectBugCustomTypeDefinitions()],
  ];
}
