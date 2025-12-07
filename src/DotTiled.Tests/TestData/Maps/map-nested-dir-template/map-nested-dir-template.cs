using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapNestedDirTemplate(string fileExt) => new Map
  {
    Class = "",
    Orientation = MapOrientation.Orthogonal,
    Width = 2,
    Height = 2,
    TileWidth = 32,
    TileHeight = 32,
    Infinite = false,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = TiledColor.Parse("#00000000", CultureInfo.InvariantCulture),
    Version = "1.10",
    TiledVersion = "1.11.2",
    NextLayerID = 3,
    NextObjectID = 2,
    Tilesets = [
      new Tileset
      {
        Version = "1.10",
        TiledVersion = "1.11.2",
        FirstGID = 1,
        Name = "B",
        TileWidth = 32,
        TileHeight = 32,
        TileCount = 0,
        Columns = 1,
        Source = $"../B/B.{(fileExt == "tmx" ? "tsx" : "tsj")}",
        Image = new Image
        {
          Format = ImageFormat.Png,
          Source = "tile.png",
          Width = 32,
          Height = 32,
        }
      }
    ],
    Layers = [
      new TileLayer
      {
        ID = 1,
        Name = "Tile Layer 1",
        Width = 2,
        Height = 2,
        Data = new Data
        {
          Encoding = DataEncoding.Csv,
          GlobalTileIDs = new Optional<uint[]>([
            0, 0,
            0, 0
          ]),
          FlippingFlags = new Optional<FlippingFlags[]>([
            FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None
          ])
        }
      },
      new ObjectLayer
      {
        ID = 2,
        Name = "Object Layer 1",
        Objects = [
          new TileObject
          {
            ID = 1,
            Name = "TemplateObject",
            X = 32,
            Y = 32,
            Width = 16,
            Height = 16,
            GID = 1,
            Template = $"../B/B.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = new Tileset
            {
              Version = "1.10",
              TiledVersion = "1.11.2",
              FirstGID = 1,
              Name = "B",
              TileWidth = 32,
              TileHeight = 32,
              TileCount = 0,
              Columns = 1,
              Source = $"B.{(fileExt == "tmx" ? "tsx" : "tsj")}",
              Image = new Image
              {
                Format = ImageFormat.Png,
                Source = "tile.png",
                Width = 32,
                Height = 32,
              }
            }
          }
        ],
      }
    ]
  };
}
