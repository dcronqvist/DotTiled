namespace DotTiled.Tests;

public partial class TmxMapReaderTests
{
  private static Map MapWithGroup() => new Map
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
    NextLayerID = 5,
    NextObjectID = 2,
    Layers = [
      new TileLayer
      {
        ID = 4,
        Name = "Tile Layer 2",
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
      new Group
      {
        ID = 3,
        Name = "Group 1",
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
                Name = "Name",
                X = 35.5f,
                Y = 26,
                Width = 64.5f,
                Height = 64.5f,
              }
            ]
          }
        ]
      }
    ]
  };
}
