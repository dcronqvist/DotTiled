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
    Properties = [
      new ClassProperty
      {
        Name = "customclassprop",
        PropertyType = "CustomClass",
        Value = [
          new BoolProperty { Name = "boolinclass", Value = true },
          new ColorProperty { Name = "colorinclass", Value = Color.Parse("#000000ff", CultureInfo.InvariantCulture) },
          new FileProperty { Name = "fileinclass", Value = "" },
          new FloatProperty { Name = "floatinclass", Value = 13.37f },
          new IntProperty { Name = "intinclass", Value = 0 },
          new ObjectProperty { Name = "objectinclass", Value = 0 },
          new StringProperty { Name = "stringinclass", Value = "This is a set string" }
        ]
      },
      new EnumProperty
      {
        Name = "customenumstringprop",
        PropertyType = "CustomEnumString",
        Value = new HashSet<string> { "CustomEnumString_2" }
      },
      new EnumProperty
      {
        Name = "customenumstringflagsprop",
        PropertyType = "CustomEnumStringFlags",
        Value = new HashSet<string> { "CustomEnumStringFlags_1", "CustomEnumStringFlags_2" }
      },
      new EnumProperty
      {
        Name = "customenumintprop",
        PropertyType = "CustomEnumInt",
        Value = new HashSet<string> { "CustomEnumInt_4" }
      },
      new EnumProperty
      {
        Name = "customenumintflagsprop",
        PropertyType = "CustomEnumIntFlags",
        Value = new HashSet<string> { "CustomEnumIntFlags_2", "CustomEnumIntFlags_3" }
      }
    ]
  };

  // This comes from map-with-custom-type-props/propertytypes.json
  public static IReadOnlyCollection<ICustomTypeDefinition> MapWithCustomTypePropsCustomTypeDefinitions() => [
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
    new CustomEnumDefinition
    {
      Name = "CustomEnumString",
      StorageType = CustomEnumStorageType.String,
      ValueAsFlags = false,
      Values = [
        "CustomEnumString_1",
        "CustomEnumString_2",
        "CustomEnumString_3"
      ]
    },
    new CustomEnumDefinition
    {
      Name = "CustomEnumStringFlags",
      StorageType = CustomEnumStorageType.String,
      ValueAsFlags = true,
      Values = [
        "CustomEnumStringFlags_1",
        "CustomEnumStringFlags_2"
      ]
    },
    new CustomEnumDefinition
    {
      Name = "CustomEnumInt",
      StorageType = CustomEnumStorageType.Int,
      ValueAsFlags = false,
      Values = [
        "CustomEnumInt_1",
        "CustomEnumInt_2",
        "CustomEnumInt_3",
        "CustomEnumInt_4",
      ]
    },
    new CustomEnumDefinition
    {
      Name = "CustomEnumIntFlags",
      StorageType = CustomEnumStorageType.Int,
      ValueAsFlags = true,
      Values = [
        "CustomEnumIntFlags_1",
        "CustomEnumIntFlags_2",
        "CustomEnumIntFlags_3"
      ]
    }
  ];
}
