namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapDuplicateObjectIdBug(string ext) => new Map
  {
    Class = "",
    Orientation = MapOrientation.Orthogonal,
    Width = 64,
    Height = 64,
    TileWidth = 16,
    TileHeight = 16,
    Infinite = true,
    ParallaxOriginX = 0,
    ParallaxOriginY = 0,
    RenderOrder = RenderOrder.RightDown,
    CompressionLevel = -1,
    BackgroundColor = new TiledColor { R = 0, G = 0, B = 0, A = 0 },
    Version = "1.10",
    TiledVersion = "1.11.2",
    NextLayerID = 2,
    NextObjectID = 3,
    Tilesets = [
      new Tileset
      {
        FirstGID = 1,
        Source = ext == "tmx" ? "tiles.tsx" : "tiles.tsj",
        Version = "1.10",
        TiledVersion = "1.11.2",
        Name = "Tiles",
        TileWidth = 16,
        TileHeight = 16,
        TileCount = 4,
        Columns = 2,
        Grid = new Grid
        {
          Orientation = GridOrientation.Orthogonal,
          Width = 32,
          Height = 32
        },
        Image = new Image
        {
          Source = "tiles.png",
          Width = 32,
          Height = 32,
          Format = ImageFormat.Png
        }
      }
    ],
    Layers = [
      new TileLayer
      {
        ID = 1,
        Name = "Tile Layer 1",
        Width = ext == "tmx" ? 64 : 16,
        Height = ext == "tmx" ? 64 : 16,
        Data = new Data
        {
          Encoding = DataEncoding.Csv,
          Chunks = new Optional<Chunk[]>([
            new Chunk
            {
              X = 0,
              Y = 0,
              Width = 16,
              Height = 16,
              GlobalTileIDs = [
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
              ],
              FlippingFlags = GetAllNoneFlippingFlags(16 * 16)
            }
          ])
        }
      },
      new ObjectLayer
      {
        ID = 3,
        Name = "Object Layer 1",
        Objects = [
          new TileObject
          {
            ID = 1,
            Template = ext == "tmx" ? "template.tx" : "template.tj",
            TemplateTileset = new Tileset
            {
              FirstGID = 1,
              Source = ext == "tmx" ? "tiles.tsx" : "tiles.tsj",
              Version = "1.10",
              TiledVersion = "1.11.2",
              Name = "Tiles",
              TileWidth = 16,
              TileHeight = 16,
              TileCount = 4,
              Columns = 2,
              Grid = new Grid
              {
                Orientation = GridOrientation.Orthogonal,
                Width = 32,
                Height = 32
              },
              Image = new Image
              {
                Source = "tiles.png",
                Width = 32,
                Height = 32,
                Format = ImageFormat.Png
              }
            },
            X = 80,
            Y = 144,

            GID = 4,
            Width = 16,
            Height = 16,
          },
          new TileObject
          {
            ID = 2,
            Template = ext == "tmx" ? "template.tx" : "template.tj",
            TemplateTileset = new Tileset
            {
              FirstGID = 1,
              Source = ext == "tmx" ? "tiles.tsx" : "tiles.tsj",
              Version = "1.10",
              TiledVersion = "1.11.2",
              Name = "Tiles",
              TileWidth = 16,
              TileHeight = 16,
              TileCount = 4,
              Columns = 2,
              Grid = new Grid
              {
                Orientation = GridOrientation.Orthogonal,
                Width = 32,
                Height = 32
              },
              Image = new Image
              {
                Source = "tiles.png",
                Width = 32,
                Height = 32,
                Format = ImageFormat.Png
              }
            },
            X = 48,
            Y = 144,

            GID = 4,
            Width = 16,
            Height = 16,
          }
        ]
      }
    ]
  };

  private static FlippingFlags[] GetAllNoneFlippingFlags(int count)
  {
    var flippingFlags = new FlippingFlags[count];
    for (int i = 0; i < count; i++)
    {
      flippingFlags[i] = FlippingFlags.None;
    }
    return flippingFlags;
  }
}
