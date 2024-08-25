using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

public abstract partial class TmxReaderBase
{
  internal List<IProperty> ReadProperties()
  {
    return _reader.ReadList("properties", "property", (r) =>
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
      var propertyType = r.GetOptionalAttribute("propertytype");
      if (propertyType is not null)
      {
        return ReadPropertyWithCustomType();
      }

      IProperty property = type switch
      {
        PropertyType.String => new StringProperty { Name = name, Value = r.GetRequiredAttribute("value") },
        PropertyType.Int => new IntProperty { Name = name, Value = r.GetRequiredAttributeParseable<int>("value") },
        PropertyType.Float => new FloatProperty { Name = name, Value = r.GetRequiredAttributeParseable<float>("value") },
        PropertyType.Bool => new BoolProperty { Name = name, Value = r.GetRequiredAttributeParseable<bool>("value") },
        PropertyType.Color => new ColorProperty { Name = name, Value = r.GetRequiredAttributeParseable<Color>("value") },
        PropertyType.File => new FileProperty { Name = name, Value = r.GetRequiredAttribute("value") },
        PropertyType.Object => new ObjectProperty { Name = name, Value = r.GetRequiredAttributeParseable<uint>("value") },
        PropertyType.Class => ReadClassProperty(),
        _ => throw new XmlException("Invalid property type")
      };
      return property;
    });
  }

  internal IProperty ReadPropertyWithCustomType()
  {
    var isClass = _reader.GetOptionalAttribute("type") == "class";

    if (isClass)
    {
      return ReadClassProperty();
    }

    return ReadEnumProperty();
  }

  internal ClassProperty ReadClassProperty()
  {
    var name = _reader.GetRequiredAttribute("name");
    var propertyType = _reader.GetRequiredAttribute("propertytype");

    var customTypeDef = _customTypeResolver(propertyType);
    if (customTypeDef is CustomClassDefinition ccd)
    {
      _reader.ReadStartElement("property");
      var propsInType = Helpers.CreateInstanceOfCustomClass(ccd);
      var props = ReadProperties();
      var mergedProps = Helpers.MergeProperties(propsInType, props);

      _reader.ReadEndElement();
      return new ClassProperty { Name = name, PropertyType = propertyType, Value = mergedProps };
    }

    throw new XmlException($"Unkonwn custom class definition: {propertyType}");
  }

  internal EnumProperty ReadEnumProperty()
  {
    var name = _reader.GetRequiredAttribute("name");
    var propertyType = _reader.GetRequiredAttribute("propertytype");
    var typeInXml = _reader.GetOptionalAttributeEnum<PropertyType>("type", (s) => s switch
    {
      "string" => PropertyType.String,
      "int" => PropertyType.Int,
      _ => throw new XmlException("Invalid property type")
    }) ?? PropertyType.String;
    var customTypeDef = _customTypeResolver(propertyType);

    if (customTypeDef is not CustomEnumDefinition ced)
      throw new XmlException($"Unknown custom enum definition: {propertyType}. Enums must be defined");

    if (ced.StorageType == CustomEnumStorageType.String)
    {
      var value = _reader.GetRequiredAttribute("value");
      if (value.Contains(',') && !ced.ValueAsFlags)
        throw new XmlException("Enum value must not contain ',' if not ValueAsFlags is set to true.");

      if (ced.ValueAsFlags)
      {
        var values = value.Split(',').Select(v => v.Trim()).ToHashSet();
        return new EnumProperty { Name = name, PropertyType = propertyType, Value = values };
      }
      else
      {
        return new EnumProperty { Name = name, PropertyType = propertyType, Value = new HashSet<string> { value } };
      }
    }
    else if (ced.StorageType == CustomEnumStorageType.Int)
    {
      var value = _reader.GetRequiredAttributeParseable<int>("value");
      if (ced.ValueAsFlags)
      {
        var allValues = ced.Values;
        var enumValues = new HashSet<string>();
        for (var i = 0; i < allValues.Count; i++)
        {
          var mask = 1 << i;
          if ((value & mask) == mask)
          {
            var enumValue = allValues[i];
            _ = enumValues.Add(enumValue);
          }
        }
        return new EnumProperty { Name = name, PropertyType = propertyType, Value = enumValues };
      }
      else
      {
        var allValues = ced.Values;
        var enumValue = allValues[value];
        return new EnumProperty { Name = name, PropertyType = propertyType, Value = new HashSet<string> { enumValue } };
      }
    }

    throw new XmlException($"Unknown custom enum storage type: {ced.StorageType}");
  }
}
