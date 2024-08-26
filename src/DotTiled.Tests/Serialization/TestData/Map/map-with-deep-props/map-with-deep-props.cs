using System.Globalization;
using DotTiled.Model;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithDeepProps() => new Map
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
    Properties = [
      new ClassProperty
      {
        Name = "customouterclassprop",
        PropertyType = "CustomOuterClass",
        Value = [
          new ClassProperty
          {
            Name = "customclasspropinclass",
            PropertyType = "CustomClass",
            Value = [
              new BoolProperty { Name = "boolinclass", Value = false },
              new ColorProperty { Name = "colorinclass", Value = Color.Parse("#000000ff", CultureInfo.InvariantCulture) },
              new FileProperty { Name = "fileinclass", Value = "" },
              new FloatProperty { Name = "floatinclass", Value = 0f },
              new IntProperty { Name = "intinclass", Value = 0 },
              new ObjectProperty { Name = "objectinclass", Value = 0 },
              new StringProperty { Name = "stringinclass", Value = "" }
            ]
          }
        ]
      },
      new ClassProperty
      {
        Name = "customouterclasspropset",
        PropertyType = "CustomOuterClass",
        Value = [
          new ClassProperty
          {
            Name = "customclasspropinclass",
            PropertyType = "CustomClass",
            Value = [
              new BoolProperty { Name = "boolinclass", Value = true },
              new ColorProperty { Name = "colorinclass", Value = Color.Parse("#000000ff", CultureInfo.InvariantCulture) },
              new FileProperty { Name = "fileinclass", Value = "" },
              new FloatProperty { Name = "floatinclass", Value = 13.37f },
              new IntProperty { Name = "intinclass", Value = 0 },
              new ObjectProperty { Name = "objectinclass", Value = 0 },
              new StringProperty { Name = "stringinclass", Value = "" }
            ]
          }
        ]
      }
    ]
  };

  public static IReadOnlyCollection<ICustomTypeDefinition> MapWithDeepPropsCustomTypeDefinitions() => [
    new CustomClassDefinition
    {
      Name = "CustomClass",
      UseAs = CustomClassUseAs.Property,
      Members = [
        new BoolProperty
        {
          Name = "boolinclass",
          Value = false
        },
        new ColorProperty
        {
          Name = "colorinclass",
          Value = Color.Parse("#000000ff", CultureInfo.InvariantCulture)
        },
        new FileProperty
        {
          Name = "fileinclass",
          Value = ""
        },
        new FloatProperty
        {
          Name = "floatinclass",
          Value = 0f
        },
        new IntProperty
        {
          Name = "intinclass",
          Value = 0
        },
        new ObjectProperty
        {
          Name = "objectinclass",
          Value = 0
        },
        new StringProperty
        {
          Name = "stringinclass",
          Value = ""
        }
      ]
    },
    new CustomClassDefinition
    {
      Name = "CustomOuterClass",
      UseAs = CustomClassUseAs.Property,
      Members = [
        new ClassProperty
        {
          Name = "customclasspropinclass",
          PropertyType = "CustomClass",
          Value = [] // So no overrides of defaults in CustomClass
        }
      ]
    }
  ];
}
