namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithClass() => new Map
  {
    Class = "TestClass",
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
    BackgroundColor = new Color { R = 0, G = 0, B = 0, A = 0 },
    Version = "1.10",
    TiledVersion = "1.11.0",
    NextLayerID = 2,
    NextObjectID = 1,
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
    ],
    Properties = [
      new BoolProperty
      {
        Name = "classbool",
        Value = true
      },
      new StringProperty
      {
        Name = "classstring",
        Value = "Hello there default value"
      }
    ]
  };

  public static IReadOnlyCollection<ICustomTypeDefinition> MapWithClassCustomTypeDefinitions() => [
    new CustomClassDefinition
    {
      Name = "TestClass",
      UseAs = CustomClassUseAs.Map,
      Members = [
        new BoolProperty
        {
          Name = "classbool",
          Value = true
        },
        new StringProperty
        {
          Name = "classstring",
          Value = "Hello there default value"
        }
      ]
    },
  ];
}
