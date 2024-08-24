using System.Globalization;
using DotTiled.Model;

namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapWithCustomTypeProps() => new Map
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
    Properties = new Dictionary<string, IProperty>
    {
      ["customclassprop"] = new ClassProperty
      {
        Name = "customclassprop",
        PropertyType = "CustomClass",
        Properties = new Dictionary<string, IProperty>
        {
          ["boolinclass"] = new BoolProperty { Name = "boolinclass", Value = true },
          ["colorinclass"] = new ColorProperty { Name = "colorinclass", Value = Color.Parse("#000000ff", CultureInfo.InvariantCulture) },
          ["fileinclass"] = new FileProperty { Name = "fileinclass", Value = "" },
          ["floatinclass"] = new FloatProperty { Name = "floatinclass", Value = 13.37f },
          ["intinclass"] = new IntProperty { Name = "intinclass", Value = 0 },
          ["objectinclass"] = new ObjectProperty { Name = "objectinclass", Value = 0 },
          ["stringinclass"] = new StringProperty { Name = "stringinclass", Value = "This is a set string" }
        }
      }
    }
  };

  // This comes from map-with-custom-type-props/propertytypes.json
  public static IReadOnlyCollection<CustomTypeDefinition> MapWithCustomTypePropsCustomTypeDefinitions() => [
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
    }
  ];
}
