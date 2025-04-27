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
    var x = _reader.GetOptionalAttributeParseable<int>("x").GetValueOr(0);
    var y = _reader.GetOptionalAttributeParseable<int>("y").GetValueOr(0);
    var width = _reader.GetOptionalAttributeParseable<int>("width").GetValueOr(0);
    var height = _reader.GetOptionalAttributeParseable<int>("height").GetValueOr(0);
    var opacity = _reader.GetOptionalAttributeParseable<float>("opacity").GetValueOr(1.0f);
    var visible = _reader.GetOptionalAttributeParseable<uint>("visible").GetValueOr(1) == 1;
    var tintColor = _reader.GetOptionalAttributeClass<TiledColor>("tintcolor");
    var offsetX = _reader.GetOptionalAttributeParseable<float>("offsetx").GetValueOr(0.0f);
    var offsetY = _reader.GetOptionalAttributeParseable<float>("offsety").GetValueOr(0.0f);
    var parallaxX = _reader.GetOptionalAttributeParseable<float>("parallaxx").GetValueOr(1.0f);
    var parallaxY = _reader.GetOptionalAttributeParseable<float>("parallaxy").GetValueOr(1.0f);
    var color = _reader.GetOptionalAttributeClass<TiledColor>("color");
    var drawOrder = _reader.GetOptionalAttributeEnum<DrawOrder>("draworder", s => s switch
    {
      "topdown" => DrawOrder.TopDown,
      "index" => DrawOrder.Index,
      _ => throw new InvalidOperationException($"Unknown draw order '{s}'")
    }).GetValueOr(DrawOrder.TopDown);

    // Elements
    var propertiesCounter = 0;
    List<IProperty> properties = Helpers.ResolveClassProperties(@class, _customTypeResolver);
    List<DotTiled.Object> objects = [];

    _reader.ProcessChildren("objectgroup", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnceUsingCounter(ref properties, Helpers.MergeProperties(properties, ReadProperties()).ToList(), "Properties", ref propertiesCounter),
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
      obj = _externalTemplateResolver(template.Value).Object;

    uint idDefault = obj?.ID.GetValueOr(0) ?? 0;
    string nameDefault = obj?.Name ?? "";
    string typeDefault = obj?.Type ?? "";
    float xDefault = obj?.X ?? 0f;
    float yDefault = obj?.Y ?? 0f;
    float widthDefault = obj?.Width ?? 0f;
    float heightDefault = obj?.Height ?? 0f;
    float rotationDefault = obj?.Rotation ?? 0f;
    Optional<uint> gidDefault = obj is TileObject tileObj ? tileObj.GID : Optional.Empty;
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
    var visible = _reader.GetOptionalAttributeParseable<uint>("visible").GetValueOr(visibleDefault ? 1u : 0u) == 1;

    // Elements
    DotTiled.Object foundObject = null;
    int propertiesCounter = 0;
    List<IProperty> properties = Helpers.ResolveClassProperties(type, _customTypeResolver) ?? propertiesDefault;

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
      {
        var (clearedGIDs, flippingFlags) = Helpers.ReadAndClearFlippingFlagsFromGIDs([gid.Value]);
        foundObject = new TileObject { ID = id, GID = clearedGIDs.Single(), FlippingFlags = flippingFlags.Single() };
      }
      else
      {
        foundObject = new RectangleObject { ID = id };
      }
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

    if (obj.GetType() != foundObject.GetType())
    {
      return obj;
    }

    return (obj, foundObject) switch
    {
      (TileObject tile, TileObject foundTile) => OverrideObject(tile, foundTile),
      (RectangleObject rectangle, RectangleObject foundRectangle) => OverrideObject(rectangle, foundRectangle),
      (PolygonObject polygon, PolygonObject foundPolygon) => OverrideObject(polygon, foundPolygon),
      (PolylineObject polyline, PolylineObject foundPolyline) => OverrideObject(polyline, foundPolyline),
      (EllipseObject ellipse, EllipseObject foundEllipse) => OverrideObject(ellipse, foundEllipse),
      (TextObject text, TextObject foundText) => OverrideObject(text, foundText),
      (PointObject point, PointObject foundPoint) => OverrideObject(point, foundPoint),
      _ => obj
    };
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

  internal static RectangleObject OverrideObject(RectangleObject obj, RectangleObject foundObject)
  {
    obj.Width = foundObject.Width;
    obj.Height = foundObject.Height;
    return obj;
  }

  internal TextObject ReadTextObject()
  {
    // Attributes
    var fontFamily = _reader.GetOptionalAttribute("fontfamily").GetValueOr("sans-serif");
    var pixelSize = _reader.GetOptionalAttributeParseable<int>("pixelsize").GetValueOr(16);
    var wrap = _reader.GetOptionalAttributeParseable<int>("wrap").GetValueOr(0) == 1;
    var color = _reader.GetOptionalAttributeClass<TiledColor>("color").GetValueOr(TiledColor.Parse("#000000", CultureInfo.InvariantCulture));
    var bold = _reader.GetOptionalAttributeParseable<int>("bold").GetValueOr(0) == 1;
    var italic = _reader.GetOptionalAttributeParseable<int>("italic").GetValueOr(0) == 1;
    var underline = _reader.GetOptionalAttributeParseable<int>("underline").GetValueOr(0) == 1;
    var strikeout = _reader.GetOptionalAttributeParseable<int>("strikeout").GetValueOr(0) == 1;
    var kerning = _reader.GetOptionalAttributeParseable<int>("kerning").GetValueOr(1) == 1;
    var hAlign = _reader.GetOptionalAttributeEnum<TextHorizontalAlignment>("halign", s => s switch
    {
      "left" => TextHorizontalAlignment.Left,
      "center" => TextHorizontalAlignment.Center,
      "right" => TextHorizontalAlignment.Right,
      "justify" => TextHorizontalAlignment.Justify,
      _ => throw new InvalidOperationException($"Unknown horizontal alignment '{s}'")
    }).GetValueOr(TextHorizontalAlignment.Left);
    var vAlign = _reader.GetOptionalAttributeEnum<TextVerticalAlignment>("valign", s => s switch
    {
      "top" => TextVerticalAlignment.Top,
      "center" => TextVerticalAlignment.Center,
      "bottom" => TextVerticalAlignment.Bottom,
      _ => throw new InvalidOperationException($"Unknown vertical alignment '{s}'")
    }).GetValueOr(TextVerticalAlignment.Top);

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
    Tileset tileset = null;

    // Should contain exactly one of
    DotTiled.Object obj = null;

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
