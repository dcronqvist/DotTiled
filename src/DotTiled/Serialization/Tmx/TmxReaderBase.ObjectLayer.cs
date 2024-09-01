using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace DotTiled.Serialization.Tmx;

public abstract partial class TmxReaderBase
{
  internal ObjectLayer ReadObjectLayer()
  {
    // Attributes
    var id = _reader.GetRequiredAttributeParseable<uint>("id");
    var name = _reader.GetOptionalAttribute("name").GetValueOr("");
    var @class = _reader.GetOptionalAttribute("class").GetValueOr("");
    var x = _reader.GetOptionalAttributeParseable<uint>("x").GetValueOr(0);
    var y = _reader.GetOptionalAttributeParseable<uint>("y").GetValueOr(0);
    var width = _reader.GetOptionalAttributeParseable<uint>("width").GetValueOr(0);
    var height = _reader.GetOptionalAttributeParseable<uint>("height").GetValueOr(0);
    var opacity = _reader.GetOptionalAttributeParseable<float>("opacity").GetValueOr(1.0f);
    var visible = _reader.GetOptionalAttributeParseable<uint>("visible").GetValueOr(1) == 1;
    var tintColor = _reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = _reader.GetOptionalAttributeParseable<float>("offsetx").GetValueOr(0.0f);
    var offsetY = _reader.GetOptionalAttributeParseable<float>("offsety").GetValueOr(0.0f);
    var parallaxX = _reader.GetOptionalAttributeParseable<float>("parallaxx").GetValueOr(1.0f);
    var parallaxY = _reader.GetOptionalAttributeParseable<float>("parallaxy").GetValueOr(1.0f);
    var color = _reader.GetOptionalAttributeClass<Color>("color");
    var drawOrder = _reader.GetOptionalAttributeEnum<DrawOrder>("draworder", s => s switch
    {
      "topdown" => DrawOrder.TopDown,
      "index" => DrawOrder.Index,
      _ => throw new InvalidOperationException($"Unknown draw order '{s}'")
    }).GetValueOr(DrawOrder.TopDown);

    // Elements
    List<IProperty> properties = null;
    List<DotTiled.Object> objects = [];

    _reader.ProcessChildren("objectgroup", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      "object" => () => objects.Add(ReadObject()),
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
      Properties = properties ?? [],
      DrawOrder = drawOrder,
      Objects = objects
    };
  }

  internal DotTiled.Object ReadObject()
  {
    // Attributes
    var template = _reader.GetOptionalAttribute("template");
    DotTiled.Object obj = null;
    if (template.HasValue)
      obj = _externalTemplateResolver(template).Object;

    uint idDefault = obj?.ID.GetValueOr(0) ?? 0;
    string nameDefault = obj?.Name ?? "";
    string typeDefault = obj?.Type ?? "";
    float xDefault = obj?.X ?? 0f;
    float yDefault = obj?.Y ?? 0f;
    float widthDefault = obj?.Width ?? 0f;
    float heightDefault = obj?.Height ?? 0f;
    float rotationDefault = obj?.Rotation ?? 0f;
    Optional<uint> gidDefault = obj is TileObject tileObj ? tileObj.GID : Optional<uint>.Empty;
    bool visibleDefault = obj?.Visible ?? true;
    List<IProperty> propertiesDefault = obj?.Properties ?? null;

    var id = _reader.GetOptionalAttributeParseable<uint>("id").GetValueOr(idDefault);
    var name = _reader.GetOptionalAttribute("name").GetValueOr(nameDefault);
    var type = _reader.GetOptionalAttribute("type").GetValueOr(typeDefault);
    var x = _reader.GetOptionalAttributeParseable<float>("x").GetValueOr(xDefault);
    var y = _reader.GetOptionalAttributeParseable<float>("y").GetValueOr(yDefault);
    var width = _reader.GetOptionalAttributeParseable<float>("width").GetValueOr(widthDefault);
    var height = _reader.GetOptionalAttributeParseable<float>("height").GetValueOr(heightDefault);
    var rotation = _reader.GetOptionalAttributeParseable<float>("rotation").GetValueOr(rotationDefault);
    var gid = _reader.GetOptionalAttributeParseable<uint>("gid").GetValueOrOptional(gidDefault);
    var visible = _reader.GetOptionalAttributeParseable<bool>("visible").GetValueOr(visibleDefault);

    // Elements
    DotTiled.Object foundObject = null;
    int propertiesCounter = 0;
    List<IProperty> properties = propertiesDefault;

    _reader.ProcessChildren("object", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnceUsingCounter(ref properties, Helpers.MergeProperties(properties, ReadProperties()).ToList(), "Properties", ref propertiesCounter),
      "ellipse" => () => Helpers.SetAtMostOnce(ref foundObject, ReadEllipseObject(), "Object marker"),
      "point" => () => Helpers.SetAtMostOnce(ref foundObject, ReadPointObject(), "Object marker"),
      "polygon" => () => Helpers.SetAtMostOnce(ref foundObject, ReadPolygonObject(), "Object marker"),
      "polyline" => () => Helpers.SetAtMostOnce(ref foundObject, ReadPolylineObject(), "Object marker"),
      "text" => () => Helpers.SetAtMostOnce(ref foundObject, ReadTextObject(), "Object marker"),
      _ => throw new InvalidOperationException($"Unknown object marker '{elementName}'")
    });

