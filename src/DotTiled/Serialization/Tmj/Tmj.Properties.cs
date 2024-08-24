using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

internal partial class Tmj
{
  internal static List<IProperty> ReadProperties(
    JsonElement element,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions) =>
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

      IProperty property = type switch
      {
        PropertyType.String => new StringProperty { Name = name, Value = e.GetRequiredProperty<string>("value") },
        PropertyType.Int => new IntProperty { Name = name, Value = e.GetRequiredProperty<int>("value") },
        PropertyType.Float => new FloatProperty { Name = name, Value = e.GetRequiredProperty<float>("value") },
        PropertyType.Bool => new BoolProperty { Name = name, Value = e.GetRequiredProperty<bool>("value") },
        PropertyType.Color => new ColorProperty { Name = name, Value = e.GetRequiredPropertyParseable<Color>("value") },
        PropertyType.File => new FileProperty { Name = name, Value = e.GetRequiredProperty<string>("value") },
        PropertyType.Object => new ObjectProperty { Name = name, Value = e.GetRequiredProperty<uint>("value") },
        PropertyType.Class => ReadClassProperty(e, customTypeDefinitions),
        _ => throw new JsonException("Invalid property type")
      };

      return property!;
    });

  internal static ClassProperty ReadClassProperty(
    JsonElement element,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    var name = element.GetRequiredProperty<string>("name");
    var propertyType = element.GetRequiredProperty<string>("propertytype");

    var customTypeDef = customTypeDefinitions.FirstOrDefault(ctd => ctd.Name == propertyType);

    if (customTypeDef is CustomClassDefinition ccd)
    {
      var propsInType = Helpers.CreateInstanceOfCustomClass(ccd);
      var props = element.GetOptionalPropertyCustom<List<IProperty>>("value", el => ReadCustomClassProperties(el, ccd, customTypeDefinitions), []);

      var mergedProps = Helpers.MergeProperties(propsInType, props);

      return new ClassProperty
      {
        Name = name,
        PropertyType = propertyType,
        Value = props
      };
    }

    throw new JsonException($"Unknown custom class '{propertyType}'.");
  }

  internal static List<IProperty> ReadCustomClassProperties(
    JsonElement element,
    CustomClassDefinition customClassDefinition,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    List<IProperty> resultingProps = Helpers.CreateInstanceOfCustomClass(customClassDefinition);

    foreach (var prop in customClassDefinition.Members)
    {
      if (!element.TryGetProperty(prop.Name, out var propElement))
        continue; // Property not present in element, therefore will use default value

      IProperty property = prop.Type switch
      {
        PropertyType.String => new StringProperty { Name = prop.Name, Value = propElement.GetValueAs<string>() },
        PropertyType.Int => new IntProperty { Name = prop.Name, Value = propElement.GetValueAs<int>() },
        PropertyType.Float => new FloatProperty { Name = prop.Name, Value = propElement.GetValueAs<float>() },
        PropertyType.Bool => new BoolProperty { Name = prop.Name, Value = propElement.GetValueAs<bool>() },
        PropertyType.Color => new ColorProperty { Name = prop.Name, Value = Color.Parse(propElement.GetValueAs<string>(), CultureInfo.InvariantCulture) },
        PropertyType.File => new FileProperty { Name = prop.Name, Value = propElement.GetValueAs<string>() },
        PropertyType.Object => new ObjectProperty { Name = prop.Name, Value = propElement.GetValueAs<uint>() },
        PropertyType.Class => ReadClassProperty(propElement, customTypeDefinitions),
        _ => throw new JsonException("Invalid property type")
      };

      Helpers.ReplacePropertyInList(resultingProps, property);
    }

    return resultingProps;
  }
}
