using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

internal partial class Tmx
{
  internal static Tileset ReadTileset(
    XmlReader reader,
    Func<string, Tileset>? externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Attributes
    var version = reader.GetOptionalAttribute("version");
    var tiledVersion = reader.GetOptionalAttribute("tiledversion");
    var firstGID = reader.GetOptionalAttributeParseable<uint>("firstgid");
    var source = reader.GetOptionalAttribute("source");
    var name = reader.GetOptionalAttribute("name");
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var tileWidth = reader.GetOptionalAttributeParseable<uint>("tilewidth");
    var tileHeight = reader.GetOptionalAttributeParseable<uint>("tileheight");
    var spacing = reader.GetOptionalAttributeParseable<uint>("spacing") ?? 0;
    var margin = reader.GetOptionalAttributeParseable<uint>("margin") ?? 0;
    var tileCount = reader.GetOptionalAttributeParseable<uint>("tilecount");
    var columns = reader.GetOptionalAttributeParseable<uint>("columns");
    var objectAlignment = reader.GetOptionalAttributeEnum<ObjectAlignment>("objectalignment", s => s switch
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
    }) ?? ObjectAlignment.Unspecified;
    var renderSize = reader.GetOptionalAttributeEnum<TileRenderSize>("rendersize", s => s switch
    {
      "tile" => TileRenderSize.Tile,
      "grid" => TileRenderSize.Grid,
      _ => throw new InvalidOperationException($"Unknown render size '{s}'")
    }) ?? TileRenderSize.Tile;
    var fillMode = reader.GetOptionalAttributeEnum<FillMode>("fillmode", s => s switch
    {
      "stretch" => FillMode.Stretch,
      "preserve-aspect-fit" => FillMode.PreserveAspectFit,
      _ => throw new InvalidOperationException($"Unknown fill mode '{s}'")
    }) ?? FillMode.Stretch;

    // Elements
    Image? image = null;
    TileOffset? tileOffset = null;
    Grid? grid = null;
    Dictionary<string, IProperty>? properties = null;
    List<Wangset>? wangsets = null;
    Transformations? transformations = null;
    List<Tile> tiles = [];

