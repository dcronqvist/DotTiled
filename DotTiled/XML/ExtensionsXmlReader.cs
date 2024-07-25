using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace DotTiled;

internal static class ExtensionsXmlReader
{
  internal static string GetRequiredAttribute(this XmlReader reader, string attribute)
  {
    return reader.GetAttribute(attribute) ?? throw new XmlException($"{attribute} attribute is required"); ;
  }

  internal static T GetRequiredAttribute<T>(this XmlReader reader, string attribute) where T : IParsable<T>
  {
    var value = reader.GetAttribute(attribute) ?? throw new XmlException($"{attribute} attribute is required");
    return T.Parse(value, CultureInfo.InvariantCulture);
  }

  internal static T GetRequiredAttributeEnum<T>(this XmlReader reader, string attribute) where T : Enum
  {
    var value = reader.GetAttribute(attribute) ?? throw new XmlException($"{attribute} attribute is required");
    return ParseEnumUsingXmlEnumAttribute<T>(value);
  }

  internal static string? GetOptionalAttribute(this XmlReader reader, string attribute, string? defaultValue = default)
  {
    return reader.GetAttribute(attribute) ?? defaultValue;
  }

  internal static T? GetOptionalAttribute<T>(this XmlReader reader, string attribute) where T : struct, IParsable<T>
  {
    var value = reader.GetAttribute(attribute);
    if (value is null)
      return null;

    return T.Parse(value, CultureInfo.InvariantCulture);
  }

  internal static T? GetOptionalAttributeClass<T>(this XmlReader reader, string attribute) where T : class, IParsable<T>
  {
    var value = reader.GetAttribute(attribute);
    if (value is null)
      return null;

    return T.Parse(value, CultureInfo.InvariantCulture);
  }

  internal static T? GetOptionalAttributeEnum<T>(this XmlReader reader, string attribute) where T : struct, Enum
  {
    var value = reader.GetAttribute(attribute);
    return value != null ? ParseEnumUsingXmlEnumAttribute<T>(value) : null;
  }

  internal static T ParseEnumUsingXmlEnumAttribute<T>(string value) where T : Enum
  {
    var enumType = typeof(T);
    var enumValues = Enum.GetValues(enumType);
    foreach (var enumValue in enumValues)
    {
      var enumMember = enumType.GetMember(enumValue.ToString()!)[0];
      var xmlEnumAttribute = enumMember.GetCustomAttributes(typeof(XmlEnumAttribute), false).FirstOrDefault() as XmlEnumAttribute;
      if (xmlEnumAttribute?.Name == value)
        return (T)enumValue;
    }

    throw new XmlException($"Failed to parse enum value {value}");
  }

  internal static List<T> ReadList<T>(this XmlReader reader, string wrapper, string elementName, Func<XmlReader, T> readElement)
  {
    var list = new List<T>();

    if (reader.IsEmptyElement)
      return list;

    reader.ReadStartElement(wrapper);
    while (reader.IsStartElement(elementName))
    {
      list.Add(readElement(reader));

      if (reader.NodeType == XmlNodeType.EndElement)
        continue; // At end of list, no need to read again

      reader.Read();
    }
    reader.ReadEndElement();

    return list;
  }

  public static T ReadElementAs<T>(this XmlReader reader) where T : IXmlSerializable
  {
    var serializer = new XmlSerializer(typeof(T));
    return (T)serializer.Deserialize(reader)!;
  }

  public static int CountDirectChildrenWithName(this XmlReader reader, string name)
  {
    var subTree = reader.ReadSubtree();
    int count = 0;
    while (subTree.Read())
    {
      if (subTree.NodeType == XmlNodeType.Element && subTree.Name == name)
        count++;
    }
    return count;
  }
}
