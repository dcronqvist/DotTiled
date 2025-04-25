using System.Numerics;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithManyLayers(string fileExt) => new Map
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
    BackgroundColor = new TiledColor { R = 0, G = 0, B = 0, A = 0 },
    Version = "1.10",
    TiledVersion = "1.11.2",
    NextLayerID = 8,
    NextObjectID = 7,
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
      new Group
      {
        ID = 2,
        Name = "Root",
        Layers = [
          new ObjectLayer
          {
            ID = 3,
            Name = "Objects",
            Objects = [
              new RectangleObject
              {
                ID = 1,
                Name = "Object 1",
                X = 25.6667f,
                Y = 28.6667f,
                Width = 31.3333f,
                Height = 31.3333f
              },
              new PointObject
              {
                ID = 3,
                Name = "P1",
                X = 117.667f,
                Y = 48.6667f
              },
              new EllipseObject
              {
                ID = 4,
                Name = "Circle1",
                X = 77f,
                Y = 72.3333f,
                Width = 34.6667f,
                Height = 34.6667f
              },
              new PolygonObject
              {
                ID = 5,
                Name = "Poly",
                X = 20.6667f,
                Y = 114.667f,
                Points = [
                  new Vector2(0, 0),
                  new Vector2(104,20),
                  new Vector2(35.6667f, 32.3333f)
                ],
                Template = fileExt == "tmx" ? "poly.tx" : "poly.tj",
                Properties = [
                  new StringProperty { Name = "templateprop", Value = "helo there" }
                ]
              },
              new TileObject
              {
                ID = 6,
                Name = "TileObj",
                GID = 7,
                X = -35,
                Y = 110.333f,
                Width = 64,
                Height = 146
              }
            ]
          },
          new Group
          {
            ID = 5,
            Name = "Sub",
            Layers = [
              new TileLayer
              {
                ID = 7,
                Name = "Tile 3",
                Width = 5,
                Height = 5,
                Data = new Data
                {
                  Encoding = DataEncoding.Csv,
                  GlobalTileIDs = new Optional<uint[]>([
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0
                  ]),
                  FlippingFlags = new Optional<FlippingFlags[]>([
                    FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
                    FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
                    FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
                    FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
                    FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
                  ])
                }
              },
              new TileLayer
              {
                ID = 6,
                Name = "Tile 2",
                Width = 5,
                Height = 5,
                Data = new Data
                {
                  Encoding = DataEncoding.Csv,
                  GlobalTileIDs = new Optional<uint[]>([
                    0, 15, 15, 0, 0,
                    0, 15, 15, 0, 0,
                    0, 15, 15, 15, 0,
                    15, 15, 15, 0, 0,
                    0, 0, 0, 0, 0
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
          },
          new ImageLayer
          {
            ID = 4,
            Name = "ImageLayer",
            Image = new Image
            {
              Format = ImageFormat.Png,
              Source = "tileset.png",
              Width = 256,
              Height = 96
            },
            RepeatX = true
          },
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
                1, 1, 1, 1, 1,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0
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
      }
    ]
  };
}
