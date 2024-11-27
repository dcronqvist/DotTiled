using System.Globalization;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithCustomTypePropsWithoutDefs() => new Map
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
      new ClassProperty
      {
        Name = "customclassprop",
        PropertyType = "CustomClass",
        Value = [
          new BoolProperty { Name = "boolinclass", Value = true },
          new FloatProperty { Name = "floatinclass", Value = 13.37f },
          new StringProperty { Name = "stringinclass", Value = "This is a set string" }
        ]
      },
      new IntProperty
      {
        Name = "customenumintflagsprop",
        Value = 6
      },
      new IntProperty
      {
        Name = "customenumintprop",
        Value = 3
      },
      new StringProperty
      {
        Name = "customenumstringprop",
        Value = "CustomEnumString_2"
      },
      new StringProperty
      {
        Name = "customenumstringflagsprop",
        Value = "CustomEnumStringFlags_1,CustomEnumStringFlags_2"
      }
    ]
  };
}
