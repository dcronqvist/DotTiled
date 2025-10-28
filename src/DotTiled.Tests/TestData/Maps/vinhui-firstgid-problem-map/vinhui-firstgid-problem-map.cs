using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  private static Tileset VinhuiObjectsTileset(uint firstGid, string fileExt) => new Tileset
  {
    Version = "1.10",
    TiledVersion = "1.11.2",
    Name = "objects",
    TileWidth = 64,
    TileHeight = 64,
    TileCount = 18,
    Columns = 0,
    FirstGID = firstGid,
    Source = $"objects.{(fileExt == "tmx" ? "tsx" : "tsj")}",
    Grid = new Grid
    {
      Orientation = GridOrientation.Orthogonal,
      Width = 1,
      Height = 1
    },
    Tiles = [
          new Tile
          {
            ID = 0,
            Image = new Image
            {
              Source = "Sprites/treeGreen_twigs.png",
              Width = 26,
              Height = 22,
              Format = ImageFormat.Png
            },
            Width = 26,
            Height = 22
          },
          new Tile
          {
            ID = 1,
            Image = new Image
            {
              Source = "Sprites/treeGreen_small.png",
              Width = 36,
              Height = 36,
              Format = ImageFormat.Png
            },
            Width = 36,
            Height = 36
          },
          new Tile
          {
            ID = 2,
            Image = new Image
            {
              Source = "Sprites/treeGreen_leaf.png",
              Width = 8,
              Height = 10,
              Format = ImageFormat.Png
            },
            Width = 8,
            Height = 10
          },
          new Tile
          {
            ID = 3,
            Image = new Image
            {
              Source = "Sprites/treeGreen_large.png",
              Width = 64,
              Height = 64,
              Format = ImageFormat.Png
            },
            Width = 64,
            Height = 64
          },
          new Tile
          {
            ID = 4,
            Image = new Image
            {
              Source = "Sprites/treeBrown_twigs.png",
              Width = 26,
              Height = 22,
              Format = ImageFormat.Png
            },
            Width = 26,
            Height = 22
          },
          new Tile
          {
            ID = 5,
            Image = new Image
            {
              Source = "Sprites/treeBrown_small.png",
              Width = 36,
              Height = 36,
              Format = ImageFormat.Png
            },
            Width = 36,
            Height = 36
          },
          new Tile
          {
            ID = 6,
            Image = new Image
            {
              Source = "Sprites/treeBrown_leaf.png",
              Width = 8,
              Height = 10,
              Format = ImageFormat.Png
            },
            Width = 8,
            Height = 10
          },
          new Tile
          {
            ID = 7,
            Image = new Image
            {
              Source = "Sprites/treeBrown_large.png",
              Width = 64,
              Height = 64,
              Format = ImageFormat.Png
            },
            Width = 64,
            Height = 64
          },
          new Tile
          {
            ID = 8,
            Image = new Image
            {
              Source = "Sprites/oilSpill_small.png",
              Width = 14,
              Height = 14,
              Format = ImageFormat.Png
            },
            Width = 14,
            Height = 14
          },
          new Tile
          {
            ID = 9,
            Image = new Image
            {
              Source = "Sprites/oilSpill_large.png",
              Width = 50,
              Height = 50,
              Format = ImageFormat.Png
            },
            Width = 50,
            Height = 50
          },
          new Tile
          {
            ID = 10,
            Image = new Image
            {
              Source = "Sprites/fenceYellow.png",
              Width = 52,
              Height = 16,
              Format = ImageFormat.Png
            },
            Width = 52,
            Height = 16,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 2,
              Objects = [
                new RectangleObject
                {
                  ID = 1,
                  X = 0,
                  Y = 0,
                  Width = 52,
                  Height = 16
                }
              ]
            }
          },
          new Tile
          {
            ID = 11,
            Image = new Image
            {
              Source = "Sprites/fenceRed.png",
              Width = 48,
              Height = 16,
              Format = ImageFormat.Png
            },
            Width = 48,
            Height = 16,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 2,
              Objects = [
                new RectangleObject
                {
                  ID = 1,
                  X = 0,
                  Y = 0,
                  Width = 48,
                  Height = 16
                }
              ]
            }
          },
          new Tile
          {
            ID = 12,
            Image = new Image
            {
              Source = "Sprites/crateWood.png",
              Width = 28,
              Height = 28,
              Format = ImageFormat.Png
            },
            Width = 28,
            Height = 28,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 2,
              Objects = [
                new RectangleObject
                {
                  ID = 2,
                  X = 0,
                  Y = 0,
                  Width = 28,
                  Height = 28
                }
              ]
            }
          },
          new Tile
          {
            ID = 13,
            Image = new Image
            {
              Source = "Sprites/crateMetal.png",
              Width = 28,
              Height = 28,
              Format = ImageFormat.Png
            },
            Width = 28,
            Height = 28,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 2,
              Objects = [
                new RectangleObject
                {
                  ID = 1,
                  X = 0,
                  Y = 0,
                  Width = 28,
                  Height = 28
                }
              ]
            }
          },
          new Tile
          {
            ID = 14,
            Image = new Image
            {
              Source = "Sprites/barrelRust_top.png",
              Width = 24,
              Height = 24,
              Format = ImageFormat.Png
            },
            Width = 24,
            Height = 24,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 5,
              Objects = [
                new EllipseObject
                {
                  ID = 4,
                  X = 0.212121f,
                  Y = 0.272727f,
                  Width = 23.667f,
                  Height = 23.667f
                }
              ]
            }
          },
          new Tile
          {
            ID = 15,
            Image = new Image
            {
              Source = "Sprites/barrelRed_top.png",
              Width = 24,
              Height = 24,
              Format = ImageFormat.Png
            },
            Width = 24,
            Height = 24,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 3,
              Objects = [
                new EllipseObject
                {
                  ID = 2,
                  X = 0.363636f,
                  Y = 0.272727f,
                  Width = 23.4545f,
                  Height = 23.7273f
                }
              ]
            }
          },
          new Tile
          {
            ID = 16,
            Image = new Image
            {
              Source = "Sprites/barrelGreen_top.png",
              Width = 24,
              Height = 24,
              Format = ImageFormat.Png
            },
            Width = 24,
            Height = 24,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 5,
              Objects = [
                new EllipseObject
                {
                  ID = 4,
                  X = 0.181818f,
                  Y = 0.272727f,
                  Width = 23.9091f,
                  Height = 23.5455f
                }
              ]
            }
          },
          new Tile
          {
            ID = 17,
            Image = new Image
            {
              Source = "Sprites/barrelBlack_top.png",
              Width = 24,
              Height = 24,
              Format = ImageFormat.Png
            },
            Width = 24,
            Height = 24,
            ObjectLayer = new ObjectLayer
            {
              DrawOrder = DrawOrder.Index,
              ID = 2,
              Objects = [
                new EllipseObject
                {
                  ID = 1,
                  X = -0.0909091f,
                  Y = 0.181818f,
                  Width = 24f,
                  Height = 24f
                }
              ]
            }
          }
        ]
  };

  public static Map VinhuiFirstgidProblemMap(string fileExt) => new Map
  {
    Class = "",
    Orientation = MapOrientation.Orthogonal,
    Width = 30,
    Height = 20,
    TileWidth = 64,
    TileHeight = 64,
    Infinite = true,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = TiledColor.Parse("#00000000", CultureInfo.InvariantCulture),
    Version = "1.10",
    TiledVersion = "1.11.2",
    NextLayerID = 5,
    NextObjectID = 81,
    Layers = [
      new TileLayer
      {
        ID = 1,
        Name = "Base",
        Width = 30,
        Height = 20,
        Data = new Data
        {
          Encoding = DataEncoding.Csv,
          Chunks = new Optional<Chunk[]>([
            new Chunk
            {
              X = 0,
              Y = 0,
              Width = 16,
              Height = 16,
              GlobalTileIDs = (uint[])[
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,14,3,3,3,3,7,3,3,3,3,3,3,3,15,1,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,2,1,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,4,3,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,2,1,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,2,1,
                1,2,1,1,1,1,4,3,3,3,3,3,3,3,5,1,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,2,1,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,2,1,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,2,1,
                1,2,1,1,1,1,2,1,1,1,1,1,1,1,4,3,
                1,4,3,3,3,3,6,3,3,15,1,1,1,1,2,1,
                1,2,1,1,1,1,1,1,1,2,1,1,1,1,2,1,
                1,2,1,1,1,1,1,1,1,2,1,1,1,1,2,1,
                1,16,3,3,3,3,3,3,3,6,3,3,3,3,17,1,
                11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11
              ],
              FlippingFlags = GenerateFlippingFlagsArray(16, 16, (x, y) => FlippingFlags.None)
            },
            new Chunk
            {
              X = 16,
              Y = 0,
              Width = 16,
              Height = 16,
              GlobalTileIDs = (uint[])[
                8,21,21,21,21,21,21,21,21,21,21,0,0,0,0,0,
                8,34,23,23,35,21,21,21,21,21,21,0,0,0,0,0,
                8,22,21,21,24,23,23,23,35,21,21,0,0,0,0,0,
                28,25,21,21,22,21,21,21,22,21,21,0,0,0,0,0,
                8,22,21,21,22,21,21,21,22,21,21,0,0,0,0,0,
                8,24,23,23,33,23,23,23,25,21,21,0,0,0,0,0,
                8,22,21,21,22,21,21,21,36,35,21,0,0,0,0,0,
                8,22,21,21,22,21,21,21,21,22,21,0,0,0,0,0,
                8,22,21,21,36,23,23,23,23,25,21,0,0,0,0,0,
                8,22,21,21,21,21,21,21,21,22,21,0,0,0,0,0,
                28,26,23,23,35,21,21,21,21,22,21,0,0,0,0,0,
                8,21,21,21,22,21,21,21,21,22,21,0,0,0,0,0,
                8,21,21,21,22,21,21,21,21,22,21,0,0,0,0,0,
                8,21,21,21,22,21,21,21,21,22,21,0,0,0,0,0,
                8,21,21,21,36,23,23,23,23,37,21,0,0,0,0,0,
                8,21,21,21,21,21,21,21,21,21,21,0,0,0,0,0
              ],
              FlippingFlags = GenerateFlippingFlagsArray(16, 16, (x, y) => FlippingFlags.None)
            }
          ])
        }
      },
      new ObjectLayer
      {
        ID = 2,
        Name = "Trees",
        Properties = [
          new FloatProperty { Name = "depth", Value = 0.2f }
        ],
        Objects = [
          new TileObject
          {
            ID = 1, GID = 44, X = 809.333f, Y = 201.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 2, GID = 44, X = 581.333f, Y = 265.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 3, GID = 44, X = 229.333f, Y = 198.667f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 4, GID = 44, X = 142.667f, Y = 350.667f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 5, GID = 44, X = 244, Y = 429.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 6, GID = 44, X = 162.667f, Y = 588, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 7, GID = 44, X = 297.333f, Y = 672, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 8, GID = 44, X = 157.333f, Y = 868, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 9, GID = 44, X = 457.333f, Y = 840, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 10, GID = 44, X = 768, Y = 829.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 11, GID = 44, X = 838.667f, Y = 565.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 12, GID = 44, X = 517.333f, Y = 529.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 13, GID = 44, X = 608, Y = 685.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 14, GID = 44, X = 745.333f, Y = 353.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 15, GID = 48, X = 1113.33f, Y = 925.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 16, GID = 48, X = 1533.33f, Y = 801.333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 17, GID = 48, X = 1158.67f, Y = 564, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 18, GID = 48, X = 1404, Y = 472, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 19, GID = 48, X = 1404, Y = 238.667f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 20, GID = 48, X = 1185.33f, Y = 234.667f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 21, GID = 48, X = 1564, Y = 93.3333f, Width = 64, Height = 64
          },
          new TileObject
          {
            ID = 22, GID = 42, X = 946, Y = 968.667f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 23, GID = 42, X = 856.667f, Y = 739.333f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 24, GID = 42, X = 687.333f, Y = 882, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 25, GID = 42, X = 344.667f, Y = 852.667f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 26, GID = 42, X = 242, Y = 547.333f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 27, GID = 42, X = 519.333f, Y = 331.333f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 28, GID = 42, X = 234, Y = 224.667f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 29, GID = 42, X = 475.333f, Y = 160.667f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 30, GID = 42, X = 690, Y = 172.667f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 31, GID = 42, X = 840.667f, Y = 264.667f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 32, GID = 42, X = 827.333f, Y = 363.333f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 33, GID = 42, X = 678, Y = 551.333f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 34, GID = 42, X = 506, Y = 668.667f, Width = 36, Height = 36
          },
          new TileObject
          {
            ID = 35, GID = 43, X = 688, Y = 335.667f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 36, GID = 43, X = 708, Y = 262.333f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 37, GID = 43, X = 608, Y = 533, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 38, GID = 43, X = 286.667f, Y = 530.333f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 39, GID = 43, X = 328, Y = 301, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 40, GID = 43, X = 170.667f, Y = 209, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 41, GID = 43, X = 278.667f, Y = 795.667f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 42, GID = 43, X = 744, Y = 735.667f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 43, GID = 43, X = 886.667f, Y = 870.333f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 44, GID = 43, X = 824, Y = 462.333f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 45, GID = 43, X = 353.333f, Y = 146.333f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 46, GID = 43, X = 141.333f, Y = 142.333f, Width = 8, Height = 10
          },
          new TileObject
          {
            ID = 47, GID = 43, X = 494.667f, Y = 222.333f, Width = 8, Height = 10
          }
        ]
      },
      new ObjectLayer
      {
        ID = 3,
        Name = "SpawnPoints",
        Objects = [
          new PointObject
          {
            ID = 73,
            X = 110,
            Y = 192
          },
          new PointObject
          {
            ID = 74,
            X = 102,
            Y = 594
          },
          new PointObject
          {
            ID = 75,
            X = 584,
            Y = 914
          },
          new PointObject
          {
            ID = 76,
            X = 874,
            Y = 240
          },
          new PointObject
          {
            ID = 77,
            X = 1346,
            Y = 210,
            Properties = [
              new FloatProperty { Name = "Rotation", Value = 10f }
            ]
          },
          new PointObject
          {
            ID = 78,
            X = 1596,
            Y = 586
          },
          new PointObject
          {
            ID = 79,
            X = 1236,
            Y = 620
          },
          new PointObject
          {
            ID = 80,
            X = 454,
            Y = 692
          }
        ]
      },
      new ObjectLayer
      {
        ID = 4,
        Name = "Crates",
        Objects = [
          new TileObject
          {
            ID = 62,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 552.667f,
            Y = 155.333f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 63,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 296f,
            Y = 290f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 64,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 153f,
            Y = 53f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 65,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 490f,
            Y = 378f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 67,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 845f,
            Y = 641f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 68,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 943f,
            Y = 680f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 69,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 146f,
            Y = 922f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 70,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 1626f,
            Y = 536f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 71,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 1248f,
            Y = 135f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          },
          new TileObject
          {
            ID = 72,
            Template = $"DestroyableCrate.{(fileExt == "tmx" ? "tx" : "tj")}",
            TemplateTileset = VinhuiObjectsTileset(1, fileExt),
            X = 1163f,
            Y = 637f,

            GID = 13,
            Width = 28,
            Height = 28,
            Properties = [
              new ClassProperty
              {
                Name = "Health",
                PropertyType = "Health",
                Value = [
                  new IntProperty { Name = "Current", Value = 20 },
                  new IntProperty { Name = "Max", Value = 20 }
                ]
              }
            ]
          }
        ]
      }
    ],
    Tilesets = [
      new Tileset
      {
        Version = "1.10",
        TiledVersion = "1.11.2",
        Name = "terrainTiles_default",
        TileWidth = 64,
        TileHeight = 64,
        TileCount = 40,
        Columns = 10,
        FirstGID = 1,
        Source = $"terrainTiles_default.{(fileExt == "tmx" ? "tsx" : "tsj")}",
        Image = new Image
        {
          Source = "Spritesheets/terrainTiles_default.png",
          Width = 640,
          Height = 256,
          Format = ImageFormat.Png
        }
      },
      VinhuiObjectsTileset(41, fileExt)
    ]
  };

  public static IReadOnlyCollection<ICustomTypeDefinition> VinhuiFirstgidProblemMapCustomTypeDefinitions() => [
    new CustomClassDefinition
    {
      ID = 1,
      Name = "Health",
      Members = [
        new IntProperty { Name = "Current", Value = 100 },
        new IntProperty { Name = "Max", Value = 100 }
      ],
      DrawFill = true,
      UseAs = CustomClassUseAs.Property | CustomClassUseAs.Object | CustomClassUseAs.Project
    }
  ];

}
