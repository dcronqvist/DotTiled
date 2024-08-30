using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal List<IProperty> ReadProperties(JsonElement element) =>
    element.GetValueAsList<IProperty>(e =>
    {
      var name = e.GetRequiredProperty<string>("name");
      var type = e.GetOptionalPropertyParseable<PropertyType>("type", s => s switch
      {
        "string" => PropertyType.String,
        "int" => PropertyType.Int,
        "float" => PropertyType.Float,
        "bool" => PropertyType.Bool,
        "color" => PropertyType.Color,
        "file" => PropertyType.File,
        "object" => PropertyType.Object,
        "class" => PropertyType.Class,
        _ => throw new JsonException("Invalid property type")
      }, PropertyType.String);
      var propertyType = e.GetOptionalProperty<string?>("propertytype", null);
      if (propertyType is not null)
      {
        return ReadPropertyWithCustomType(e);
      }

      IProperty property = type switch
      {
        PropertyType.String => new StringProperty { Name = name, Value = e.GetRequiredProperty<string>("value") },
        PropertyType.Int => new IntProperty { Name = name, Value = e.GetRequiredProperty<int>("value") },
        PropertyType.Float => new FloatProperty { Name = name, Value = e.GetRequiredProperty<float>("value") },
        PropertyType.Bool => new BoolProperty { Name = name, Value = e.GetRequiredProperty<bool>("value") },
        PropertyType.Color => new ColorProperty { Name = name, Value = e.GetRequiredPropertyParseable<Color>("value") },
        PropertyType.File => new FileProperty { Name = name, Value = e.GetRequiredProperty<string>("value") },
        PropertyType.Object => new ObjectProperty { Name = name, Value = e.GetRequiredProperty<uint>("value") },
        PropertyType.Class => throw new JsonException("Class property must have a property type"),
        PropertyType.Enum => throw new JsonException("Enum property must have a property type"),
        _ => throw new JsonException("Invalid property type")
      };

      return property!;
    });

  internal IProperty ReadPropertyWithCustomType(JsonElement element)
  {
    var isClass = element.GetOptionalProperty<string?>("type", null) == "class";
    if (isClass)
    {
      return ReadClassProperty(element);
    }

    return ReadEnumProperty(element);
  }

  internal ClassProperty ReadClassProperty(JsonElement element)
  {
    var name = element.GetRequiredProperty<string>("name");
    var propertyType = element.GetRequiredProperty<string>("propertytype");
    var customTypeDef = _customTypeResolver(propertyType);

    if (customTypeDef is CustomClassDefinition ccd)
    {
      var propsInType = Helpers.CreateInstanceOfCustomClass(ccd, _customTypeResolver);
      var props = element.GetOptionalPropertyCustom<List<IProperty>>("value", e => ReadPropertiesInsideClass(e, ccd), []);
      var mergedProps = Helpers.MergeProperties(propsInType, props);

      return new ClassProperty
      {
        Name = name,
        PropertyType = propertyType,
        Value = mergedProps
      };
    }

    throw new JsonException($"Unknown custom class '{propertyType}'.");
  }

  internal List<IProperty> ReadPropertiesInsideClass(
    JsonElement element,
    CustomClassDefinition customClassDefinition)
  {
    List<IProperty> resultingProps = [];

    foreach (var prop in customClassDefinition.Members)
    {
      if (!element.TryGetProperty(prop.Name, out var propElement))
        continue;

      IProperty property = prop.Type switch
      {
        PropertyType.String => new StringProperty { Name = prop.Name, Value = propElement.GetValueAs<string>() },
        PropertyType.Int => new IntProperty { Name = prop.Name, Value = propElement.GetValueAs<int>() },
        PropertyType.Float => new FloatProperty { Name = prop.Name, Value = propElement.GetValueAs<float>() },
        PropertyType.Bool => new BoolProperty { Name = prop.Name, Value = propElement.GetValueAs<bool>() },
        PropertyType.Color => new ColorProperty { Name = prop.Name, Value = Color.Parse(propElement.GetValueAs<string>(), CultureInfo.InvariantCulture) },
        PropertyType.File => new FileProperty { Name = prop.Name, Value = propElement.GetValueAs<string>() },
        PropertyType.Object => new ObjectProperty { Name = prop.Name, Value = propElement.GetValueAs<uint>() },
        PropertyType.Class => new ClassProperty { Name = prop.Name, PropertyType = ((ClassProperty)prop).PropertyType, Value = ReadPropertiesInsideClass(propElement, (CustomClassDefinition)_customTypeResolver(((ClassProperty)prop).PropertyType)) },
        PropertyType.Enum => ReadEnumProperty(propElement),
        _ => throw new JsonException("Invalid property type")
      };

      resultingProps.Add(property);
    }

    return resultingProps;
  }

  internal EnumProperty ReadEnumProperty(JsonElement element)
  {
    var name = element.GetRequiredProperty<string>("name");
    var propertyType = element.GetRequiredProperty<string>("propertytype");
    var typeInXml = element.GetOptionalPropertyParseable<PropertyType>("type", (s) => s switch
    {
      "string" => PropertyType.String,
      "int" => PropertyType.Int,
      _ => throw new JsonException("Invalid property type")
    }, PropertyType.String);
    var customTypeDef = _customTypeResolver(propertyType);

    if (customTypeDef is not CustomEnumDefinition ced)
      throw new JsonException($"Unknown custom enum '{propertyType}'. Enums must be defined");

    if (ced.StorageType == CustomEnumStorageType.String)
    {
      var value = element.GetRequiredProperty<string>("value");
      if (value.Contains(',') && !ced.ValueAsFlags)
        throw new JsonException("Enum value must not contain ',' if not ValueAsFlags is set to true.");

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
      var value = element.GetRequiredProperty<int>("value");
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

    throw new JsonException($"Unknown custom enum '{propertyType}'. Enums must be defined");
  }
}
