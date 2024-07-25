using System.Xml;
using System.Xml.Serialization;

namespace DotTiled;

public static class XmlHelpers
{
  public static Dictionary<string, IProperty> ReadProperties(XmlReader reader)
  {
    return reader.ReadList<(string PropName, IProperty Prop)>("properties", "property",
      reader =>
      {
        var type = reader.GetRequiredAttributeEnum<PropertyType>("type");
        var propertyRuntimeType = type switch
        {
          PropertyType.String => typeof(StringProperty),
          PropertyType.Int => typeof(IntProperty),
          PropertyType.Float => typeof(FloatProperty),
          PropertyType.Bool => typeof(BooleanProperty),
          PropertyType.Color => typeof(ColorProperty),
          PropertyType.File => typeof(FileProperty),
          PropertyType.Object => typeof(ObjectProperty),
          PropertyType.Class => typeof(ClassProperty),
          _ => throw new XmlException("Invalid property type")
        };

        var serializer = new XmlSerializer(propertyRuntimeType);
        var deserializedProperty = (IProperty)serializer.Deserialize(reader)!;
        return (deserializedProperty.Name, deserializedProperty);
      }
    ).ToDictionary(x => x.PropName, x => x.Prop);
  }

  public static BaseTileset ReadTileset(XmlReader reader, Func<string, BaseTileset> tilesetResolver)
  {
    var imageChildren = reader.CountDirectChildrenWithName("image");
    var tileChildren = reader.CountDirectChildrenWithName("tile");
    if (imageChildren == 0 && tileChildren == 0)
    {
      // This is a tileset that must have "source" set
      var source = reader.GetRequiredAttribute("source");
      return tilesetResolver(source);
    }
    if (imageChildren == 1)
    {
      // This is a single image tileset
      return reader.ReadElementAs<ImageTileset>();
    }

    throw new XmlException("Invalid tileset");
  }
}
