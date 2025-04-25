using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal Map ReadMap(JsonElement element)
  {
    var version = element.GetRequiredProperty<string>("version");
    var tiledVersion = element.GetRequiredProperty<string>("tiledversion");
    var @class = element.GetOptionalProperty<string>("class").GetValueOr("");
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
    }).GetValueOr(RenderOrder.RightDown);
    var compressionLevel = element.GetOptionalProperty<int>("compressionlevel").GetValueOr(-1);
    var width = element.GetRequiredProperty<int>("width");
    var height = element.GetRequiredProperty<int>("height");
    var tileWidth = element.GetRequiredProperty<int>("tilewidth");
    var tileHeight = element.GetRequiredProperty<int>("tileheight");
    var hexSideLength = element.GetOptionalProperty<int>("hexsidelength");
    var staggerAxis = element.GetOptionalPropertyParseable<StaggerAxis>("staggeraxis", s => s switch
    {
      "x" => StaggerAxis.X,
      "y" => StaggerAxis.Y,
      _ => throw new JsonException($"Unknown stagger axis '{s}'")
    });
    var staggerIndex = element.GetOptionalPropertyParseable<StaggerIndex>("staggerindex", s => s switch
    {
      "odd" => StaggerIndex.Odd,
      "even" => StaggerIndex.Even,
      _ => throw new JsonException($"Unknown stagger index '{s}'")
    });
    var parallaxOriginX = element.GetOptionalProperty<float>("parallaxoriginx").GetValueOr(0f);
    var parallaxOriginY = element.GetOptionalProperty<float>("parallaxoriginy").GetValueOr(0f);
    var backgroundColor = element.GetOptionalPropertyParseable<Color>("backgroundcolor").GetValueOr(Color.Parse("#00000000", CultureInfo.InvariantCulture));
    var nextLayerID = element.GetRequiredProperty<uint>("nextlayerid");
    var nextObjectID = element.GetRequiredProperty<uint>("nextobjectid");
    var infinite = element.GetOptionalProperty<bool>("infinite").GetValueOr(false);

    var properties = ResolveAndMergeProperties(@class, element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));

    List<BaseLayer> layers = element.GetOptionalPropertyCustom<List<BaseLayer>>("layers", e => e.GetValueAsList<BaseLayer>(el => ReadLayer(el))).GetValueOr([]);
    List<Tileset> tilesets = element.GetOptionalPropertyCustom<List<Tileset>>("tilesets", e => e.GetValueAsList<Tileset>(el => ReadTileset(el, version, tiledVersion))).GetValueOr([]);

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
}
