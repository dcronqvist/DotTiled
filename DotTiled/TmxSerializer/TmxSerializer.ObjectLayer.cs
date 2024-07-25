using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Xml;

namespace DotTiled;

public partial class TmxSerializer
{
  private ObjectLayer ReadObjectLayer(XmlReader reader)
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
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r), "Properties"),
      "object" => () => objects.Add(ReadObject(r)),
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

  private Object ReadObject(XmlReader reader)
  {
    // Attributes
    var id = reader.GetRequiredAttributeParseable<uint>("id");
    var name = reader.GetOptionalAttribute("name") ?? "";
    var type = reader.GetOptionalAttribute("type") ?? "";
    var x = reader.GetOptionalAttributeParseable<float>("x") ?? 0f;
    var y = reader.GetOptionalAttributeParseable<float>("y") ?? 0f;
    var width = reader.GetOptionalAttributeParseable<float>("width") ?? 0f;
    var height = reader.GetOptionalAttributeParseable<float>("height") ?? 0f;
    var rotation = reader.GetOptionalAttributeParseable<float>("rotation") ?? 0f;
    var gid = reader.GetOptionalAttributeParseable<uint>("gid");
    var visible = reader.GetOptionalAttributeParseable<bool>("visible") ?? true;
    var template = reader.GetOptionalAttribute("template");

    // Elements
    Object? obj = null;
    Dictionary<string, IProperty>? properties = null;

    reader.ProcessChildren("object", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r), "Properties"),
      "ellipse" => () => Helpers.SetAtMostOnce(ref obj, ReadEllipseObject(r, id), "Object marker"),
      "point" => () => Helpers.SetAtMostOnce(ref obj, ReadPointObject(r, id), "Object marker"),
      "polygon" => () => Helpers.SetAtMostOnce(ref obj, ReadPolygonObject(r, id), "Object marker"),
      "polyline" => () => Helpers.SetAtMostOnce(ref obj, ReadPolylineObject(r, id), "Object marker"),
      "text" => () => Helpers.SetAtMostOnce(ref obj, ReadTextObject(r, id), "Object marker"),
      _ => throw new Exception($"Unknown object marker '{elementName}'")
    });

    if (obj is null)
    {
      obj = new RectangleObject { ID = id };
      reader.Skip();
    }

    obj.Name = name;
    obj.Type = type;
    obj.X = x;
    obj.Y = y;
    obj.Width = width;
    obj.Height = height;
    obj.Rotation = rotation;
    obj.GID = gid;
    obj.Visible = visible;
    obj.Template = template;
    obj.Properties = properties;

    return obj;
  }

  private EllipseObject ReadEllipseObject(XmlReader reader, uint id)
  {
    reader.Skip();
    return new EllipseObject { ID = id };
  }

  private PointObject ReadPointObject(XmlReader reader, uint id)
  {
    reader.Skip();
    return new PointObject { ID = id };
  }

  private PolygonObject ReadPolygonObject(XmlReader reader, uint id)
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
    return new PolygonObject { ID = id, Points = points };
  }

  private PolylineObject ReadPolylineObject(XmlReader reader, uint id)
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
    return new PolylineObject { ID = id, Points = points };
  }

  private TextObject ReadTextObject(XmlReader reader, uint id)
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
      ID = id,
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
}
