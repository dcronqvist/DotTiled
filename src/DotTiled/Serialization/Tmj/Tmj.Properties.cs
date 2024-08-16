using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using DotTiled.Model;
using DotTiled.Model.Properties;
using DotTiled.Model.Properties.CustomTypes;

namespace DotTiled.Serialization.Tmj;

internal partial class Tmj
{
  internal static Dictionary<string, IProperty> ReadProperties(
    JsonElement element,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions) =>
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
    }).ToDictionary(p => p.Name);

  internal static ClassProperty ReadClassProperty(
    JsonElement element,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    var name = element.GetRequiredProperty<string>("name");
    var propertyType = element.GetRequiredProperty<string>("propertytype");

    var customTypeDef = customTypeDefinitions.FirstOrDefault(ctd => ctd.Name == propertyType);

    if (customTypeDef is CustomClassDefinition ccd)
    {
      var propsInType = CreateInstanceOfCustomClass(ccd);
      var props = element.GetOptionalPropertyCustom<Dictionary<string, IProperty>>("value", el => ReadCustomClassProperties(el, ccd, customTypeDefinitions), []);

      var mergedProps = Helpers.MergeProperties(propsInType, props);

      return new ClassProperty
      {
        Name = name,
        PropertyType = propertyType,
        Properties = mergedProps
      };
    }

    throw new JsonException($"Unknown custom class '{propertyType}'.");
  }

  internal static Dictionary<string, IProperty> ReadCustomClassProperties(
    JsonElement element,
    CustomClassDefinition customClassDefinition,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    Dictionary<string, IProperty> resultingProps = [];

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

      resultingProps[prop.Name] = property;
    }

    return resultingProps;
  }

  internal static Dictionary<string, IProperty> CreateInstanceOfCustomClass(CustomClassDefinition customClassDefinition)
  {
    return customClassDefinition.Members.ToDictionary(m => m.Name, m => m.Clone());
  }
}
