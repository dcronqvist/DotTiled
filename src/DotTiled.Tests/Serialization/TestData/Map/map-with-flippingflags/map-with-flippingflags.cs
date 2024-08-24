using System.Globalization;
using DotTiled.Model;

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
    HexSideLength = null,
    StaggerAxis = null,
    StaggerIndex = null,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = Color.Parse("#00000000", CultureInfo.InvariantCulture),
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
          Chunks = null,
          Compression = null,
          GlobalTileIDs = [
            1, 1, 0, 0, 7,
            1, 1, 0, 0, 7,
            0, 0, 1, 0, 7,
            0, 0, 0, 1, 7,
            21, 21, 21, 21, 1
          ],
          FlippingFlags = [
            FlippingFlags.None, FlippingFlags.FlippedDiagonally | FlippingFlags.FlippedHorizontally, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.FlippedDiagonally | FlippingFlags.FlippedVertically, FlippingFlags.FlippedVertically | FlippingFlags.FlippedHorizontally, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.FlippedVertically,
            FlippingFlags.FlippedHorizontally, FlippingFlags.FlippedHorizontally, FlippingFlags.FlippedHorizontally, FlippingFlags.FlippedHorizontally, FlippingFlags.None
          ]
        }
      }
    ]
  };
}
