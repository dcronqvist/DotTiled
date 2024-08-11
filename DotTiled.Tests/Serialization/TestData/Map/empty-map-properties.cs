namespace DotTiled.Tests;

public partial class TestData
{
  public static Map EmptyMapWithProperties() => new Map
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
      }
    ],
    Properties = new Dictionary<string, IProperty>
    {
      ["MapBool"] = new BoolProperty { Name = "MapBool", Value = true },
      ["MapColor"] = new ColorProperty { Name = "MapColor", Value = new Color { R = 255, G = 0, B = 0, A = 255 } },
      ["MapFile"] = new FileProperty { Name = "MapFile", Value = "file.png" },
      ["MapFloat"] = new FloatProperty { Name = "MapFloat", Value = 5.2f },
      ["MapInt"] = new IntProperty { Name = "MapInt", Value = 42 },
      ["MapObject"] = new ObjectProperty { Name = "MapObject", Value = 5 },
      ["MapString"] = new StringProperty { Name = "MapString", Value = "string in map" }
    }
  };
}
