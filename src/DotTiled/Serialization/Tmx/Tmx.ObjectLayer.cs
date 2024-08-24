using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Xml;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

internal partial class Tmx
{
  internal static ObjectLayer ReadObjectLayer(
    XmlReader reader,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
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
      _ => throw new InvalidOperationException($"Unknown draw order '{s}'")
    }) ?? DrawOrder.TopDown;

    // Elements
    List<IProperty>? properties = null;
    List<Model.Object> objects = [];

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
      Properties = properties ?? [],
      DrawOrder = drawOrder,
      Objects = objects
    };
  }

  internal static Model.Object ReadObject(
    XmlReader reader,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    // Attributes
    var template = reader.GetOptionalAttribute("template");
    Model.Object? obj = null;
    if (template is not null)
      obj = externalTemplateResolver(template).Object;

    uint? idDefault = obj?.ID ?? null;
    string nameDefault = obj?.Name ?? "";
    string typeDefault = obj?.Type ?? "";
    float xDefault = obj?.X ?? 0f;
    float yDefault = obj?.Y ?? 0f;
    float widthDefault = obj?.Width ?? 0f;
    float heightDefault = obj?.Height ?? 0f;
    float rotationDefault = obj?.Rotation ?? 0f;
    uint? gidDefault = obj is TileObject tileObj ? tileObj.GID : null;
    bool visibleDefault = obj?.Visible ?? true;
    List<IProperty>? propertiesDefault = obj?.Properties ?? null;

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
    Model.Object? foundObject = null;
    int propertiesCounter = 0;
    List<IProperty>? properties = propertiesDefault;

    reader.ProcessChildren("object", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnceUsingCounter(ref properties, Helpers.MergeProperties(properties, ReadProperties(r, customTypeDefinitions)).ToList(), "Properties", ref propertiesCounter),
      "ellipse" => () => Helpers.SetAtMostOnce(ref foundObject, ReadEllipseObject(r), "Object marker"),
      "point" => () => Helpers.SetAtMostOnce(ref foundObject, ReadPointObject(r), "Object marker"),
      "polygon" => () => Helpers.SetAtMostOnce(ref foundObject, ReadPolygonObject(r), "Object marker"),
      "polyline" => () => Helpers.SetAtMostOnce(ref foundObject, ReadPolylineObject(r), "Object marker"),
      "text" => () => Helpers.SetAtMostOnce(ref foundObject, ReadTextObject(r), "Object marker"),
      _ => throw new InvalidOperationException($"Unknown object marker '{elementName}'")
    });

    if (foundObject is null)
    {
      if (gid is not null)
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

  internal static Model.Object OverrideObject(Model.Object? obj, Model.Object foundObject)
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

  internal static EllipseObject ReadEllipseObject(XmlReader reader)
  {
    reader.Skip();
    return new EllipseObject { };
  }

  internal static EllipseObject OverrideObject(EllipseObject obj, EllipseObject _) => obj;

  internal static PointObject ReadPointObject(XmlReader reader)
  {
    reader.Skip();
    return new PointObject { };
  }

  internal static PointObject OverrideObject(PointObject obj, PointObject _) => obj;

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

  internal static PolygonObject OverrideObject(PolygonObject obj, PolygonObject foundObject)
  {
    obj.Points = foundObject.Points;
    return obj;
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

  internal static PolylineObject OverrideObject(PolylineObject obj, PolylineObject foundObject)
  {
    obj.Points = foundObject.Points;
    return obj;
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
      _ => throw new InvalidOperationException($"Unknown horizontal alignment '{s}'")
    }) ?? TextHorizontalAlignment.Left;
    var vAlign = reader.GetOptionalAttributeEnum<TextVerticalAlignment>("valign", s => s switch
    {
      "top" => TextVerticalAlignment.Top,
      "center" => TextVerticalAlignment.Center,
      "bottom" => TextVerticalAlignment.Bottom,
      _ => throw new InvalidOperationException($"Unknown vertical alignment '{s}'")
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

  internal static Template ReadTemplate(
    XmlReader reader,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    // No attributes

    // At most one of
    Tileset? tileset = null;

    // Should contain exactly one of
    Model.Object? obj = null;

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
