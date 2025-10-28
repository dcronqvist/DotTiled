using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithCommonProps() => new Map
  {
    Class = "",
    Orientation = MapOrientation.Isometric,
    Width = 5,
    Height = 5,
    TileWidth = 32,
    TileHeight = 16,
    Infinite = false,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = TiledColor.Parse("#00ff00", CultureInfo.InvariantCulture),
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
    Properties =
    [
      new BoolProperty { Name = "boolprop", Value = true },
      new ColorProperty { Name = "colorprop", Value = TiledColor.Parse("#ff55ffff", CultureInfo.InvariantCulture) },
      new FileProperty { Name = "fileprop", Value = "file.txt" },
      new FloatProperty { Name = "floatprop", Value = 4.2f },
      new IntProperty { Name = "intprop", Value = 8 },
      new ObjectProperty { Name = "objectprop", Value = 5 },
      new StringProperty { Name = "stringprop", Value = "This is a string, hello world!" },
      new ColorProperty { Name = "unsetcolorprop", Value = Optional.Empty }
    ]
  };
}
