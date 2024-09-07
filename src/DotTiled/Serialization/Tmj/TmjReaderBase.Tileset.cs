using System.Collections.Generic;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal Tileset ReadTileset(
    JsonElement element,
    Optional<string> parentVersion = null,
    Optional<string> parentTiledVersion = null)
  {
    var backgroundColor = element.GetOptionalPropertyParseable<Color>("backgroundcolor");
    var @class = element.GetOptionalProperty<string>("class").GetValueOr("");
    var columns = element.GetOptionalProperty<uint>("columns");
    var fillMode = element.GetOptionalPropertyParseable<FillMode>("fillmode", s => s switch
    {
      "stretch" => FillMode.Stretch,
      "preserve-aspect-fit" => FillMode.PreserveAspectFit,
      _ => throw new JsonException($"Unknown fill mode '{s}'")
    }).GetValueOr(FillMode.Stretch);
    var firstGID = element.GetOptionalProperty<uint>("firstgid");
    var grid = element.GetOptionalPropertyCustom<Grid>("grid", ReadGrid);
    var image = element.GetOptionalProperty<string>("image");
    var imageHeight = element.GetOptionalProperty<uint>("imageheight");
    var imageWidth = element.GetOptionalProperty<uint>("imagewidth");
    var margin = element.GetOptionalProperty<uint>("margin");
    var name = element.GetOptionalProperty<string>("name");
    var objectAlignment = element.GetOptionalPropertyParseable<ObjectAlignment>("objectalignment", s => s switch
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
      _ => throw new JsonException($"Unknown object alignment '{s}'")
    }).GetValueOr(ObjectAlignment.Unspecified);
    var properties = ResolveAndMergeProperties(@class, element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));
    var source = element.GetOptionalProperty<string>("source");
    var spacing = element.GetOptionalProperty<uint>("spacing");
    var tileCount = element.GetOptionalProperty<uint>("tilecount");
    var tiledVersion = element.GetOptionalProperty<string>("tiledversion").GetValueOrOptional(parentTiledVersion);
    var tileHeight = element.GetOptionalProperty<uint>("tileheight");
    var tileOffset = element.GetOptionalPropertyCustom<TileOffset>("tileoffset", ReadTileOffset);
    var tileRenderSize = element.GetOptionalPropertyParseable<TileRenderSize>("tilerendersize", s => s switch
    {
      "tile" => TileRenderSize.Tile,
      "grid" => TileRenderSize.Grid,
      _ => throw new JsonException($"Unknown tile render size '{s}'")
    }).GetValueOr(TileRenderSize.Tile);
    var tiles = element.GetOptionalPropertyCustom<List<Tile>>("tiles", ReadTiles).GetValueOr([]);
    var tileWidth = element.GetOptionalProperty<uint>("tilewidth");
    var transparentColor = element.GetOptionalPropertyParseable<Color>("transparentcolor");
    var type = element.GetOptionalProperty<string>("type");
    var version = element.GetOptionalProperty<string>("version").GetValueOrOptional(parentVersion);
    var transformations = element.GetOptionalPropertyCustom<Transformations>("transformations", ReadTransformations);
    var wangsets = element.GetOptionalPropertyCustom<List<Wangset>>("wangsets", el => el.GetValueAsList<Wangset>(e => ReadWangset(e))).GetValueOr([]);

    if (source.HasValue)
    {
      var resolvedTileset = _externalTilesetResolver(source);
      resolvedTileset.FirstGID = firstGID;
      resolvedTileset.Source = source;
      return resolvedTileset;
    }

    Optional<Image> imageModel = image.HasValue ? new Image
    {
      Format = Helpers.ParseImageFormatFromSource(image),
      Source = image,
      Height = imageHeight,
      Width = imageWidth,
      TransparentColor = transparentColor
    } : Optional<Image>.Empty;

    return new Tileset
    {
      Class = @class,
      Columns = columns,
      FillMode = fillMode,
      FirstGID = firstGID,
      Grid = grid,
      Image = imageModel,
      Margin = margin,
      Name = name,
      ObjectAlignment = objectAlignment,
      Properties = properties,
      Source = source,
      Spacing = spacing,
      TileCount = tileCount,
      TiledVersion = tiledVersion,
      TileHeight = tileHeight,
      TileOffset = tileOffset,
      RenderSize = tileRenderSize,
      Tiles = tiles,
      TileWidth = tileWidth,
      Version = version,
      Wangsets = wangsets,
      Transformations = transformations
    };
  }

  internal static Transformations ReadTransformations(JsonElement element)
  {
    var hFlip = element.GetOptionalProperty<bool>("hflip").GetValueOr(false);
    var vFlip = element.GetOptionalProperty<bool>("vflip").GetValueOr(false);
    var rotate = element.GetOptionalProperty<bool>("rotate").GetValueOr(false);
    var preferUntransformed = element.GetOptionalProperty<bool>("preferuntransformed").GetValueOr(false);

    return new Transformations
    {
      HFlip = hFlip,
      VFlip = vFlip,
      Rotate = rotate,
      PreferUntransformed = preferUntransformed
    };
  }

  internal static Grid ReadGrid(JsonElement element)
  {
    var orientation = element.GetOptionalPropertyParseable<GridOrientation>("orientation", s => s switch
    {
      "orthogonal" => GridOrientation.Orthogonal,
      "isometric" => GridOrientation.Isometric,
      _ => throw new JsonException($"Unknown grid orientation '{s}'")
    }).GetValueOr(GridOrientation.Orthogonal);
    var height = element.GetRequiredProperty<uint>("height");
    var width = element.GetRequiredProperty<uint>("width");

    return new Grid
    {
      Orientation = orientation,
      Height = height,
      Width = width
    };
  }

  internal static TileOffset ReadTileOffset(JsonElement element)
  {
    var x = element.GetRequiredProperty<int>("x");
    var y = element.GetRequiredProperty<int>("y");

    return new TileOffset
    {
      X = x,
      Y = y
    };
  }

  internal List<Tile> ReadTiles(JsonElement element) =>
    element.GetValueAsList<Tile>(e =>
    {
      var animation = e.GetOptionalPropertyCustom<List<Frame>>("animation", e => e.GetValueAsList<Frame>(ReadFrame)).GetValueOr([]);
      var id = e.GetRequiredProperty<uint>("id");
      var image = e.GetOptionalProperty<string>("image");
      var imageHeight = e.GetOptionalProperty<uint>("imageheight");
      var imageWidth = e.GetOptionalProperty<uint>("imagewidth");
      var x = e.GetOptionalProperty<uint>("x").GetValueOr(0);
      var y = e.GetOptionalProperty<uint>("y").GetValueOr(0);
      var width = e.GetOptionalProperty<uint>("width").GetValueOr(imageWidth.GetValueOr(0));
      var height = e.GetOptionalProperty<uint>("height").GetValueOr(imageHeight.GetValueOr(0));
      var objectGroup = e.GetOptionalPropertyCustom<ObjectLayer>("objectgroup", e => ReadObjectLayer(e));
      var probability = e.GetOptionalProperty<float>("probability").GetValueOr(0.0f);
      var type = e.GetOptionalProperty<string>("type").GetValueOr("");
      var properties = ResolveAndMergeProperties(type, e.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));

      Optional<Image> imageModel = image.HasValue ? new Image
      {
        Format = Helpers.ParseImageFormatFromSource(image),
        Source = image,
        Height = imageHeight ?? 0,
        Width = imageWidth ?? 0
      } : Optional<Image>.Empty;

      return new Tile
      {
        Animation = animation,
        ID = id,
        Image = imageModel,
        X = x,
        Y = y,
        Width = width,
        Height = height,
        ObjectLayer = objectGroup,
        Probability = probability,
        Properties = properties,
        Type = type
      };
    });

  internal static Frame ReadFrame(JsonElement element)
  {
    var duration = element.GetRequiredProperty<uint>("duration");
    var tileID = element.GetRequiredProperty<uint>("tileid");

    return new Frame
    {
      Duration = duration,
      TileID = tileID
    };
  }

  internal Wangset ReadWangset(JsonElement element)
  {
    var @class = element.GetOptionalProperty<string>("class").GetValueOr("");
    var colors = element.GetOptionalPropertyCustom<List<WangColor>>("colors", e => e.GetValueAsList<WangColor>(el => ReadWangColor(el))).GetValueOr([]);
    var name = element.GetRequiredProperty<string>("name");
    var properties = ResolveAndMergeProperties(@class, element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));
    var tile = element.GetOptionalProperty<int>("tile").GetValueOr(0);
    var type = element.GetOptionalProperty<string>("type").GetValueOr("");
    var wangTiles = element.GetOptionalPropertyCustom<List<WangTile>>("wangtiles", e => e.GetValueAsList<WangTile>(ReadWangTile)).GetValueOr([]);

    return new Wangset
    {
      Class = @class,
      WangColors = colors,
      Name = name,
      Properties = properties,
      Tile = tile,
      WangTiles = wangTiles
    };
  }

  internal WangColor ReadWangColor(JsonElement element)
  {
    var @class = element.GetOptionalProperty<string>("class").GetValueOr("");
    var color = element.GetRequiredPropertyParseable<Color>("color");
    var name = element.GetRequiredProperty<string>("name");
    var probability = element.GetOptionalProperty<float>("probability").GetValueOr(1.0f);
    var properties = ResolveAndMergeProperties(@class, element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));
    var tile = element.GetOptionalProperty<int>("tile").GetValueOr(0);

    return new WangColor
    {
      Class = @class,
      Color = color,
      Name = name,
      Probability = probability,
      Properties = properties,
      Tile = tile
    };
  }

  internal static WangTile ReadWangTile(JsonElement element)
  {
    var tileID = element.GetRequiredProperty<uint>("tileid");
    var wangID = element.GetOptionalPropertyCustom<List<byte>>("wangid", e => e.GetValueAsList<byte>(el => (byte)el.GetUInt32())).GetValueOr([]);

    return new WangTile
    {
      TileID = tileID,
      WangID = [.. wangID]
    };
  }
}
