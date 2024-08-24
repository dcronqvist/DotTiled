using System.Globalization;
using DotTiled.Model;

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
    HexSideLength = null,
    StaggerAxis = null,
    StaggerIndex = null,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = Color.Parse("#00ff00", CultureInfo.InvariantCulture),
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
          Chunks = null,
          Compression = null,
          GlobalTileIDs = [
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0,
            0, 0, 0, 0, 0
          ],
          FlippingFlags = [
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
          ]
        }
      }
    ],
    Properties = new Dictionary<string, IProperty>
    {
      ["boolprop"] = new BoolProperty { Name = "boolprop", Value = true },
      ["colorprop"] = new ColorProperty { Name = "colorprop", Value = Color.Parse("#ff55ffff", CultureInfo.InvariantCulture) },
      ["fileprop"] = new FileProperty { Name = "fileprop", Value = "file.txt" },
      ["floatprop"] = new FloatProperty { Name = "floatprop", Value = 4.2f },
      ["intprop"] = new IntProperty { Name = "intprop", Value = 8 },
      ["objectprop"] = new ObjectProperty { Name = "objectprop", Value = 5 },
      ["stringprop"] = new StringProperty { Name = "stringprop", Value = "This is a string, hello world!" }
    }
  };
}
