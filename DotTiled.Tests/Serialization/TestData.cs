using System.Xml;

namespace DotTiled.Tests;

public static partial class TestData
{
  public static XmlReader GetXmlReaderFor(string testDataFile)
  {
    var names = typeof(TestData).Assembly.GetManifestResourceNames();

    var fullyQualifiedTestDataFile = $"DotTiled.Tests.{testDataFile}";
    using var stream = typeof(TestData).Assembly.GetManifestResourceStream(fullyQualifiedTestDataFile)
                       ?? throw new ArgumentException($"Test data file '{fullyQualifiedTestDataFile}' not found");

    using var stringReader = new StreamReader(stream);
    var xml = stringReader.ReadToEnd();
    var xmlStringReader = new StringReader(xml);
    return XmlReader.Create(xmlStringReader);
  }

  public static string GetRawStringFor(string testDataFile)
  {
    var fullyQualifiedTestDataFile = $"DotTiled.Tests.{testDataFile}";
    using var stream = typeof(TestData).Assembly.GetManifestResourceStream(fullyQualifiedTestDataFile)
                       ?? throw new ArgumentException($"Test data file '{fullyQualifiedTestDataFile}' not found");

    using var stringReader = new StreamReader(stream);
    return stringReader.ReadToEnd();
  }

  public static IEnumerable<object[]> MapsThatHaveTmxAndTmj =>
  [
    ["Serialization.TestData.Map.default_map.default-map", TestData.DefaultMap(), Array.Empty<CustomTypeDefinition>()]
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
