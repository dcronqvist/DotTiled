namespace DotTiled.Tests;

public partial class TestData
{
  public static Map SimpleMapWithEmbeddedTileset() => new Map
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
    Tilesets = [
      new Tileset
      {
        FirstGID = 1,
        Name = "Tileset 1",
        TileWidth = 32,
        TileHeight = 32,
        TileCount = 8,
        Columns = 4,
        Image = new Image
        {
          Format = ImageFormat.Png,
          Source = "tiles.png",
          Width = 128,
          Height = 64
        }
      }
    ],
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
          Compression = null,
          GlobalTileIDs = [
            1,1,1,1,1,
            1,1,1,1,1,
            1,1,1,1,1,
            2,2,2,2,2,
            2,2,2,2,2
          ],
          FlippingFlags = [
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
            FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
          ]
        },
      }
    ]
  };
}
