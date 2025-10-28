using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapExternalTilesetWangset(string fileExt) => new Map
  {
    Class = "",
    Orientation = MapOrientation.Orthogonal,
    Width = 5,
    Height = 5,
    TileWidth = 24,
    TileHeight = 24,
    Infinite = false,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = TiledColor.Parse("#00000000", CultureInfo.InvariantCulture),
    Version = "1.10",
    TiledVersion = "1.11.0",
    NextLayerID = 2,
    NextObjectID = 1,
    Tilesets = [
      new Tileset
      {
        Version = "1.10",
        TiledVersion = "1.11.0",
        FirstGID = 1,
        Name = "tileset",
        TileWidth = 24,
        TileHeight = 24,
        TileCount = 48,
        Columns = 10,
        Source = $"wangset-tileset.{(fileExt == "tmx" ? "tsx" : "tsj")}",
        Transformations = new Transformations
        {
          HFlip = true,
          VFlip = true,
          Rotate = false,
          PreferUntransformed = false
        },
        Grid = new Grid
        {
          Orientation = GridOrientation.Orthogonal,
          Width = 32,
          Height = 32
        },
        Image = new Image
        {
          Format = ImageFormat.Png,
          Source = "tileset.png",
          Width = 256,
          Height = 96,
        },
        Wangsets = [
          new Wangset
          {
            Name = "test-terrain",
            Tile = -1,
            WangColors = [
              new WangColor
              {
                Name = "Water",
                Color = TiledColor.Parse("#ff0000", CultureInfo.InvariantCulture),
                Tile = 0,
                Probability = 1
              },
              new WangColor
              {
                Name = "Grass",
                Color = TiledColor.Parse("#00ff00", CultureInfo.InvariantCulture),
                Tile = -1,
                Probability = 1
              },
              new WangColor
              {
                Name = "Stone",
                Color = TiledColor.Parse("#0000ff", CultureInfo.InvariantCulture),
                Tile = 29,
                Probability = 1
              }
            ],
            WangTiles = [
              new WangTile
              {
                TileID = 0,
                WangID = [1,1,0,0,0,1,1,1]
              },
              new WangTile
              {
                TileID = 1,
                WangID = [1,1,1,1,0,0,0,1]
              },
              new WangTile
              {
                TileID = 10,
                WangID = [0,0,0,1,1,1,1,1]
              },
              new WangTile
              {
                TileID = 11,
                WangID = [0,1,1,1,1,1,0,0]
              }
            ]
          }
        ]
      }
    ],
    Layers = [
      new TileLayer
      {
        ID = 1,
        Name = "Tile Layer 1",
        Width = 5,
        Height = 5,
        Data = new Data
        {
          Encoding = DataEncoding.Csv,
          GlobalTileIDs = new Optional<uint[]>([
            2, 2, 12, 11, 0,
            1, 12, 1, 11, 0,
            2, 1, 0, 1, 0,
            12, 11, 12, 2, 0,
            0, 0, 0, 0, 0
          ]),
          FlippingFlags = new Optional<FlippingFlags[]>([
            FlippingFlags.FlippedHorizontally, FlippingFlags.None, FlippingFlags.FlippedHorizontally, FlippingFlags.FlippedHorizontally, FlippingFlags.None,
            FlippingFlags.FlippedVertically, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically | FlippingFlags.FlippedHorizontally, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically | FlippingFlags.FlippedHorizontally, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedHorizontally, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
          ])
        }
      }
    ]
  };
}
