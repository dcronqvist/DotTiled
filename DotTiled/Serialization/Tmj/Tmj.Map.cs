using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static Map ReadMap(ref Utf8JsonReader reader)
  {
    string version = default!;
    string tiledVersion = default!;
    string @class = "";
    MapOrientation orientation = default;
    RenderOrder renderOrder = RenderOrder.RightDown;
    int compressionLevel = -1;
    uint width = 0;
    uint height = 0;
    uint tileWidth = 0;
    uint tileHeight = 0;
    uint? hexSideLength = null;
    StaggerAxis? staggerAxis = null;
    StaggerIndex? staggerIndex = null;
    float parallaxOriginX = 0.0f;
    float parallaxOriginY = 0.0f;
    Color backgroundColor = Color.Parse("#00000000", CultureInfo.InvariantCulture);
    uint nextLayerID = 0;
    uint nextObjectID = 0;
    bool infinite = false;

    // At most one of
    Dictionary<string, IProperty>? properties = null;

    // Any number of
    List<BaseLayer> layers = [];
    List<Tileset> tilesets = [];

    reader.ProcessJsonObject([
      new RequiredProperty("version", (ref Utf8JsonReader reader) => version = reader.Progress(reader.GetString()!)),
      new RequiredProperty("tiledversion", (ref Utf8JsonReader reader) => tiledVersion = reader.Progress(reader.GetString()!)),
      new OptionalProperty("class", (ref Utf8JsonReader reader) => @class = reader.Progress(reader.GetString() ?? ""), allowNull: true),
      new RequiredProperty("orientation", (ref Utf8JsonReader reader) => orientation = reader.Progress(reader.GetString()) switch
      {
        "orthogonal" => MapOrientation.Orthogonal,
        "isometric" => MapOrientation.Isometric,
        "staggered" => MapOrientation.Staggered,
        "hexagonal" => MapOrientation.Hexagonal,
        _ => throw new JsonException("Invalid orientation.")
      }),
      new OptionalProperty("renderorder", (ref Utf8JsonReader reader) => renderOrder = reader.Progress(reader.GetString()) switch
      {
        "right-down" => RenderOrder.RightDown,
        "right-up" => RenderOrder.RightUp,
        "left-down" => RenderOrder.LeftDown,
        "left-up" => RenderOrder.LeftUp,
        _ => throw new JsonException("Invalid render order.")
      }),
      new OptionalProperty("compressionlevel", (ref Utf8JsonReader reader) => compressionLevel = reader.Progress(reader.GetInt32())),
      new RequiredProperty("width", (ref Utf8JsonReader reader) => width = reader.Progress(reader.GetUInt32())),
      new RequiredProperty("height", (ref Utf8JsonReader reader) => height = reader.Progress(reader.GetUInt32())),
      new RequiredProperty("tilewidth", (ref Utf8JsonReader reader) => tileWidth = reader.Progress(reader.GetUInt32())),
      new RequiredProperty("tileheight", (ref Utf8JsonReader reader) => tileHeight = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("hexsidelength", (ref Utf8JsonReader reader) => hexSideLength = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("staggeraxis", (ref Utf8JsonReader reader) => staggerAxis = reader.Progress(reader.GetString()) switch
      {
        "x" => StaggerAxis.X,
        "y" => StaggerAxis.Y,
        _ => throw new JsonException("Invalid stagger axis.")
      }),
      new OptionalProperty("staggerindex", (ref Utf8JsonReader reader) => staggerIndex = reader.Progress(reader.GetString()) switch
      {
        "odd" => StaggerIndex.Odd,
        "even" => StaggerIndex.Even,
        _ => throw new JsonException("Invalid stagger index.")
      }),
      new OptionalProperty("parallaxoriginx", (ref Utf8JsonReader reader) => parallaxOriginX = reader.Progress(reader.GetSingle())),
      new OptionalProperty("parallaxoriginy", (ref Utf8JsonReader reader) => parallaxOriginY = reader.Progress(reader.GetSingle())),
      new OptionalProperty("backgroundcolor", (ref Utf8JsonReader reader) => backgroundColor = Color.Parse(reader.Progress(reader.GetString()!), CultureInfo.InvariantCulture)),
      new RequiredProperty("nextlayerid", (ref Utf8JsonReader reader) => nextLayerID = reader.Progress(reader.GetUInt32())),
      new RequiredProperty("nextobjectid", (ref Utf8JsonReader reader) => nextObjectID = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("infinite", (ref Utf8JsonReader reader) => infinite = reader.Progress(reader.GetUInt32()) == 1),

      new OptionalProperty("properties", (ref Utf8JsonReader reader) => properties = ReadProperties(ref reader)),
      new OptionalProperty("tilesets", (ref Utf8JsonReader reader) => tilesets = ReadTilesets(ref reader))
    ], "map");

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

  internal static Dictionary<string, IProperty> ReadProperties(ref Utf8JsonReader reader)
  {
    var properties = new Dictionary<string, IProperty>();

    reader.ProcessJsonArray((ref Utf8JsonReader reader) =>
    {
      var property = ReadProperty(ref reader);
      properties.Add(property.Name, property);
    });

    return properties;
  }

  internal static IProperty ReadProperty(ref Utf8JsonReader reader)
  {
    string name = default!;
    string type = default!;
    IProperty property = null;

    reader.ProcessJsonObject([
      new RequiredProperty("name", (ref Utf8JsonReader reader) => name = reader.Progress(reader.GetString()!)),
      new RequiredProperty("type", (ref Utf8JsonReader reader) => type = reader.Progress(reader.GetString()!)),
      new RequiredProperty("value", (ref Utf8JsonReader reader) =>
      {
        property = type switch
        {
          "string" => new StringProperty { Name = name, Value = reader.Progress(reader.GetString()!) },
          "int" => new IntProperty { Name = name, Value = reader.Progress(reader.GetInt32()) },
          "float" => new FloatProperty { Name = name, Value = reader.Progress(reader.GetSingle()) },
          "bool" => new BoolProperty { Name = name, Value = reader.Progress(reader.GetBoolean()) },
          "color" => new ColorProperty { Name = name, Value = Color.Parse(reader.Progress(reader.GetString()!), CultureInfo.InvariantCulture) },
          "file" => new FileProperty { Name = name, Value = reader.Progress(reader.GetString()!) },
          "object" => new ObjectProperty { Name = name, Value = reader.Progress(reader.GetUInt32()) },
          // "class" => ReadClassProperty(ref reader),
          _ => throw new JsonException("Invalid property type.")
        };
      }),
    ], "property");

    return property!;
  }

  internal static List<Tileset> ReadTilesets(ref Utf8JsonReader reader)
  {
    var tilesets = new List<Tileset>();

    reader.ProcessJsonArray((ref Utf8JsonReader reader) =>
    {
      var tileset = ReadTileset(ref reader);
      tilesets.Add(tileset);
    });

    return tilesets;
  }

  internal static Tileset ReadTileset(ref Utf8JsonReader reader)
  {
    string? version = null;
    string? tiledVersion = null;
    uint? firstGID = null;
    string? source = null;
    string? name = null;
    string @class = "";
    uint? tileWidth = null;
    uint? tileHeight = null;
    uint? spacing = null;
    uint? margin = null;
    uint? tileCount = null;
    uint? columns = null;
    ObjectAlignment objectAlignment = ObjectAlignment.Unspecified;
    FillMode fillMode = FillMode.Stretch;

    string? image = null;
    uint? imageWidth = null;
    uint? imageHeight = null;

    Dictionary<string, IProperty>? properties = null;

    reader.ProcessJsonObject([
      new OptionalProperty("version", (ref Utf8JsonReader reader) => version = reader.Progress(reader.GetString())),
      new OptionalProperty("tiledversion", (ref Utf8JsonReader reader) => tiledVersion = reader.Progress(reader.GetString())),
      new OptionalProperty("firstgid", (ref Utf8JsonReader reader) => firstGID = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("source", (ref Utf8JsonReader reader) => source = reader.Progress(reader.GetString())),
      new OptionalProperty("name", (ref Utf8JsonReader reader) => name = reader.Progress(reader.GetString())),
      new OptionalProperty("class", (ref Utf8JsonReader reader) => @class = reader.Progress(reader.GetString() ?? ""), allowNull: true),
      new OptionalProperty("tilewidth", (ref Utf8JsonReader reader) => tileWidth = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("tileheight", (ref Utf8JsonReader reader) => tileHeight = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("spacing", (ref Utf8JsonReader reader) => spacing = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("margin", (ref Utf8JsonReader reader) => margin = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("tilecount", (ref Utf8JsonReader reader) => tileCount = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("columns", (ref Utf8JsonReader reader) => columns = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("objectalignment", (ref Utf8JsonReader reader) => objectAlignment = reader.Progress(reader.GetString()) switch
      {
        "unspecified" => ObjectAlignment.Unspecified,
        "topleft" => ObjectAlignment.TopLeft,
        "top" => ObjectAlignment.Top,
        "topright" => ObjectAlignment.TopRight,
        "left" => ObjectAlignment.Left,
        "center" => ObjectAlignment.Center,
        "right" => ObjectAlignment.Right,
        "bottomleft" => ObjectAlignment.BottomLeft,
        "bottom" => ObjectAlignment.Bottom,
        "bottomright" => ObjectAlignment.BottomRight,
        _ => throw new JsonException("Invalid object alignment.")
      }),
      new OptionalProperty("fillmode", (ref Utf8JsonReader reader) => fillMode = reader.Progress(reader.GetString()) switch
      {
        "stretch" => FillMode.Stretch,
        "preserve-aspect-fit" => FillMode.PreserveAspectFit,
        _ => throw new JsonException("Invalid fill mode.")
      }),

      new OptionalProperty("image", (ref Utf8JsonReader reader) => image = reader.Progress(reader.GetString())),
      new OptionalProperty("imagewidth", (ref Utf8JsonReader reader) => imageWidth = reader.Progress(reader.GetUInt32())),
      new OptionalProperty("imageheight", (ref Utf8JsonReader reader) => imageHeight = reader.Progress(reader.GetUInt32())),

      new OptionalProperty("properties", (ref Utf8JsonReader reader) => properties = ReadProperties(ref reader))
    ], "tileset");

    Image? imageInstance = image is not null ? new Image
    {
      Format = ParseImageFormatFromSource(image),
      Width = imageWidth,
      Height = imageHeight,
      Source = image
    } : null;

    return new Tileset
    {
      Version = version,
      TiledVersion = tiledVersion,
      FirstGID = firstGID,
      Source = source,
      Name = name,
      Class = @class,
      TileWidth = tileWidth,
      TileHeight = tileHeight,
      Spacing = spacing,
      Margin = margin,
      TileCount = tileCount,
      Columns = columns,
      ObjectAlignment = objectAlignment,
      FillMode = fillMode,
      Image = imageInstance,
      Properties = properties
    };
  }

  private static ImageFormat ParseImageFormatFromSource(string? source)
  {
    if (source is null)
      throw new JsonException("Image source is required to determine image format.");

    var extension = Path.GetExtension(source);
    return extension switch
    {
      ".png" => ImageFormat.Png,
      ".jpg" => ImageFormat.Jpg,
      ".jpeg" => ImageFormat.Jpg,
      ".gif" => ImageFormat.Gif,
      ".bmp" => ImageFormat.Bmp,
      _ => throw new JsonException("Invalid image format.")
    };
  }
}
