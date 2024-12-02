using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithFlippingFlags(string fileExt) => new Map
  {
    Class = "",
    Orientation = MapOrientation.Orthogonal,
    Width = 5,
    Height = 5,
    TileWidth = 32,
    TileHeight = 32,
    Infinite = false,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = Color.Parse("#00000000", CultureInfo.InvariantCulture),
    Version = "1.10",
    TiledVersion = "1.11.0",
    NextLayerID = 3,
    NextObjectID = 3,
    Tilesets = [
      new Tileset
      {
        Version = "1.10",
        TiledVersion = "1.11.0",
        FirstGID = 1,
        Name = "tileset",
        TileWidth = 32,
        TileHeight = 32,
        TileCount = 24,
        Columns = 8,
        Source = $"tileset.{(fileExt == "tmx" ? "tsx" : "tsj")}",
        Image = new Image
        {
          Format = ImageFormat.Png,
          Source = "tileset.png",
          Width = 256,
          Height = 96,
        }
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
            1, 1, 0, 0, 7,
            1, 1, 0, 0, 7,
            0, 0, 1, 0, 7,
            0, 0, 0, 1, 7,
            21, 21, 21, 21, 1
          ]),
          FlippingFlags = new Optional<FlippingFlags[]>([
            FlippingFlags.None, FlippingFlags.FlippedDiagonally | FlippingFlags.FlippedHorizontally, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.FlippedDiagonally | FlippingFlags.FlippedVertically, FlippingFlags.FlippedVertically | FlippingFlags.FlippedHorizontally, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.FlippedHorizontally, FlippingFlags.FlippedHorizontally, FlippingFlags.FlippedHorizontally, FlippingFlags.FlippedHorizontally, FlippingFlags.None
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
            GID = 21,
            X = 80.0555f,
            Y = 48.3887f,
            Width = 32,
            Height = 32,
            FlippingFlags = FlippingFlags.FlippedHorizontally
          },
          new TileObject
          {
            ID = 2,
            GID = 21,
            X = 15.833f,
            Y = 112.056f,
            Width = 32,
            Height = 32,
            FlippingFlags = FlippingFlags.FlippedHorizontally | FlippingFlags.FlippedVertically
          }
        ]
      }
    ]
  };
}
