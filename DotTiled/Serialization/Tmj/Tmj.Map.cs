using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static Map ReadMap(JsonElement element)
  {
    var version = element.GetRequiredProperty<string>("version");
    var tiledVersion = element.GetRequiredProperty<string>("tiledversion");
    string @class = element.GetOptionalProperty<string>("class", "");
    var orientation = element.GetRequiredPropertyParseable<MapOrientation>("orientation", s => s switch
    {
      "orthogonal" => MapOrientation.Orthogonal,
      "isometric" => MapOrientation.Isometric,
      "staggered" => MapOrientation.Staggered,
      "hexagonal" => MapOrientation.Hexagonal,
      _ => throw new JsonException($"Unknown orientation '{s}'")
    });
    var renderOrder = element.GetOptionalPropertyParseable<RenderOrder>("renderorder", s => s switch
    {
      "right-down" => RenderOrder.RightDown,
      "right-up" => RenderOrder.RightUp,
      "left-down" => RenderOrder.LeftDown,
      "left-up" => RenderOrder.LeftUp,
      _ => throw new JsonException($"Unknown render order '{s}'")
    }, RenderOrder.RightDown);
    var compressionLevel = element.GetOptionalProperty<int>("compressionlevel", -1);
    var width = element.GetRequiredProperty<uint>("width");
    var height = element.GetRequiredProperty<uint>("height");
    var tileWidth = element.GetRequiredProperty<uint>("tilewidth");
    var tileHeight = element.GetRequiredProperty<uint>("tileheight");
    var hexSideLength = element.GetOptionalProperty<uint?>("hexsidelength", null);
    var staggerAxis = element.GetOptionalPropertyParseable<StaggerAxis?>("staggeraxis", s => s switch
    {
      "x" => StaggerAxis.X,
      "y" => StaggerAxis.Y,
      _ => throw new JsonException($"Unknown stagger axis '{s}'")
    }, null);
    var staggerIndex = element.GetOptionalPropertyParseable<StaggerIndex?>("staggerindex", s => s switch
    {
      "odd" => StaggerIndex.Odd,
      "even" => StaggerIndex.Even,
      _ => throw new JsonException($"Unknown stagger index '{s}'")
    }, null);
    var parallaxOriginX = element.GetOptionalProperty<float>("parallaxoriginx", 0.0f);
    var parallaxOriginY = element.GetOptionalProperty<float>("parallaxoriginy", 0.0f);
    var backgroundColor = element.GetOptionalPropertyParseable<Color>("backgroundcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), Color.Parse("#00000000", CultureInfo.InvariantCulture));
    var nextLayerID = element.GetRequiredProperty<uint>("nextlayerid");
    var nextObjectID = element.GetRequiredProperty<uint>("nextobjectid");
    var infinite = element.GetOptionalProperty<int>("infinite", 0) == 1;

    // At most one of
    Dictionary<string, IProperty>? properties = element.GetOptionalPropertyCustom<Dictionary<string, IProperty>>("properties", ReadProperties, null);

    // Any number of
    List<BaseLayer> layers = [];
    List<Tileset> tilesets = [];

    return new Map
    {
      Version = version,
      TiledVersion = tiledVersion,
      Class = @class,
      Orientation = orientation,
      RenderOrder = renderOrder,
      CompressionLevel = compressionLevel,
      Width = width,
      Height = height,
      TileWidth = tileWidth,
      TileHeight = tileHeight,
      HexSideLength = hexSideLength,
      StaggerAxis = staggerAxis,
      StaggerIndex = staggerIndex,
      ParallaxOriginX = parallaxOriginX,
      ParallaxOriginY = parallaxOriginY,
      BackgroundColor = backgroundColor,
      NextLayerID = nextLayerID,
      NextObjectID = nextObjectID,
      Infinite = infinite,
      Properties = properties,
      Tilesets = tilesets,
      Layers = layers
    };
  }

  internal static Dictionary<string, IProperty> ReadProperties(JsonElement element)
  {
    var properties = new Dictionary<string, IProperty>();

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
    }).ForEach(p => properties.Add(p.Name, p));

    return properties;
  }

  internal static ClassProperty ReadClassProperty(JsonElement element)
  {
    var name = element.GetRequiredProperty<string>("name");
    var propertyType = element.GetRequiredProperty<string>("propertytype");

    var properties = element.GetRequiredPropertyCustom<Dictionary<string, IProperty>>("properties", ReadProperties);

    return new ClassProperty { Name = name, PropertyType = propertyType, Properties = properties };
  }

  // internal static List<Tileset> ReadTilesets(ref Utf8JsonReader reader)
  // {
  //   var tilesets = new List<Tileset>();

  //   reader.ProcessJsonArray((ref Utf8JsonReader reader) =>
  //   {
  //     var tileset = ReadTileset(ref reader);
  //     tilesets.Add(tileset);
  //   });

  //   return tilesets;
  // }

  // internal static Tileset ReadTileset(ref Utf8JsonReader reader)
  // {
  //   string? version = null;
  //   string? tiledVersion = null;
  //   uint? firstGID = null;
  //   string? source = null;
  //   string? name = null;
  //   string @class = "";
  //   uint? tileWidth = null;
  //   uint? tileHeight = null;
  //   uint? spacing = null;
  //   uint? margin = null;
  //   uint? tileCount = null;
  //   uint? columns = null;
  //   ObjectAlignment objectAlignment = ObjectAlignment.Unspecified;
  //   FillMode fillMode = FillMode.Stretch;

  //   string? image = null;
  //   uint? imageWidth = null;
  //   uint? imageHeight = null;

  //   Dictionary<string, IProperty>? properties = null;

  //   reader.ProcessJsonObject([
  //     new OptionalProperty("version", (ref Utf8JsonReader reader) => version = reader.Progress(reader.GetString())),
  //     new OptionalProperty("tiledversion", (ref Utf8JsonReader reader) => tiledVersion = reader.Progress(reader.GetString())),
  //     new OptionalProperty("firstgid", (ref Utf8JsonReader reader) => firstGID = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("source", (ref Utf8JsonReader reader) => source = reader.Progress(reader.GetString())),
  //     new OptionalProperty("name", (ref Utf8JsonReader reader) => name = reader.Progress(reader.GetString())),
  //     new OptionalProperty("class", (ref Utf8JsonReader reader) => @class = reader.Progress(reader.GetString() ?? ""), allowNull: true),
  //     new OptionalProperty("tilewidth", (ref Utf8JsonReader reader) => tileWidth = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("tileheight", (ref Utf8JsonReader reader) => tileHeight = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("spacing", (ref Utf8JsonReader reader) => spacing = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("margin", (ref Utf8JsonReader reader) => margin = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("tilecount", (ref Utf8JsonReader reader) => tileCount = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("columns", (ref Utf8JsonReader reader) => columns = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("objectalignment", (ref Utf8JsonReader reader) => objectAlignment = reader.Progress(reader.GetString()) switch
  //     {
  //       "unspecified" => ObjectAlignment.Unspecified,
  //       "topleft" => ObjectAlignment.TopLeft,
  //       "top" => ObjectAlignment.Top,
  //       "topright" => ObjectAlignment.TopRight,
  //       "left" => ObjectAlignment.Left,
  //       "center" => ObjectAlignment.Center,
  //       "right" => ObjectAlignment.Right,
  //       "bottomleft" => ObjectAlignment.BottomLeft,
  //       "bottom" => ObjectAlignment.Bottom,
  //       "bottomright" => ObjectAlignment.BottomRight,
  //       _ => throw new JsonException("Invalid object alignment.")
  //     }),
  //     new OptionalProperty("fillmode", (ref Utf8JsonReader reader) => fillMode = reader.Progress(reader.GetString()) switch
  //     {
  //       "stretch" => FillMode.Stretch,
  //       "preserve-aspect-fit" => FillMode.PreserveAspectFit,
  //       _ => throw new JsonException("Invalid fill mode.")
  //     }),

  //     new OptionalProperty("image", (ref Utf8JsonReader reader) => image = reader.Progress(reader.GetString())),
  //     new OptionalProperty("imagewidth", (ref Utf8JsonReader reader) => imageWidth = reader.Progress(reader.GetUInt32())),
  //     new OptionalProperty("imageheight", (ref Utf8JsonReader reader) => imageHeight = reader.Progress(reader.GetUInt32())),

  //     new OptionalProperty("properties", (ref Utf8JsonReader reader) => properties = ReadProperties(ref reader))
  //   ], "tileset");

  //   Image? imageInstance = image is not null ? new Image
  //   {
  //     Format = ParseImageFormatFromSource(image),
  //     Width = imageWidth,
  //     Height = imageHeight,
  //     Source = image
  //   } : null;

  //   return new Tileset
  //   {
  //     Version = version,
  //     TiledVersion = tiledVersion,
  //     FirstGID = firstGID,
  //     Source = source,
  //     Name = name,
  //     Class = @class,
  //     TileWidth = tileWidth,
  //     TileHeight = tileHeight,
  //     Spacing = spacing,
  //     Margin = margin,
  //     TileCount = tileCount,
  //     Columns = columns,
  //     ObjectAlignment = objectAlignment,
  //     FillMode = fillMode,
  //     Image = imageInstance,
  //     Properties = properties
  //   };
  // }

  // private static ImageFormat ParseImageFormatFromSource(string? source)
  // {
  //   if (source is null)
  //     throw new JsonException("Image source is required to determine image format.");

  //   var extension = Path.GetExtension(source);
  //   return extension switch
  //   {
  //     ".png" => ImageFormat.Png,
  //     ".jpg" => ImageFormat.Jpg,
  //     ".jpeg" => ImageFormat.Jpg,
  //     ".gif" => ImageFormat.Gif,
  //     ".bmp" => ImageFormat.Bmp,
  //     _ => throw new JsonException("Invalid image format.")
  //   };
  // }
}
