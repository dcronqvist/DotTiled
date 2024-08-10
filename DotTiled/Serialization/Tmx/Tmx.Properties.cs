using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DotTiled;

internal partial class Tmx
{
  internal static Dictionary<string, IProperty> ReadProperties(
    XmlReader reader,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    return reader.ReadList("properties", "property", (r) =>
    {
      var name = r.GetRequiredAttribute("name");
      var type = r.GetOptionalAttributeEnum<PropertyType>("type", (s) => s switch
      {
        "string" => PropertyType.String,
        "int" => PropertyType.Int,
        "float" => PropertyType.Float,
        "bool" => PropertyType.Bool,
        "color" => PropertyType.Color,
        "file" => PropertyType.File,
        "object" => PropertyType.Object,
        "class" => PropertyType.Class,
        _ => throw new XmlException("Invalid property type")
      }) ?? PropertyType.String;

      IProperty property = type switch
      {
        PropertyType.String => new StringProperty { Name = name, Value = r.GetRequiredAttribute("value") },
        PropertyType.Int => new IntProperty { Name = name, Value = r.GetRequiredAttributeParseable<int>("value") },
        PropertyType.Float => new FloatProperty { Name = name, Value = r.GetRequiredAttributeParseable<float>("value") },
        PropertyType.Bool => new BoolProperty { Name = name, Value = r.GetRequiredAttributeParseable<bool>("value") },
        PropertyType.Color => new ColorProperty { Name = name, Value = r.GetRequiredAttributeParseable<Color>("value") },
        PropertyType.File => new FileProperty { Name = name, Value = r.GetRequiredAttribute("value") },
        PropertyType.Object => new ObjectProperty { Name = name, Value = r.GetRequiredAttributeParseable<uint>("value") },
        PropertyType.Class => ReadClassProperty(r, customTypeDefinitions),
        _ => throw new XmlException("Invalid property type")
      };
      return (name, property);
    }).ToDictionary(x => x.name, x => x.property);
  }

  internal static ClassProperty ReadClassProperty(
    XmlReader reader,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    var name = reader.GetRequiredAttribute("name");
    var propertyType = reader.GetRequiredAttribute("propertytype");

    var customTypeDef = customTypeDefinitions.FirstOrDefault(ctd => ctd.Name == propertyType);
    if (customTypeDef is CustomClassDefinition ccd)
    {
      reader.ReadStartElement("property");
      var propsInType = CreateInstanceOfCustomClass(ccd);
      var props = ReadProperties(reader, customTypeDefinitions);

      var mergedProps = MergeProperties(propsInType, props);

      reader.ReadEndElement();
      return new ClassProperty { Name = name, PropertyType = propertyType, Properties = mergedProps };
    }

    throw new XmlException($"Unkonwn custom class definition: {propertyType}");
  }

  internal static Dictionary<string, IProperty> CreateInstanceOfCustomClass(CustomClassDefinition customClassDefinition)
  {
    return customClassDefinition.Members.ToDictionary(m => m.Name, m => m.Clone());
  }
}
