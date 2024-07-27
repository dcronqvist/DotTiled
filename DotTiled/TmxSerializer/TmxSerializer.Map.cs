using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace DotTiled;

public partial class TmxSerializer
{
  private Map ReadMap(XmlReader reader)
  {
    // Attributes
    var version = reader.GetRequiredAttribute("version");
    var tiledVersion = reader.GetRequiredAttribute("tiledversion");
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var orientation = reader.GetRequiredAttributeEnum<MapOrientation>("orientation", s => s switch
    {
      "orthogonal" => MapOrientation.Orthogonal,
      "isometric" => MapOrientation.Isometric,
      "staggered" => MapOrientation.Staggered,
      "hexagonal" => MapOrientation.Hexagonal,
      _ => throw new Exception($"Unknown orientation '{s}'")
    });
    var renderOrder = reader.GetOptionalAttributeEnum<RenderOrder>("renderorder", s => s switch
    {
      "right-down" => RenderOrder.RightDown,
      "right-up" => RenderOrder.RightUp,
      "left-down" => RenderOrder.LeftDown,
      "left-up" => RenderOrder.LeftUp,
      _ => throw new Exception($"Unknown render order '{s}'")
    }) ?? RenderOrder.RightDown;
    var compressionLevel = reader.GetOptionalAttributeParseable<int>("compressionlevel") ?? -1;
    var width = reader.GetRequiredAttributeParseable<uint>("width");
    var height = reader.GetRequiredAttributeParseable<uint>("height");
    var tileWidth = reader.GetRequiredAttributeParseable<uint>("tilewidth");
    var tileHeight = reader.GetRequiredAttributeParseable<uint>("tileheight");
    var hexSideLength = reader.GetOptionalAttributeParseable<uint>("hexsidelength");
    var staggerAxis = reader.GetOptionalAttributeEnum<StaggerAxis>("staggeraxis", s => s switch
    {
      "x" => StaggerAxis.X,
      "y" => StaggerAxis.Y,
      _ => throw new Exception($"Unknown stagger axis '{s}'")
    });
    var staggerIndex = reader.GetOptionalAttributeEnum<StaggerIndex>("staggerindex", s => s switch
    {
      "odd" => StaggerIndex.Odd,
      "even" => StaggerIndex.Even,
      _ => throw new Exception($"Unknown stagger index '{s}'")
    });
    var parallaxOriginX = reader.GetOptionalAttributeParseable<float>("parallaxoriginx") ?? 0.0f;
    var parallaxOriginY = reader.GetOptionalAttributeParseable<float>("parallaxoriginy") ?? 0.0f;
    var backgroundColor = reader.GetOptionalAttributeClass<Color>("backgroundcolor") ?? Color.Parse("#00000000", CultureInfo.InvariantCulture);
    var nextLayerID = reader.GetRequiredAttributeParseable<uint>("nextlayerid");
    var nextObjectID = reader.GetRequiredAttributeParseable<uint>("nextobjectid");
    var infinite = (reader.GetOptionalAttributeParseable<uint>("infinite") ?? 0) == 1;

    // At most one of
    Dictionary<string, IProperty>? properties = null;

    // Any number of
    List<BaseLayer> layers = [];
    List<Tileset> tilesets = [];

    reader.ProcessChildren("map", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r), "Properties"),
      "tileset" => () => tilesets.Add(ReadTileset(r)),
      "layer" => () => layers.Add(ReadTileLayer(r, dataUsesChunks: infinite)),
      "objectgroup" => () => layers.Add(ReadObjectLayer(r)),
      "imagelayer" => () => layers.Add(ReadImageLayer(r)),
      "group" => () => layers.Add(ReadGroup(r)),
      _ => r.Skip
    });

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