    if (foundObject is null)
    {
      if (gid.HasValue)
        foundObject = new TileObject { ID = id, GID = gid.Value };
      else
        foundObject = new RectangleObject { ID = id };
    }

    foundObject.ID = id;
    foundObject.Name = name;
    foundObject.Type = type;
    foundObject.X = x;
    foundObject.Y = y;
    foundObject.Width = width;
    foundObject.Height = height;
    foundObject.Rotation = rotation;
    foundObject.Visible = visible;
    foundObject.Properties = properties ?? [];
    foundObject.Template = template;

    return OverrideObject(obj, foundObject);
  }

  internal static DotTiled.Object OverrideObject(DotTiled.Object obj, DotTiled.Object foundObject)
  {
    if (obj is null)
      return foundObject;

    if (obj.GetType() != foundObject.GetType())
    {
      obj.ID = foundObject.ID;
      obj.Name = foundObject.Name;
      obj.Type = foundObject.Type;
      obj.X = foundObject.X;
      obj.Y = foundObject.Y;
      obj.Width = foundObject.Width;
      obj.Height = foundObject.Height;
      obj.Rotation = foundObject.Rotation;
      obj.Visible = foundObject.Visible;
      obj.Properties = Helpers.MergeProperties(obj.Properties, foundObject.Properties).ToList();
      obj.Template = foundObject.Template;
      return obj;
    }

    return OverrideObject((dynamic)obj, (dynamic)foundObject);
  }

  internal EllipseObject ReadEllipseObject()
  {
    _reader.Skip();
    return new EllipseObject { };
  }

  internal static EllipseObject OverrideObject(EllipseObject obj, EllipseObject _) => obj;

  internal PointObject ReadPointObject()
  {
    _reader.Skip();
    return new PointObject { };
  }

  internal static PointObject OverrideObject(PointObject obj, PointObject _) => obj;

  internal PolygonObject ReadPolygonObject()
  {
    // Attributes
    var points = _reader.GetRequiredAttributeParseable<List<Vector2>>("points", s =>
    {
      // Takes on format "x1,y1 x2,y2 x3,y3 ..."
      var coords = s.Split(' ');
      return coords.Select(c =>
      {
        var xy = c.Split(',');
        return new Vector2(float.Parse(xy[0], CultureInfo.InvariantCulture), float.Parse(xy[1], CultureInfo.InvariantCulture));
      }).ToList();
    });

    _reader.ReadStartElement("polygon");
    return new PolygonObject { Points = points };
  }

  internal static PolygonObject OverrideObject(PolygonObject obj, PolygonObject foundObject)
  {
    obj.Points = foundObject.Points;
    return obj;
  }

  internal PolylineObject ReadPolylineObject()
  {
    // Attributes
    var points = _reader.GetRequiredAttributeParseable<List<Vector2>>("points", s =>
    {
      // Takes on format "x1,y1 x2,y2 x3,y3 ..."
      var coords = s.Split(' ');
      return coords.Select(c =>
      {
        var xy = c.Split(',');
        return new Vector2(float.Parse(xy[0], CultureInfo.InvariantCulture), float.Parse(xy[1], CultureInfo.InvariantCulture));
      }).ToList();
    });

    _reader.ReadStartElement("polyline");
    return new PolylineObject { Points = points };
  }

  internal static PolylineObject OverrideObject(PolylineObject obj, PolylineObject foundObject)
  {
    obj.Points = foundObject.Points;
    return obj;
  }

  internal TextObject ReadTextObject()
  {
    // Attributes
    var fontFamily = _reader.GetOptionalAttribute("fontfamily") ?? "sans-serif";
    var pixelSize = _reader.GetOptionalAttributeParseable<int>("pixelsize") ?? 16;
    var wrap = _reader.GetOptionalAttributeParseable<bool>("wrap") ?? false;
    var color = _reader.GetOptionalAttributeClass<Color>("color") ?? Color.Parse("#000000", CultureInfo.InvariantCulture);
    var bold = _reader.GetOptionalAttributeParseable<bool>("bold") ?? false;
    var italic = _reader.GetOptionalAttributeParseable<bool>("italic") ?? false;
    var underline = _reader.GetOptionalAttributeParseable<bool>("underline") ?? false;
    var strikeout = _reader.GetOptionalAttributeParseable<bool>("strikeout") ?? false;
    var kerning = _reader.GetOptionalAttributeParseable<bool>("kerning") ?? true;
    var hAlign = _reader.GetOptionalAttributeEnum<TextHorizontalAlignment>("halign", s => s switch
    {
      "left" => TextHorizontalAlignment.Left,
      "center" => TextHorizontalAlignment.Center,
      "right" => TextHorizontalAlignment.Right,
      "justify" => TextHorizontalAlignment.Justify,
      _ => throw new InvalidOperationException($"Unknown horizontal alignment '{s}'")
    }) ?? TextHorizontalAlignment.Left;
    var vAlign = _reader.GetOptionalAttributeEnum<TextVerticalAlignment>("valign", s => s switch
    {
      "top" => TextVerticalAlignment.Top,
      "center" => TextVerticalAlignment.Center,
      "bottom" => TextVerticalAlignment.Bottom,
      _ => throw new InvalidOperationException($"Unknown vertical alignment '{s}'")
    }) ?? TextVerticalAlignment.Top;

    // Elements
    var text = _reader.ReadElementContentAsString("text", "");

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

  internal static TextObject OverrideObject(TextObject obj, TextObject foundObject)
  {
    obj.FontFamily = foundObject.FontFamily;
    obj.PixelSize = foundObject.PixelSize;
    obj.Wrap = foundObject.Wrap;
    obj.Color = foundObject.Color;
    obj.Bold = foundObject.Bold;
    obj.Italic = foundObject.Italic;
    obj.Underline = foundObject.Underline;
    obj.Strikeout = foundObject.Strikeout;
    obj.Kerning = foundObject.Kerning;
    obj.HorizontalAlignment = foundObject.HorizontalAlignment;
    obj.VerticalAlignment = foundObject.VerticalAlignment;
    obj.Text = foundObject.Text;
    return obj;
  }

  internal static TileObject OverrideObject(TileObject obj, TileObject foundObject)
  {
    obj.GID = foundObject.GID;
    return obj;
  }

  internal Template ReadTemplate()
  {
    // No attributes

    // At most one of
    Tileset? tileset = null;

    // Should contain exactly one of
    DotTiled.Object? obj = null;

    _reader.ProcessChildren("template", (r, elementName) => elementName switch
    {
      "tileset" => () => Helpers.SetAtMostOnce(ref tileset, ReadTileset(), "Tileset"),
      "object" => () => Helpers.SetAtMostOnce(ref obj, ReadObject(), "Object"),
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
