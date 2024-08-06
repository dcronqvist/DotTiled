using System.Globalization;
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

    reader.ProcessJsonObject([
      new RequiredProperty<string>("version", value => version = value),
      new RequiredProperty<string>("tiledVersion", value => tiledVersion = value),
      new OptionalProperty<string>("class", value => @class = value ?? ""),
      new RequiredProperty<string>("orientation", value => orientation = value switch
      {
        "orthogonal" => MapOrientation.Orthogonal,
        "isometric" => MapOrientation.Isometric,
        "staggered" => MapOrientation.Staggered,
        "hexagonal" => MapOrientation.Hexagonal,
        _ => throw new JsonException("Invalid orientation.")
      }),
      new OptionalProperty<string>("renderOrder", value => renderOrder = value switch
      {
        "right-down" => RenderOrder.RightDown,
        "right-up" => RenderOrder.RightUp,
        "left-down" => RenderOrder.LeftDown,
        "left-up" => RenderOrder.LeftUp,
        _ => throw new JsonException("Invalid render order.")
      }),
      new OptionalProperty<int>("compressionLevel", value => compressionLevel = value),
      new RequiredProperty<uint>("width", value => width = value),
      new RequiredProperty<uint>("height", value => height = value),
      new RequiredProperty<uint>("tileWidth", value => tileWidth = value),
      new RequiredProperty<uint>("tileHeight", value => tileHeight = value),
      new OptionalProperty<uint>("hexSideLength", value => hexSideLength = value),
      new OptionalProperty<string>("staggerAxis", value => staggerAxis = value switch
      {
        "x" => StaggerAxis.X,
        "y" => StaggerAxis.Y,
        _ => throw new JsonException("Invalid stagger axis.")
      }),
      new OptionalProperty<string>("staggerIndex", value => staggerIndex = value switch
      {
        "odd" => StaggerIndex.Odd,
        "even" => StaggerIndex.Even,
        _ => throw new JsonException("Invalid stagger index.")
      }),
      new OptionalProperty<float>("parallaxOriginX", value => parallaxOriginX = value),
      new OptionalProperty<float>("parallaxOriginY", value => parallaxOriginY = value),
      new OptionalProperty<string>("backgroundColor", value => backgroundColor = Color.Parse(value!, CultureInfo.InvariantCulture)),
      new RequiredProperty<uint>("nextLayerID", value => nextLayerID = value),
      new RequiredProperty<uint>("nextObjectID", value => nextObjectID = value),
      new OptionalProperty<uint>("infinite", value => infinite = value == 1)
    ]);

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
      //Properties = properties,
      //Tilesets = tilesets,
      //Layers = layers
    };
  }
}
