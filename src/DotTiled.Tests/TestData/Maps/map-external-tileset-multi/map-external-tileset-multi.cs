using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapExternalTilesetMulti(string fileExt) => new Map
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
        TileOffset = new TileOffset
        {
          X = 1,
          Y = 5
        },
        Version = "1.10",
        TiledVersion = "1.11.0",
        FirstGID = 1,
        Name = "multi-tileset",
        TileWidth = 256,
        TileHeight = 96,
        TileCount = 2,
        Columns = 0,
        Source = $"multi-tileset.{(fileExt == "tmx" ? "tsx" : "tsj")}",
        Grid = new Grid
        {
          Orientation = GridOrientation.Orthogonal,
          Width = 1,
          Height = 1
        },
        Properties = [
          new BoolProperty { Name = "tilesetbool", Value = true },
          new ColorProperty { Name = "tilesetcolor", Value = TiledColor.Parse("#ffff0000", CultureInfo.InvariantCulture) },
          new FileProperty { Name = "tilesetfile", Value = "" },
          new FloatProperty { Name = "tilesetfloat", Value = 5.2f },
          new IntProperty { Name = "tilesetint", Value = 9 },
          new ObjectProperty { Name = "tilesetobject", Value = 0 },
          new StringProperty { Name = "tilesetstring", Value = "hello world!" }
        ],
        Tiles = [
          new Tile
          {
            ID = 0,
            Width = 256,
            Height = 96,
            Image = new Image
            {
              Format = ImageFormat.Png,
              Source = "tileset.png",
              Width = 256,
              Height = 96
            }
          },
          new Tile
          {
            ID = 1,
            Width = 256,
            Height = 96,
            Image = new Image
            {
              Format = ImageFormat.Png,
              Source = "tileset.png",
              Width = 256,
              Height = 96
            }
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
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            1, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 2, 0, 0, 0
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
