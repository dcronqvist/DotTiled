using System;
using System.Collections.Generic;
using System.Globalization;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

/// <summary>
/// Base class for Tiled XML format readers.
/// </summary>
public abstract partial class TmxReaderBase
{
  internal Map ReadMap()
  {
    // Attributes
    var version = _reader.GetRequiredAttribute("version");
    var tiledVersion = _reader.GetRequiredAttribute("tiledversion");
    var @class = _reader.GetOptionalAttribute("class") ?? "";
    var orientation = _reader.GetRequiredAttributeEnum<MapOrientation>("orientation", s => s switch
    {
      "orthogonal" => MapOrientation.Orthogonal,
      "isometric" => MapOrientation.Isometric,
      "staggered" => MapOrientation.Staggered,
      "hexagonal" => MapOrientation.Hexagonal,
      _ => throw new InvalidOperationException($"Unknown orientation '{s}'")
    });
    var renderOrder = _reader.GetOptionalAttributeEnum<RenderOrder>("renderorder", s => s switch
    {
      "right-down" => RenderOrder.RightDown,
      "right-up" => RenderOrder.RightUp,
      "left-down" => RenderOrder.LeftDown,
      "left-up" => RenderOrder.LeftUp,
      _ => throw new InvalidOperationException($"Unknown render order '{s}'")
    }) ?? RenderOrder.RightDown;
    var compressionLevel = _reader.GetOptionalAttributeParseable<int>("compressionlevel") ?? -1;
    var width = _reader.GetRequiredAttributeParseable<uint>("width");
    var height = _reader.GetRequiredAttributeParseable<uint>("height");
    var tileWidth = _reader.GetRequiredAttributeParseable<uint>("tilewidth");
    var tileHeight = _reader.GetRequiredAttributeParseable<uint>("tileheight");
    var hexSideLength = _reader.GetOptionalAttributeParseable<uint>("hexsidelength");
    var staggerAxis = _reader.GetOptionalAttributeEnum<StaggerAxis>("staggeraxis", s => s switch
    {
      "x" => StaggerAxis.X,
      "y" => StaggerAxis.Y,
      _ => throw new InvalidOperationException($"Unknown stagger axis '{s}'")
    });
    var staggerIndex = _reader.GetOptionalAttributeEnum<StaggerIndex>("staggerindex", s => s switch
    {
      "odd" => StaggerIndex.Odd,
      "even" => StaggerIndex.Even,
      _ => throw new InvalidOperationException($"Unknown stagger index '{s}'")
    });
    var parallaxOriginX = _reader.GetOptionalAttributeParseable<float>("parallaxoriginx") ?? 0.0f;
    var parallaxOriginY = _reader.GetOptionalAttributeParseable<float>("parallaxoriginy") ?? 0.0f;
    var backgroundColor = _reader.GetOptionalAttributeClass<Color>("backgroundcolor") ?? Color.Parse("#00000000", CultureInfo.InvariantCulture);
    var nextLayerID = _reader.GetRequiredAttributeParseable<uint>("nextlayerid");
    var nextObjectID = _reader.GetRequiredAttributeParseable<uint>("nextobjectid");
    var infinite = (_reader.GetOptionalAttributeParseable<uint>("infinite") ?? 0) == 1;

    // At most one of
    List<IProperty>? properties = null;

    // Any number of
    List<BaseLayer> layers = [];
    List<Tileset> tilesets = [];

    _reader.ProcessChildren("map", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      "tileset" => () => tilesets.Add(ReadTileset()),
      "layer" => () => layers.Add(ReadTileLayer(infinite)),
      "objectgroup" => () => layers.Add(ReadObjectLayer()),
      "imagelayer" => () => layers.Add(ReadImageLayer()),
      "group" => () => layers.Add(ReadGroup()),
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
      Properties = properties ?? [],
      Tilesets = tilesets,
      Layers = layers
    };
  }
}
