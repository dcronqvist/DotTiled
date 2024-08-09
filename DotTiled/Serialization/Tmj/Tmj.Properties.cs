using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static Dictionary<string, IProperty> ReadProperties(JsonElement element) =>
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
        PropertyType.Class => ReadClassProperty(e),
        _ => throw new JsonException("Invalid property type")
      };

      return property!;
    }).ToDictionary(p => p.Name);

  internal static ClassProperty ReadClassProperty(JsonElement element)
  {
    var name = element.GetRequiredProperty<string>("name");
    var propertyType = element.GetRequiredProperty<string>("propertytype");

    var properties = element.GetRequiredPropertyCustom<Dictionary<string, IProperty>>("properties", ReadProperties);

    return new ClassProperty { Name = name, PropertyType = propertyType, Properties = properties };
  }
}
