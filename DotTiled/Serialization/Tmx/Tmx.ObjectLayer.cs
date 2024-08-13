using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Xml;

namespace DotTiled;

internal partial class Tmx
{
  internal static ObjectLayer ReadObjectLayer(
    XmlReader reader,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Attributes
    var id = reader.GetRequiredAttributeParseable<uint>("id");
    var name = reader.GetOptionalAttribute("name") ?? "";
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var x = reader.GetOptionalAttributeParseable<uint>("x") ?? 0;
    var y = reader.GetOptionalAttributeParseable<uint>("y") ?? 0;
    var width = reader.GetOptionalAttributeParseable<uint>("width");
    var height = reader.GetOptionalAttributeParseable<uint>("height");
    var opacity = reader.GetOptionalAttributeParseable<float>("opacity") ?? 1.0f;
    var visible = reader.GetOptionalAttributeParseable<bool>("visible") ?? true;
    var tintColor = reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = reader.GetOptionalAttributeParseable<float>("offsetx") ?? 0.0f;
    var offsetY = reader.GetOptionalAttributeParseable<float>("offsety") ?? 0.0f;
    var parallaxX = reader.GetOptionalAttributeParseable<float>("parallaxx") ?? 1.0f;
    var parallaxY = reader.GetOptionalAttributeParseable<float>("parallaxy") ?? 1.0f;
    var color = reader.GetOptionalAttributeClass<Color>("color");
    var drawOrder = reader.GetOptionalAttributeEnum<DrawOrder>("draworder", s => s switch
    {
      "topdown" => DrawOrder.TopDown,
      "index" => DrawOrder.Index,
      _ => throw new Exception($"Unknown draw order '{s}'")
    }) ?? DrawOrder.TopDown;

    // Elements
    Dictionary<string, IProperty>? properties = null;
    List<Object> objects = [];

    reader.ProcessChildren("objectgroup", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r, customTypeDefinitions), "Properties"),
      "object" => () => objects.Add(ReadObject(r, externalTemplateResolver, customTypeDefinitions)),
      _ => r.Skip
    });

    return new ObjectLayer
    {
      ID = id,
      Name = name,
      Class = @class,
      X = x,
      Y = y,
      Width = width,
      Height = height,
      Opacity = opacity,
      Visible = visible,
      TintColor = tintColor,
      OffsetX = offsetX,
      OffsetY = offsetY,
      ParallaxX = parallaxX,
      ParallaxY = parallaxY,
      Color = color,
      Properties = properties,
      DrawOrder = drawOrder,
      Objects = objects
    };
  }

  internal static Object ReadObject(
    XmlReader reader,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Attributes
    var template = reader.GetOptionalAttribute("template");

    uint? idDefault = null;
    string nameDefault = "";
    string typeDefault = "";
    float xDefault = 0f;
    float yDefault = 0f;
    float widthDefault = 0f;
    float heightDefault = 0f;
    float rotationDefault = 0f;
    uint? gidDefault = null;
    bool visibleDefault = true;
    Dictionary<string, IProperty>? propertiesDefault = null;

    // Perform template copy first
    if (template is not null)
    {
      var resolvedTemplate = externalTemplateResolver(template);
      var templObj = resolvedTemplate.Object;

      idDefault = templObj.ID;
      nameDefault = templObj.Name;
      typeDefault = templObj.Type;
      xDefault = templObj.X;
      yDefault = templObj.Y;
      widthDefault = templObj.Width;
      heightDefault = templObj.Height;
      rotationDefault = templObj.Rotation;
      visibleDefault = templObj.Visible;
      propertiesDefault = templObj.Properties;
    }

    var id = reader.GetOptionalAttributeParseable<uint>("id") ?? idDefault;
    var name = reader.GetOptionalAttribute("name") ?? nameDefault;
    var type = reader.GetOptionalAttribute("type") ?? typeDefault;
    var x = reader.GetOptionalAttributeParseable<float>("x") ?? xDefault;
    var y = reader.GetOptionalAttributeParseable<float>("y") ?? yDefault;
    var width = reader.GetOptionalAttributeParseable<float>("width") ?? widthDefault;
    var height = reader.GetOptionalAttributeParseable<float>("height") ?? heightDefault;
    var rotation = reader.GetOptionalAttributeParseable<float>("rotation") ?? rotationDefault;
    var gid = reader.GetOptionalAttributeParseable<uint>("gid") ?? gidDefault;
    var visible = reader.GetOptionalAttributeParseable<bool>("visible") ?? visibleDefault;

    // Elements
    Object? obj = null;
    int propertiesCounter = 0;
    Dictionary<string, IProperty>? properties = propertiesDefault;

    reader.ProcessChildren("object", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnceUsingCounter(ref properties, Helpers.MergeProperties(properties, ReadProperties(r, customTypeDefinitions)), "Properties", ref propertiesCounter),
      "ellipse" => () => Helpers.SetAtMostOnce(ref obj, ReadEllipseObject(r), "Object marker"),
      "point" => () => Helpers.SetAtMostOnce(ref obj, ReadPointObject(r), "Object marker"),
      "polygon" => () => Helpers.SetAtMostOnce(ref obj, ReadPolygonObject(r), "Object marker"),
      "polyline" => () => Helpers.SetAtMostOnce(ref obj, ReadPolylineObject(r), "Object marker"),
      "text" => () => Helpers.SetAtMostOnce(ref obj, ReadTextObject(r), "Object marker"),
      _ => throw new Exception($"Unknown object marker '{elementName}'")
    });

    if (gid is not null)
    {
      obj = new TileObject { ID = id, GID = gid.Value };
      reader.Skip();
    }

    if (obj is null)
    {
      obj = new RectangleObject { ID = id };
      reader.Skip();
    }

    obj.ID = id;
    obj.Name = name;
    obj.Type = type;
    obj.X = x;
    obj.Y = y;
    obj.Width = width;
    obj.Height = height;
    obj.Rotation = rotation;
    obj.Visible = visible;
    obj.Template = template;
    obj.Properties = properties;

    return obj;
  }

  internal static EllipseObject ReadEllipseObject(XmlReader reader)
  {
    reader.Skip();
    return new EllipseObject { };
  }

  internal static PointObject ReadPointObject(XmlReader reader)
  {
    reader.Skip();
    return new PointObject { };
  }

  internal static PolygonObject ReadPolygonObject(XmlReader reader)
  {
    // Attributes
    var points = reader.GetRequiredAttributeParseable<List<Vector2>>("points", s =>
    {
      // Takes on format "x1,y1 x2,y2 x3,y3 ..."
      var coords = s.Split(' ');
      return coords.Select(c =>
      {
        var xy = c.Split(',');
        return new Vector2(float.Parse(xy[0], CultureInfo.InvariantCulture), float.Parse(xy[1], CultureInfo.InvariantCulture));
      }).ToList();
    });

    reader.ReadStartElement("polygon");
    return new PolygonObject { Points = points };
  }

  internal static PolylineObject ReadPolylineObject(XmlReader reader)
  {
    // Attributes
    var points = reader.GetRequiredAttributeParseable<List<Vector2>>("points", s =>
    {
      // Takes on format "x1,y1 x2,y2 x3,y3 ..."
      var coords = s.Split(' ');
      return coords.Select(c =>
      {
        var xy = c.Split(',');
        return new Vector2(float.Parse(xy[0], CultureInfo.InvariantCulture), float.Parse(xy[1], CultureInfo.InvariantCulture));
      }).ToList();
    });

    reader.ReadStartElement("polyline");
    return new PolylineObject { Points = points };
  }

  internal static TextObject ReadTextObject(XmlReader reader)
  {
    // Attributes
    var fontFamily = reader.GetOptionalAttribute("fontfamily") ?? "sans-serif";
    var pixelSize = reader.GetOptionalAttributeParseable<int>("pixelsize") ?? 16;
    var wrap = reader.GetOptionalAttributeParseable<bool>("wrap") ?? false;
    var color = reader.GetOptionalAttributeClass<Color>("color") ?? Color.Parse("#000000", CultureInfo.InvariantCulture);
    var bold = reader.GetOptionalAttributeParseable<bool>("bold") ?? false;
    var italic = reader.GetOptionalAttributeParseable<bool>("italic") ?? false;
    var underline = reader.GetOptionalAttributeParseable<bool>("underline") ?? false;
    var strikeout = reader.GetOptionalAttributeParseable<bool>("strikeout") ?? false;
    var kerning = reader.GetOptionalAttributeParseable<bool>("kerning") ?? true;
    var hAlign = reader.GetOptionalAttributeEnum<TextHorizontalAlignment>("halign", s => s switch
    {
      "left" => TextHorizontalAlignment.Left,
      "center" => TextHorizontalAlignment.Center,
      "right" => TextHorizontalAlignment.Right,
      "justify" => TextHorizontalAlignment.Justify,
      _ => throw new Exception($"Unknown horizontal alignment '{s}'")
    }) ?? TextHorizontalAlignment.Left;
    var vAlign = reader.GetOptionalAttributeEnum<TextVerticalAlignment>("valign", s => s switch
    {
      "top" => TextVerticalAlignment.Top,
      "center" => TextVerticalAlignment.Center,
      "bottom" => TextVerticalAlignment.Bottom,
      _ => throw new Exception($"Unknown vertical alignment '{s}'")
    }) ?? TextVerticalAlignment.Top;

    // Elements
    var text = reader.ReadElementContentAsString("text", "");

    return new TextObject
    {
      FontFamily = fontFamily,
      PixelSize = pixelSize,
      Wrap = wrap,
      Color = color,
      Bold = bold,
      Italic = italic,
      Underline = underline,
      Strikeout = strikeout,
      Kerning = kerning,
      HorizontalAlignment = hAlign,
      VerticalAlignment = vAlign,
      Text = text
    };
  }

  internal static Template ReadTemplate(
    XmlReader reader,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // No attributes

    // At most one of
    Tileset? tileset = null;

    // Should contain exactly one of
    Object? obj = null;

    reader.ProcessChildren("template", (r, elementName) => elementName switch
    {
      "tileset" => () => Helpers.SetAtMostOnce(ref tileset, ReadTileset(r, externalTilesetResolver, externalTemplateResolver, customTypeDefinitions), "Tileset"),
      "object" => () => Helpers.SetAtMostOnce(ref obj, ReadObject(r, externalTemplateResolver, customTypeDefinitions), "Object"),
      _ => r.Skip
    });

    if (obj is null)
      throw new NotSupportedException("Template must contain exactly one object");

    return new Template
    {
      Tileset = tileset,
      Object = obj
    };
  }
}
