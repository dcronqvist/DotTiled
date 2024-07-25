using System.Xml;

namespace DotTiled.Tests;

public static class TmxSerializerTestData
{
  public static XmlReader GetReaderFor(string testDataFile)
  {
    var fullyQualifiedTestDataFile = $"DotTiled.Tests.{testDataFile}";
    using var stream = typeof(TmxSerializerTestData).Assembly.GetManifestResourceStream(fullyQualifiedTestDataFile)
                       ?? throw new ArgumentException($"Test data file '{fullyQualifiedTestDataFile}' not found");

    using var stringReader = new StreamReader(stream);
    var xml = stringReader.ReadToEnd();
    var xmlStringReader = new StringReader(xml);
    return XmlReader.Create(xmlStringReader);
  }
}
