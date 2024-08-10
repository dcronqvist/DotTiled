using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static Tileset ReadTileset(
    JsonElement element,
    Func<string, Tileset>? externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    var backgroundColor = element.GetOptionalPropertyParseable<Color?>("backgroundcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var @class = element.GetOptionalProperty<string>("class", "");
    var columns = element.GetOptionalProperty<uint?>("columns", null);
    var fillMode = element.GetOptionalPropertyParseable<FillMode>("fillmode", s => s switch
    {
      "stretch" => FillMode.Stretch,
      "preserve-aspect-fit" => FillMode.PreserveAspectFit,
      _ => throw new JsonException($"Unknown fill mode '{s}'")
    }, FillMode.Stretch);
    var firstGID = element.GetOptionalProperty<uint?>("firstgid", null);
    var grid = element.GetOptionalPropertyCustom<Grid?>("grid", ReadGrid, null);
    var image = element.GetOptionalProperty<string?>("image", null);
    var imageHeight = element.GetOptionalProperty<uint?>("imageheight", null);
    var imageWidth = element.GetOptionalProperty<uint?>("imagewidth", null);
    var margin = element.GetOptionalProperty<uint?>("margin", null);
    var name = element.GetOptionalProperty<string?>("name", null);
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
    }, ObjectAlignment.Unspecified);
    var properties = element.GetOptionalPropertyCustom<Dictionary<string, IProperty>?>("properties", el => ReadProperties(el, customTypeDefinitions), null);
    var source = element.GetOptionalProperty<string?>("source", null);
    var spacing = element.GetOptionalProperty<uint?>("spacing", null);
    var tileCount = element.GetOptionalProperty<uint?>("tilecount", null);
    var tiledVersion = element.GetOptionalProperty<string?>("tiledversion", null);
    var tileHeight = element.GetOptionalProperty<uint?>("tileheight", null);
    var tileOffset = element.GetOptionalPropertyCustom<TileOffset?>("tileoffset", ReadTileOffset, null);
    var tileRenderSize = element.GetOptionalPropertyParseable<TileRenderSize>("tilerendersize", s => s switch
    {
      "tile" => TileRenderSize.Tile,
      "grid" => TileRenderSize.Grid,
      _ => throw new JsonException($"Unknown tile render size '{s}'")
    }, TileRenderSize.Tile);
    var tiles = element.GetOptionalPropertyCustom<List<Tile>>("tiles", el => ReadTiles(el, customTypeDefinitions), []);
    var tileWidth = element.GetOptionalProperty<uint?>("tilewidth", null);
    var transparentColor = element.GetOptionalPropertyParseable<Color?>("transparentcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var type = element.GetOptionalProperty<string?>("type", null);
    var version = element.GetOptionalProperty<string?>("version", null);
    //var wangsets = element.GetOptionalPropertyCustom<List<Wangset>?>("wangsets", ReadWangSets, null);

    if (source is not null)
    {
      if (externalTilesetResolver is null)
        throw new JsonException("External tileset resolver is required to resolve external tilesets.");

      var resolvedTileset = externalTilesetResolver(source);
      resolvedTileset.FirstGID = firstGID;
      resolvedTileset.Source = source;
      return resolvedTileset;
    }

    var imageModel = new Image
    {
      Format = ParseImageFormatFromSource(image!),
      Source = image,
      Height = imageHeight,
      Width = imageWidth,
      TransparentColor = transparentColor
    };

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
      //Wangsets = wangsets
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
      _ => throw new JsonException($"Unsupported image format '{extension}'")
    };
  }

  internal static Grid ReadGrid(JsonElement element)
  {
    var orientation = element.GetOptionalPropertyParseable<GridOrientation>("orientation", s => s switch
    {
      "orthogonal" => GridOrientation.Orthogonal,
      "isometric" => GridOrientation.Isometric,
      _ => throw new JsonException($"Unknown grid orientation '{s}'")
    }, GridOrientation.Orthogonal);
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

  internal static List<Tile> ReadTiles(
    JsonElement element,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions) =>
    element.GetValueAsList<Tile>(e =>
    {
      //var animation = e.GetOptionalPropertyCustom<List<Frame>>("animation", ReadFrames, null);
      var id = e.GetRequiredProperty<uint>("id");
      var image = e.GetOptionalProperty<string?>("image", null);
      var imageHeight = e.GetOptionalProperty<uint?>("imageheight", null);
      var imageWidth = e.GetOptionalProperty<uint?>("imagewidth", null);
      var x = e.GetOptionalProperty<uint>("x", 0);
      var y = e.GetOptionalProperty<uint>("y", 0);
      var width = e.GetOptionalProperty<uint>("width", imageWidth ?? 0);
      var height = e.GetOptionalProperty<uint>("height", imageHeight ?? 0);
      //var objectGroup = e.GetOptionalPropertyCustom<ObjectLayer?>("objectgroup", ReadObjectLayer, null);
      var probability = e.GetOptionalProperty<float>("probability", 1.0f);
      var properties = e.GetOptionalPropertyCustom<Dictionary<string, IProperty>?>("properties", el => ReadProperties(el, customTypeDefinitions), null);
      // var terrain, replaced by wangsets
      var type = e.GetOptionalProperty<string>("type", "");

      var imageModel = image != null ? new Image
      {
        Format = ParseImageFormatFromSource(image),
        Source = image,
        Height = imageHeight ?? 0,
        Width = imageWidth ?? 0
      } : null;

      return new Tile
      {
        //Animation = animation,
        ID = id,
        Image = imageModel,
        X = x,
        Y = y,
        Width = width,
        Height = height,
        //ObjectLayer = objectGroup,
        Probability = probability,
        Properties = properties,
        Type = type
      };
    });
}
