using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DotTiled.Serialization.Tmx;

public abstract partial class TmxReaderBase
{
  internal Tileset ReadTileset(
    Optional<string> parentVersion = null,
    Optional<string> parentTiledVersion = null)
  {
    var firstGID = _reader.GetOptionalAttributeParseable<uint>("firstgid");
    var source = _reader.GetOptionalAttribute("source");

    // Check if external tileset
    if (source.HasValue && firstGID.HasValue)
    {
      // Is external tileset
      var externalTileset = _externalTilesetResolver(source);
      externalTileset.FirstGID = firstGID;
      externalTileset.Source = source;

      _reader.ProcessChildren("tileset", (r, elementName) => r.Skip);
      return externalTileset;
    }

    // Attributes
    var version = _reader.GetOptionalAttribute("version").GetValueOrOptional(parentVersion);
    var tiledVersion = _reader.GetOptionalAttribute("tiledversion").GetValueOrOptional(parentTiledVersion);
    var name = _reader.GetRequiredAttribute("name");
    var @class = _reader.GetOptionalAttribute("class").GetValueOr("");
    var tileWidth = _reader.GetRequiredAttributeParseable<uint>("tilewidth");
    var tileHeight = _reader.GetRequiredAttributeParseable<uint>("tileheight");
    var spacing = _reader.GetOptionalAttributeParseable<uint>("spacing").GetValueOr(0);
    var margin = _reader.GetOptionalAttributeParseable<uint>("margin").GetValueOr(0);
    var tileCount = _reader.GetRequiredAttributeParseable<uint>("tilecount");
    var columns = _reader.GetRequiredAttributeParseable<uint>("columns");
    var objectAlignment = _reader.GetOptionalAttributeEnum<ObjectAlignment>("objectalignment", s => s switch
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
      _ => throw new InvalidOperationException($"Unknown object alignment '{s}'")
    }).GetValueOr(ObjectAlignment.Unspecified);
    var renderSize = _reader.GetOptionalAttributeEnum<TileRenderSize>("rendersize", s => s switch
    {
      "tile" => TileRenderSize.Tile,
      "grid" => TileRenderSize.Grid,
      _ => throw new InvalidOperationException($"Unknown render size '{s}'")
    }).GetValueOr(TileRenderSize.Tile);
    var fillMode = _reader.GetOptionalAttributeEnum<FillMode>("fillmode", s => s switch
    {
      "stretch" => FillMode.Stretch,
      "preserve-aspect-fit" => FillMode.PreserveAspectFit,
      _ => throw new InvalidOperationException($"Unknown fill mode '{s}'")
    }).GetValueOr(FillMode.Stretch);

    // Elements
    Image image = null;
    TileOffset tileOffset = null;
    Grid grid = null;
    List<IProperty> properties = null;
    List<Wangset> wangsets = null;
    Transformations transformations = null;
    List<Tile> tiles = [];

