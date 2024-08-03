namespace DotTiled.Tests;

public partial class TmxMapReaderTests
{
  private static Map MapWithObjectTemplate() => new Map
  {
    Version = "1.10",
    TiledVersion = "1.11.0",
    Orientation = MapOrientation.Orthogonal,
    RenderOrder = RenderOrder.RightDown,
    Width = 5,
    Height = 5,
    TileWidth = 32,
    TileHeight = 32,
    Infinite = false,
    NextLayerID = 3,
    NextObjectID = 3,
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
          GlobalTileIDs = [
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0,
            0,0,0,0,0
          ],
          FlippingFlags = [
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
          ]
        }
      },
      new ObjectLayer
      {
        ID = 2,
        Name = "Object Layer 1",
        Objects = [
          new RectangleObject
          {
            ID = 1,
            Template = "map-with-object-template.tx",
            Name = "Thingy 2",
            X = 94.5749f,
            Y = 33.6842f,
            Width = 37.0156f,
            Height = 37.0156f,
            Properties = new Dictionary<string, IProperty>
            {
              ["Bool"] = new BoolProperty { Name = "Bool", Value = true },
              ["TestClassInTemplate"] = new ClassProperty
              {
                Name = "TestClassInTemplate",
                PropertyType = "TestClass",
                Properties = new Dictionary<string, IProperty>
                {
                  ["Amount"] = new FloatProperty { Name = "Amount", Value = 37 },
                  ["Name"] = new StringProperty { Name = "Name", Value = "I am here" }
                }
              }
            }
          },
          new RectangleObject
          {
            ID = 2,
            Template = "map-with-object-template.tx",
            Name = "Thingy",
            X = 29.7976f,
            Y = 33.8693f,
            Width = 37.0156f,
            Height = 37.0156f,
            Properties = new Dictionary<string, IProperty>
            {
              ["Bool"] = new BoolProperty { Name = "Bool", Value = true },
              ["TestClassInTemplate"] = new ClassProperty
              {
                Name = "TestClassInTemplate",
                PropertyType = "TestClass",
                Properties = new Dictionary<string, IProperty>
                {
                  ["Amount"] = new FloatProperty { Name = "Amount", Value = 4.2f },
                  ["Name"] = new StringProperty { Name = "Name", Value = "Hello there" }
                }
              }
            }
          },
          new RectangleObject
          {
            ID = 3,
            Template = "map-with-object-template.tx",
            Name = "Thingy 3",
            X = 5,
            Y = 5,
            Width = 37.0156f,
            Height = 37.0156f,
            Properties = new Dictionary<string, IProperty>
            {
              ["Bool"] = new BoolProperty { Name = "Bool", Value = true },
              ["TestClassInTemplate"] = new ClassProperty
              {
                Name = "TestClassInTemplate",
                PropertyType = "TestClass",
                Properties = new Dictionary<string, IProperty>
                {
                  ["Amount"] = new FloatProperty { Name = "Amount", Value = 4.2f },
                  ["Name"] = new StringProperty { Name = "Name", Value = "I am here 3" }
                }
              }
            }
          }
        ]
      }
    ]
  };
}
