using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithEmbeddedTileset() => new Map
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
        TileWidth = 32,
        TileHeight = 32,
        TileCount = 24,
        Columns = 8,
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
            0, 0, 0, 0, 7,
            9, 10, 0, 0, 7,
            17, 18, 0, 0, 0
          ]),
          FlippingFlags = new Optional<FlippingFlags[]>([
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
          ])
        }
      }
    ]
  };
}