    reader.ProcessChildren("tileset", (r, elementName) => elementName switch
    {
      "image" => () => Helpers.SetAtMostOnce(ref image, ReadImage(r), "Image"),
      "tileoffset" => () => Helpers.SetAtMostOnce(ref tileOffset, ReadTileOffset(r), "TileOffset"),
      "grid" => () => Helpers.SetAtMostOnce(ref grid, ReadGrid(r), "Grid"),
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r, customTypeDefinitions), "Properties"),
      "wangsets" => () => Helpers.SetAtMostOnce(ref wangsets, ReadWangsets(r, customTypeDefinitions), "Wangsets"),
      "transformations" => () => Helpers.SetAtMostOnce(ref transformations, ReadTransformations(r), "Transformations"),
      "tile" => () => tiles.Add(ReadTile(r, externalTemplateResolver, customTypeDefinitions)),
      _ => r.Skip
    });

    // Check if tileset is referring to external file
    if (source is not null)
    {
      if (externalTilesetResolver is null)
        throw new InvalidOperationException("External tileset resolver is required to resolve external tilesets.");

      var resolvedTileset = externalTilesetResolver(source);
      resolvedTileset.FirstGID = firstGID;
      resolvedTileset.Source = source;
      return resolvedTileset;
    }

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
      Properties = properties,
      Wangsets = wangsets,
      Transformations = transformations,
      Tiles = tiles
    };
  }

  internal static Image ReadImage(XmlReader reader)
  {
    // Attributes
    var format = reader.GetOptionalAttributeEnum<ImageFormat>("format", s => s switch
    {
      "png" => ImageFormat.Png,
      "jpg" => ImageFormat.Jpg,
      "bmp" => ImageFormat.Bmp,
      "gif" => ImageFormat.Gif,
      _ => throw new InvalidOperationException($"Unknown image format '{s}'")
    });
    var source = reader.GetOptionalAttribute("source");
    var transparentColor = reader.GetOptionalAttributeClass<Color>("trans");
    var width = reader.GetOptionalAttributeParseable<uint>("width");
    var height = reader.GetOptionalAttributeParseable<uint>("height");

    reader.ProcessChildren("image", (r, elementName) => elementName switch
    {
      "data" => throw new NotSupportedException("Embedded image data is not supported."),
      _ => r.Skip
    });

    if (format is null && source is not null)
      format = ParseImageFormatFromSource(source);

    return new Image
    {
      Format = format,
      Source = source,
      TransparentColor = transparentColor,
      Width = width,
      Height = height,
    };
  }


  private static ImageFormat ParseImageFormatFromSource(string source)
  {
    var extension = Path.GetExtension(source).ToLowerInvariant();
    return extension switch
    {
      ".png" => ImageFormat.Png,
      ".gif" => ImageFormat.Gif,
      ".jpg" => ImageFormat.Jpg,
      ".jpeg" => ImageFormat.Jpg,
      ".bmp" => ImageFormat.Bmp,
      _ => throw new XmlException($"Unsupported image format '{extension}'")
    };
  }

  internal static TileOffset ReadTileOffset(XmlReader reader)
  {
    // Attributes
    var x = reader.GetOptionalAttributeParseable<float>("x") ?? 0f;
    var y = reader.GetOptionalAttributeParseable<float>("y") ?? 0f;

    reader.ReadStartElement("tileoffset");
    return new TileOffset { X = x, Y = y };
  }

  internal static Grid ReadGrid(XmlReader reader)
  {
    // Attributes
    var orientation = reader.GetOptionalAttributeEnum<GridOrientation>("orientation", s => s switch
    {
      "orthogonal" => GridOrientation.Orthogonal,
      "isometric" => GridOrientation.Isometric,
      _ => throw new InvalidOperationException($"Unknown orientation '{s}'")
    }) ?? GridOrientation.Orthogonal;
    var width = reader.GetRequiredAttributeParseable<uint>("width");
    var height = reader.GetRequiredAttributeParseable<uint>("height");

    reader.ReadStartElement("grid");
    return new Grid { Orientation = orientation, Width = width, Height = height };
  }

  internal static Transformations ReadTransformations(XmlReader reader)
  {
    // Attributes
    var hFlip = (reader.GetOptionalAttributeParseable<uint>("hflip") ?? 0) == 1;
    var vFlip = (reader.GetOptionalAttributeParseable<uint>("vflip") ?? 0) == 1;
    var rotate = (reader.GetOptionalAttributeParseable<uint>("rotate") ?? 0) == 1;
    var preferUntransformed = (reader.GetOptionalAttributeParseable<uint>("preferuntransformed") ?? 0) == 1;

    reader.ReadStartElement("transformations");
    return new Transformations { HFlip = hFlip, VFlip = vFlip, Rotate = rotate, PreferUntransformed = preferUntransformed };
  }

  internal static Tile ReadTile(
    XmlReader reader,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Attributes
    var id = reader.GetRequiredAttributeParseable<uint>("id");
    var type = reader.GetOptionalAttribute("type") ?? "";
    var probability = reader.GetOptionalAttributeParseable<float>("probability") ?? 0f;
    var x = reader.GetOptionalAttributeParseable<uint>("x") ?? 0;
    var y = reader.GetOptionalAttributeParseable<uint>("y") ?? 0;
    var width = reader.GetOptionalAttributeParseable<uint>("width");
    var height = reader.GetOptionalAttributeParseable<uint>("height");

    // Elements
    Dictionary<string, IProperty>? properties = null;
    Image? image = null;
    ObjectLayer? objectLayer = null;
    List<Frame>? animation = null;

    reader.ProcessChildren("tile", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r, customTypeDefinitions), "Properties"),
      "image" => () => Helpers.SetAtMostOnce(ref image, ReadImage(r), "Image"),
      "objectgroup" => () => Helpers.SetAtMostOnce(ref objectLayer, ReadObjectLayer(r, externalTemplateResolver, customTypeDefinitions), "ObjectLayer"),
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
      Width = width ?? image?.Width ?? 0,
      Height = height ?? image?.Height ?? 0,
      Properties = properties,
      Image = image,
      ObjectLayer = objectLayer,
      Animation = animation
    };
  }

  internal static List<Wangset> ReadWangsets(
    XmlReader reader,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions) =>
    reader.ReadList<Wangset>("wangsets", "wangset", r => ReadWangset(r, customTypeDefinitions));

  internal static Wangset ReadWangset(
    XmlReader reader,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Attributes
    var name = reader.GetRequiredAttribute("name");
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var tile = reader.GetRequiredAttributeParseable<int>("tile");

    // Elements
    Dictionary<string, IProperty>? properties = null;
    List<WangColor> wangColors = [];
    List<WangTile> wangTiles = [];

    reader.ProcessChildren("wangset", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r, customTypeDefinitions), "Properties"),
      "wangcolor" => () => wangColors.Add(ReadWangColor(r, customTypeDefinitions)),
      "wangtile" => () => wangTiles.Add(ReadWangTile(r)),
      _ => r.Skip
    });

    if (wangColors.Count > 254)
      throw new ArgumentException("Wangset can have at most 254 Wang colors.");

    return new Wangset
    {
      Name = name,
      Class = @class,
      Tile = tile,
      Properties = properties,
      WangColors = wangColors,
      WangTiles = wangTiles
    };
  }

  internal static WangColor ReadWangColor(
    XmlReader reader,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    // Attributes
    var name = reader.GetRequiredAttribute("name");
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var color = reader.GetRequiredAttributeParseable<Color>("color");
    var tile = reader.GetRequiredAttributeParseable<int>("tile");
    var probability = reader.GetOptionalAttributeParseable<float>("probability") ?? 0f;

    // Elements
    Dictionary<string, IProperty>? properties = null;

    reader.ProcessChildren("wangcolor", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r, customTypeDefinitions), "Properties"),
      _ => r.Skip
    });

    return new WangColor
    {
      Name = name,
      Class = @class,
      Color = color,
      Tile = tile,
      Probability = probability,
      Properties = properties
    };
  }

  internal static WangTile ReadWangTile(XmlReader reader)
  {
    // Attributes
    var tileID = reader.GetRequiredAttributeParseable<uint>("tileid");
    var wangID = reader.GetRequiredAttributeParseable<byte[]>("wangid", s =>
    {
      // Comma-separated list of indices (0-254)
      var indices = s.Split(',').Select(i => byte.Parse(i, CultureInfo.InvariantCulture)).ToArray();
      if (indices.Length > 8)
        throw new ArgumentException("Wang ID can have at most 8 indices.");
      return indices;
    });

    reader.ReadStartElement("wangtile");

    return new WangTile
    {
      TileID = tileID,
      WangID = wangID
    };
  }
}
