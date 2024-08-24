using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

internal partial class Tmj
{
  internal static ObjectLayer ReadObjectLayer(
      JsonElement element,
      Func<string, Template> externalTemplateResolver,
      IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    var id = element.GetRequiredProperty<uint>("id");
    var name = element.GetRequiredProperty<string>("name");
    var @class = element.GetOptionalProperty<string>("class", "");
    var opacity = element.GetOptionalProperty<float>("opacity", 1.0f);
    var visible = element.GetOptionalProperty<bool>("visible", true);
    var tintColor = element.GetOptionalPropertyParseable<Color?>("tintcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var offsetX = element.GetOptionalProperty<float>("offsetx", 0.0f);
    var offsetY = element.GetOptionalProperty<float>("offsety", 0.0f);
    var parallaxX = element.GetOptionalProperty<float>("parallaxx", 1.0f);
    var parallaxY = element.GetOptionalProperty<float>("parallaxy", 1.0f);
    var properties = element.GetOptionalPropertyCustom<Dictionary<string, IProperty>?>("properties", e => ReadProperties(e, customTypeDefinitions), null);

    var x = element.GetOptionalProperty<uint>("x", 0);
    var y = element.GetOptionalProperty<uint>("y", 0);
    var width = element.GetOptionalProperty<uint?>("width", null);
    var height = element.GetOptionalProperty<uint?>("height", null);
    var color = element.GetOptionalPropertyParseable<Color?>("color", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var drawOrder = element.GetOptionalPropertyParseable<DrawOrder>("draworder", s => s switch
    {
      "topdown" => DrawOrder.TopDown,
      "index" => DrawOrder.Index,
      _ => throw new JsonException($"Unknown draw order '{s}'.")
    }, DrawOrder.TopDown);

    var objects = element.GetOptionalPropertyCustom<List<Model.Object>>("objects", e => e.GetValueAsList<Model.Object>(el => ReadObject(el, externalTemplateResolver, customTypeDefinitions)), []);

    return new ObjectLayer
    {
      ID = id,
      Name = name,
      Class = @class,
      Opacity = opacity,
      Visible = visible,
      TintColor = tintColor,
      OffsetX = offsetX,
      OffsetY = offsetY,
      ParallaxX = parallaxX,
      ParallaxY = parallaxY,
      Properties = properties,
      X = x,
      Y = y,
      Width = width,
      Height = height,
      Color = color,
      DrawOrder = drawOrder,
      Objects = objects
    };
  }

  internal static Model.Object ReadObject(
    JsonElement element,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
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
    bool ellipseDefault = false;
    bool pointDefault = false;
    List<Vector2>? polygonDefault = null;
    List<Vector2>? polylineDefault = null;
    Dictionary<string, IProperty>? propertiesDefault = null;

    var template = element.GetOptionalProperty<string?>("template", null);
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
      ellipseDefault = templObj is EllipseObject;
      pointDefault = templObj is PointObject;
      polygonDefault = (templObj is PolygonObject polygonObj) ? polygonObj.Points : null;
      polylineDefault = (templObj is PolylineObject polylineObj) ? polylineObj.Points : null;
    }

    var ellipse = element.GetOptionalProperty<bool>("ellipse", ellipseDefault);
    var gid = element.GetOptionalProperty<uint?>("gid", gidDefault);
    var height = element.GetOptionalProperty<float>("height", heightDefault);
    var id = element.GetOptionalProperty<uint?>("id", idDefault);
    var name = element.GetOptionalProperty<string>("name", nameDefault);
    var point = element.GetOptionalProperty<bool>("point", pointDefault);
    var polygon = element.GetOptionalPropertyCustom<List<Vector2>?>("polygon", ReadPoints, polygonDefault);
    var polyline = element.GetOptionalPropertyCustom<List<Vector2>?>("polyline", ReadPoints, polylineDefault);
    var properties = element.GetOptionalPropertyCustom<Dictionary<string, IProperty>?>("properties", e => ReadProperties(e, customTypeDefinitions), propertiesDefault);
    var rotation = element.GetOptionalProperty<float>("rotation", rotationDefault);
    var text = element.GetOptionalPropertyCustom<TextObject?>("text", ReadText, null);
    var type = element.GetOptionalProperty<string>("type", typeDefault);
    var visible = element.GetOptionalProperty<bool>("visible", visibleDefault);
    var width = element.GetOptionalProperty<float>("width", widthDefault);
    var x = element.GetOptionalProperty<float>("x", xDefault);
    var y = element.GetOptionalProperty<float>("y", yDefault);

    if (gid is not null)
    {
      return new TileObject
      {
        ID = id,
        Name = name,
        Type = type,
        X = x,
        Y = y,
        Width = width,
        Height = height,
        Rotation = rotation,
        Visible = visible,
        Template = template,
        Properties = properties,
        GID = gid.Value
      };
    }

    if (ellipse)
    {
      return new EllipseObject
      {
        ID = id,
        Name = name,
        Type = type,
        X = x,
        Y = y,
        Width = width,
        Height = height,
        Rotation = rotation,
        Visible = visible,
        Template = template,
        Properties = properties
      };
    }

    if (point)
    {
      return new PointObject
      {
        ID = id,
        Name = name,
        Type = type,
        X = x,
        Y = y,
        Width = width,
        Height = height,
        Rotation = rotation,
        Visible = visible,
        Template = template,
        Properties = properties
      };
    }

    if (polygon is not null)
    {
      return new PolygonObject
      {
        ID = id,
        Name = name,
        Type = type,
        X = x,
        Y = y,
        Width = width,
        Height = height,
        Rotation = rotation,
        Visible = visible,
        Template = template,
        Properties = properties,
        Points = polygon
      };
    }

    if (polyline is not null)
    {
      return new PolylineObject
      {
        ID = id,
        Name = name,
        Type = type,
        X = x,
        Y = y,
        Width = width,
        Height = height,
        Rotation = rotation,
        Visible = visible,
        Template = template,
        Properties = properties,
        Points = polyline
      };
    }

    if (text is not null)
    {
      text.ID = id;
      text.Name = name;
      text.Type = type;
      text.X = x;
      text.Y = y;
      text.Width = width;
      text.Height = height;
      text.Rotation = rotation;
      text.Visible = visible;
      text.Template = template;
      text.Properties = properties;
      return text;
    }

    return new RectangleObject
    {
      ID = id,
      Name = name,
      Type = type,
      X = x,
      Y = y,
      Width = width,
      Height = height,
      Rotation = rotation,
      Visible = visible,
      Template = template,
      Properties = properties
    };
  }

  internal static List<Vector2> ReadPoints(JsonElement element) =>
    element.GetValueAsList<Vector2>(e =>
    {
      var x = e.GetRequiredProperty<float>("x");
      var y = e.GetRequiredProperty<float>("y");
      return new Vector2(x, y);
    });

  internal static TextObject ReadText(JsonElement element)
  {
    var bold = element.GetOptionalProperty<bool>("bold", false);
    var color = element.GetOptionalPropertyParseable<Color>("color", s => Color.Parse(s, CultureInfo.InvariantCulture), Color.Parse("#00000000", CultureInfo.InvariantCulture));
    var fontfamily = element.GetOptionalProperty<string>("fontfamily", "sans-serif");
    var halign = element.GetOptionalPropertyParseable<TextHorizontalAlignment>("halign", s => s switch
    {
      "left" => TextHorizontalAlignment.Left,
      "center" => TextHorizontalAlignment.Center,
      "right" => TextHorizontalAlignment.Right,
      _ => throw new JsonException($"Unknown horizontal alignment '{s}'.")
    }, TextHorizontalAlignment.Left);
    var italic = element.GetOptionalProperty<bool>("italic", false);
    var kerning = element.GetOptionalProperty<bool>("kerning", true);
    var pixelsize = element.GetOptionalProperty<int>("pixelsize", 16);
    var strikeout = element.GetOptionalProperty<bool>("strikeout", false);
    var text = element.GetRequiredProperty<string>("text");
    var underline = element.GetOptionalProperty<bool>("underline", false);
    var valign = element.GetOptionalPropertyParseable<TextVerticalAlignment>("valign", s => s switch
    {
      "top" => TextVerticalAlignment.Top,
      "center" => TextVerticalAlignment.Center,
      "bottom" => TextVerticalAlignment.Bottom,
      _ => throw new JsonException($"Unknown vertical alignment '{s}'.")
    }, TextVerticalAlignment.Top);
    var wrap = element.GetOptionalProperty<bool>("wrap", false);

    return new TextObject
    {
      Bold = bold,
      Color = color,
      FontFamily = fontfamily,
      HorizontalAlignment = halign,
      Italic = italic,
      Kerning = kerning,
      PixelSize = pixelsize,
      Strikeout = strikeout,
      Text = text,
      Underline = underline,
      VerticalAlignment = valign,
      Wrap = wrap
    };
  }
}