    _reader.ProcessChildren("tileset", (r, elementName) => elementName switch
    {
      "image" => () => Helpers.SetAtMostOnce(ref image, ReadImage(), "Image"),
      "tileoffset" => () => Helpers.SetAtMostOnce(ref tileOffset, ReadTileOffset(), "TileOffset"),
      "grid" => () => Helpers.SetAtMostOnce(ref grid, ReadGrid(), "Grid"),
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      "wangsets" => () => Helpers.SetAtMostOnce(ref wangsets, ReadWangsets(), "Wangsets"),
      "transformations" => () => Helpers.SetAtMostOnce(ref transformations, ReadTransformations(), "Transformations"),
      "tile" => () => tiles.Add(ReadTile()),
      _ => r.Skip
    });

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
      RenderSize = renderSize,
      FillMode = fillMode,
      Image = image,
      TileOffset = tileOffset,
      Grid = grid,
      Properties = properties ?? [],
      Wangsets = wangsets ?? [],
      Transformations = transformations,
      Tiles = tiles ?? []
    };
  }

  internal Image ReadImage()
  {
    // Attributes
    var format = _reader.GetOptionalAttributeEnum<ImageFormat>("format", s => s switch
    {
      "png" => ImageFormat.Png,
      "jpg" => ImageFormat.Jpg,
      "bmp" => ImageFormat.Bmp,
      "gif" => ImageFormat.Gif,
      _ => throw new InvalidOperationException($"Unknown image format '{s}'")
    });
    var source = _reader.GetOptionalAttribute("source");
    var transparentColor = _reader.GetOptionalAttributeClass<Color>("trans");
    var width = _reader.GetOptionalAttributeParseable<uint>("width");
    var height = _reader.GetOptionalAttributeParseable<uint>("height");

    _reader.ProcessChildren("image", (r, elementName) => elementName switch
    {
      "data" => throw new NotSupportedException("Embedded image data is not supported."),
      _ => r.Skip
    });

    if (!format.HasValue && source.HasValue)
      format = Helpers.ParseImageFormatFromSource(source);

    return new Image
    {
      Format = format,
      Source = source,
      TransparentColor = transparentColor,
      Width = width,
      Height = height,
    };
  }

  internal TileOffset ReadTileOffset()
  {
    // Attributes
    var x = _reader.GetOptionalAttributeParseable<float>("x").GetValueOr(0f);
    var y = _reader.GetOptionalAttributeParseable<float>("y").GetValueOr(0f);

    _reader.ReadStartElement("tileoffset");
    return new TileOffset { X = x, Y = y };
  }

  internal Grid ReadGrid()
  {
    // Attributes
    var orientation = _reader.GetOptionalAttributeEnum<GridOrientation>("orientation", s => s switch
    {
      "orthogonal" => GridOrientation.Orthogonal,
      "isometric" => GridOrientation.Isometric,
      _ => throw new InvalidOperationException($"Unknown orientation '{s}'")
    }).GetValueOr(GridOrientation.Orthogonal);
    var width = _reader.GetRequiredAttributeParseable<uint>("width");
    var height = _reader.GetRequiredAttributeParseable<uint>("height");

    _reader.ReadStartElement("grid");
    return new Grid { Orientation = orientation, Width = width, Height = height };
  }

  internal Transformations ReadTransformations()
  {
    // Attributes
    var hFlip = _reader.GetOptionalAttributeParseable<uint>("hflip").GetValueOr(0) == 1;
    var vFlip = _reader.GetOptionalAttributeParseable<uint>("vflip").GetValueOr(0) == 1;
    var rotate = _reader.GetOptionalAttributeParseable<uint>("rotate").GetValueOr(0) == 1;
    var preferUntransformed = _reader.GetOptionalAttributeParseable<uint>("preferuntransformed").GetValueOr(0) == 1;

    _reader.ReadStartElement("transformations");
    return new Transformations { HFlip = hFlip, VFlip = vFlip, Rotate = rotate, PreferUntransformed = preferUntransformed };
  }

  internal Tile ReadTile()
  {
    // Attributes
    var id = _reader.GetRequiredAttributeParseable<uint>("id");
    var type = _reader.GetOptionalAttribute("type").GetValueOr("");
    var probability = _reader.GetOptionalAttributeParseable<float>("probability").GetValueOr(0f);
    var x = _reader.GetOptionalAttributeParseable<uint>("x").GetValueOr(0);
    var y = _reader.GetOptionalAttributeParseable<uint>("y").GetValueOr(0);
    var width = _reader.GetOptionalAttributeParseable<uint>("width");
    var height = _reader.GetOptionalAttributeParseable<uint>("height");

    // Elements
    List<IProperty> properties = null;
    Image image = null;
    ObjectLayer objectLayer = null;
    List<Frame> animation = null;

    _reader.ProcessChildren("tile", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      "image" => () => Helpers.SetAtMostOnce(ref image, ReadImage(), "Image"),
      "objectgroup" => () => Helpers.SetAtMostOnce(ref objectLayer, ReadObjectLayer(), "ObjectLayer"),
      "animation" => () => Helpers.SetAtMostOnce(ref animation, r.ReadList<Frame>("animation", "frame", (ar) =>
      {
        var tileID = ar.GetRequiredAttributeParseable<uint>("tileid");
        var duration = ar.GetRequiredAttributeParseable<uint>("duration");
        return new Frame { TileID = tileID, Duration = duration };
      }), "Animation"),
      _ => r.Skip
    });

    return new Tile
    {
      ID = id,
      Type = type,
      Probability = probability,
      X = x,
      Y = y,
      Width = width.HasValue ? width : image?.Width ?? 0,
      Height = height.HasValue ? height : image?.Height ?? 0,
      Properties = properties ?? [],
      Image = image is null ? Optional<Image>.Empty : image,
      ObjectLayer = objectLayer is null ? Optional<ObjectLayer>.Empty : objectLayer,
      Animation = animation ?? []
    };
  }

  internal List<Wangset> ReadWangsets() =>
    _reader.ReadList<Wangset>("wangsets", "wangset", r => ReadWangset());

  internal Wangset ReadWangset()
  {
    // Attributes
    var name = _reader.GetRequiredAttribute("name");
    var @class = _reader.GetOptionalAttribute("class").GetValueOr("");
    var tile = _reader.GetRequiredAttributeParseable<int>("tile");

    // Elements
    List<IProperty> properties = null;
    List<WangColor> wangColors = [];
    List<WangTile> wangTiles = [];

    _reader.ProcessChildren("wangset", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      "wangcolor" => () => wangColors.Add(ReadWangColor()),
      "wangtile" => () => wangTiles.Add(ReadWangTile()),
      _ => r.Skip
    });

    if (wangColors.Count > 254)
      throw new ArgumentException("Wangset can have at most 254 Wang colors.");

    return new Wangset
    {
      Name = name,
      Class = @class,
      Tile = tile,
      Properties = properties ?? [],
      WangColors = wangColors,
      WangTiles = wangTiles
    };
  }

  internal WangColor ReadWangColor()
  {
    // Attributes
    var name = _reader.GetRequiredAttribute("name");
    var @class = _reader.GetOptionalAttribute("class").GetValueOr("");
    var color = _reader.GetRequiredAttributeParseable<Color>("color");
    var tile = _reader.GetRequiredAttributeParseable<int>("tile");
    var probability = _reader.GetOptionalAttributeParseable<float>("probability").GetValueOr(0f);

    // Elements
    List<IProperty> properties = null;

    _reader.ProcessChildren("wangcolor", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      _ => r.Skip
    });

    return new WangColor
    {
      Name = name,
      Class = @class,
      Color = color,
      Tile = tile,
      Probability = probability,
      Properties = properties ?? []
    };
  }

  internal WangTile ReadWangTile()
  {
    // Attributes
    var tileID = _reader.GetRequiredAttributeParseable<uint>("tileid");
    var wangID = _reader.GetRequiredAttributeParseable<byte[]>("wangid", s =>
    {
      // Comma-separated list of indices (0-254)
      var indices = s.Split(',').Select(i => byte.Parse(i, CultureInfo.InvariantCulture)).ToArray();
      if (indices.Length > 8)
        throw new ArgumentException("Wang ID can have at most 8 indices.");
      return indices;
    });

    _reader.ReadStartElement("wangtile");

    return new WangTile
    {
      TileID = tileID,
      WangID = wangID
    };
  }
}
