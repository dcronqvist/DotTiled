using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

internal partial class Tmj
{
  internal static Map ReadMap(
    JsonElement element,
    Func<string, Tileset>? externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
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
    var infinite = element.GetOptionalProperty<bool>("infinite", false);

    var properties = element.GetOptionalPropertyCustom<List<IProperty>?>("properties", el => ReadPropertiesList(el, customTypeDefinitions), null);

    List<BaseLayer> layers = element.GetOptionalPropertyCustom<List<BaseLayer>>("layers", e => e.GetValueAsList<BaseLayer>(el => ReadLayer(el, externalTemplateResolver, customTypeDefinitions)), []);
    List<Tileset> tilesets = element.GetOptionalPropertyCustom<List<Tileset>>("tilesets", e => e.GetValueAsList<Tileset>(el => ReadTileset(el, externalTilesetResolver, externalTemplateResolver, customTypeDefinitions)), []);

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
      Properties = properties ?? [],
      Tilesets = tilesets,
      Layers = layers
    };
  }
}
