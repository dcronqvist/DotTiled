using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
    var tiledVersion = _reader.GetOptionalAttribute("tiledversion");
    var @class = _reader.GetOptionalAttribute("class").GetValueOr("");
    var orientation = _reader.GetRequiredAttributeEnum<MapOrientation>("orientation", Helpers.CreateMapper<MapOrientation>(
      s => throw new InvalidOperationException($"Unknown orientation '{s}'"),
      ("orthogonal", MapOrientation.Orthogonal),
      ("isometric", MapOrientation.Isometric),
      ("staggered", MapOrientation.Staggered),
      ("hexagonal", MapOrientation.Hexagonal)
    ));
    var renderOrder = _reader.GetOptionalAttributeEnum<RenderOrder>("renderorder", s => s switch
    {
      "right-down" => RenderOrder.RightDown,
      "right-up" => RenderOrder.RightUp,
      "left-down" => RenderOrder.LeftDown,
      "left-up" => RenderOrder.LeftUp,
      _ => throw new InvalidOperationException($"Unknown render order '{s}'")
    }).GetValueOr(RenderOrder.RightDown);
    var compressionLevel = _reader.GetOptionalAttributeParseable<int>("compressionlevel").GetValueOr(-1);
    var width = _reader.GetRequiredAttributeParseable<int>("width");
    var height = _reader.GetRequiredAttributeParseable<int>("height");
    var tileWidth = _reader.GetRequiredAttributeParseable<int>("tilewidth");
    var tileHeight = _reader.GetRequiredAttributeParseable<int>("tileheight");
    var hexSideLength = _reader.GetOptionalAttributeParseable<int>("hexsidelength");
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
    var parallaxOriginX = _reader.GetOptionalAttributeParseable<float>("parallaxoriginx").GetValueOr(0.0f);
    var parallaxOriginY = _reader.GetOptionalAttributeParseable<float>("parallaxoriginy").GetValueOr(0.0f);
    var backgroundColor = _reader.GetOptionalAttributeClass<Color>("backgroundcolor").GetValueOr(Color.Parse("#00000000", CultureInfo.InvariantCulture));
    var nextLayerID = _reader.GetRequiredAttributeParseable<uint>("nextlayerid");
    var nextObjectID = _reader.GetRequiredAttributeParseable<uint>("nextobjectid");
    var infinite = _reader.GetOptionalAttributeParseable<uint>("infinite").GetValueOr(0) == 1;

    // At most one of
    var propertiesCounter = 0;
    List<IProperty> properties = Helpers.ResolveClassProperties(@class, _customTypeResolver);

    // Any number of
    List<BaseLayer> layers = [];
    List<Tileset> tilesets = [];

    _reader.ProcessChildren("map", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnceUsingCounter(ref properties, Helpers.MergeProperties(properties, ReadProperties()).ToList(), "Properties", ref propertiesCounter),
      "tileset" => () => tilesets.Add(ReadTileset(version, tiledVersion)),
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
