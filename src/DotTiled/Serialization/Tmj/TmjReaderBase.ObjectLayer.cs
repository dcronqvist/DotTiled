using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal ObjectLayer ReadObjectLayer(JsonElement element)
  {
    var id = element.GetRequiredProperty<uint>("id");
    var name = element.GetRequiredProperty<string>("name");
    var @class = element.GetOptionalProperty<string>("class").GetValueOr("");
    var opacity = element.GetOptionalProperty<float>("opacity").GetValueOr(1.0f);
    var visible = element.GetOptionalProperty<bool>("visible").GetValueOr(true);
    var tintColor = element.GetOptionalPropertyParseable<TiledColor>("tintcolor");
    var offsetX = element.GetOptionalProperty<float>("offsetx").GetValueOr(0.0f);
    var offsetY = element.GetOptionalProperty<float>("offsety").GetValueOr(0.0f);
    var parallaxX = element.GetOptionalProperty<float>("parallaxx").GetValueOr(1.0f);
    var parallaxY = element.GetOptionalProperty<float>("parallaxy").GetValueOr(1.0f);
    var properties = ResolveAndMergeProperties(@class, element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));

    var x = element.GetOptionalProperty<int>("x").GetValueOr(0);
    var y = element.GetOptionalProperty<int>("y").GetValueOr(0);
    var width = element.GetOptionalProperty<int>("width").GetValueOr(0);
    var height = element.GetOptionalProperty<int>("height").GetValueOr(0);
    var color = element.GetOptionalPropertyParseable<TiledColor>("color");
    var drawOrder = element.GetOptionalPropertyParseable<DrawOrder>("draworder", s => s switch
    {
      "topdown" => DrawOrder.TopDown,
      "index" => DrawOrder.Index,
      _ => throw new JsonException($"Unknown draw order '{s}'.")
    }).GetValueOr(DrawOrder.TopDown);

    var objects = element.GetOptionalPropertyCustom<List<DotTiled.Object>>("objects", e => e.GetValueAsList<DotTiled.Object>(el => ReadObject(el))).GetValueOr([]);

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

  internal DotTiled.Object ReadObject(JsonElement element)
  {
    Optional<uint> idDefault = Optional.Empty;
    string nameDefault = "";
    string typeDefault = "";
    float xDefault = 0f;
    float yDefault = 0f;
    float widthDefault = 0f;
    float heightDefault = 0f;
    float rotationDefault = 0f;
    bool visibleDefault = true;
    bool ellipseDefault = false;
    bool pointDefault = false;

    List<Vector2> polygonDefault = null;
    List<Vector2> polylineDefault = null;
    List<IProperty> propertiesDefault = [];

    var template = element.GetOptionalProperty<string>("template");
    if (template.HasValue)
    {
      var resolvedTemplate = _externalTemplateResolver(template.Value);
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

    var ellipse = element.GetOptionalProperty<bool>("ellipse").GetValueOr(ellipseDefault);
    var gid = element.GetOptionalProperty<uint>("gid");
    var height = element.GetOptionalProperty<float>("height").GetValueOr(heightDefault);
    var id = element.GetOptionalProperty<uint>("id").GetValueOrOptional(idDefault);
    var name = element.GetOptionalProperty<string>("name").GetValueOr(nameDefault);
    var point = element.GetOptionalProperty<bool>("point").GetValueOr(pointDefault);
    var polygon = element.GetOptionalPropertyCustom<List<Vector2>>("polygon", ReadPoints).GetValueOr(polygonDefault);
    var polyline = element.GetOptionalPropertyCustom<List<Vector2>>("polyline", ReadPoints).GetValueOr(polylineDefault);
    var properties = element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr(propertiesDefault);
    var rotation = element.GetOptionalProperty<float>("rotation").GetValueOr(rotationDefault);
    var text = element.GetOptionalPropertyCustom<TextObject>("text", ReadText);
    var type = element.GetOptionalProperty<string>("type").GetValueOr(typeDefault);
    var visible = element.GetOptionalProperty<bool>("visible").GetValueOr(visibleDefault);
    var width = element.GetOptionalProperty<float>("width").GetValueOr(widthDefault);
    var x = element.GetOptionalProperty<float>("x").GetValueOr(xDefault);
    var y = element.GetOptionalProperty<float>("y").GetValueOr(yDefault);

    if (gid.HasValue)
    {
      var (clearedGIDs, flippingFlags) = Helpers.ReadAndClearFlippingFlagsFromGIDs([gid.Value]);

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
        GID = clearedGIDs.Single(),
        FlippingFlags = flippingFlags.Single()
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

    if (text.HasValue)
    {
      text.Value.ID = id;
      text.Value.Name = name;
      text.Value.Type = type;
      text.Value.X = x;
      text.Value.Y = y;
      text.Value.Width = width;
      text.Value.Height = height;
      text.Value.Rotation = rotation;
      text.Value.Visible = visible;
      text.Value.Template = template;
      text.Value.Properties = properties;
      return text.Value;
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
    var bold = element.GetOptionalProperty<bool>("bold").GetValueOr(false);
    var color = element.GetOptionalPropertyParseable<TiledColor>("color").GetValueOr(TiledColor.Parse("#000000", CultureInfo.InvariantCulture));
    var fontfamily = element.GetOptionalProperty<string>("fontfamily").GetValueOr("sans-serif");
    var halign = element.GetOptionalPropertyParseable<TextHorizontalAlignment>("halign", s => s switch
    {
      "left" => TextHorizontalAlignment.Left,
      "center" => TextHorizontalAlignment.Center,
      "right" => TextHorizontalAlignment.Right,
      _ => throw new JsonException($"Unknown horizontal alignment '{s}'.")
    }).GetValueOr(TextHorizontalAlignment.Left);
    var italic = element.GetOptionalProperty<bool>("italic").GetValueOr(false);
    var kerning = element.GetOptionalProperty<bool>("kerning").GetValueOr(true);
    var pixelsize = element.GetOptionalProperty<int>("pixelsize").GetValueOr(16);
    var strikeout = element.GetOptionalProperty<bool>("strikeout").GetValueOr(false);
    var text = element.GetRequiredProperty<string>("text");
    var underline = element.GetOptionalProperty<bool>("underline").GetValueOr(false);
    var valign = element.GetOptionalPropertyParseable<TextVerticalAlignment>("valign", s => s switch
    {
      "top" => TextVerticalAlignment.Top,
      "center" => TextVerticalAlignment.Center,
      "bottom" => TextVerticalAlignment.Bottom,
      _ => throw new JsonException($"Unknown vertical alignment '{s}'.")
    }).GetValueOr(TextVerticalAlignment.Top);
    var wrap = element.GetOptionalProperty<bool>("wrap").GetValueOr(false);

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
