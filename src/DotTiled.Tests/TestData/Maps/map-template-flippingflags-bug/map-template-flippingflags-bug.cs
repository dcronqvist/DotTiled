namespace DotTiled.Tests;

public partial class TestData
{
  public static Map MapTemplateFlippingFlagsBug(string ext) => new Map
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
    BackgroundColor = new TiledColor { R = 0, G = 0, B = 0, A = 0 },
    Version = "1.10",
    TiledVersion = "1.11.2",
    NextLayerID = 3,
    NextObjectID = 6,
    Tilesets = [
      new Tileset
      {
        FirstGID = 1,
        Source = ext == "tmx" ? "tileset.tsx" : "tileset.tsj",
        Version = "1.10",
        TiledVersion = "1.11.2",
        Name = "tileset",
        TileWidth = 32,
        TileHeight = 32,
        TileCount = 0,
        Columns = 4,
        Image = new Image
        {
          Source = "tileset.png",
          Width = 128,
          Height = 160,
          Format = ImageFormat.Png
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
          GlobalTileIDs = new uint[]
          {
            1,1,1,1,1,
            1,1,1,1,1,
            1,1,1,1,1,
            1,1,1,1,1,
            1,1,1,1,1
          },
          FlippingFlags = GetAllNoneFlippingFlags(5 * 5)
        }
      },
      new ObjectLayer
      {
        ID = 2,
        Name = "Object Layer 1",
        Objects = [
          // Object 1: Uses template as-is (gid=5 with FlippingFlags.HorizontalFlip only)
          new TileObject
          {
            ID = 1,
            Template = ext == "tmx" ? "template.tx" : "template.tj",
            TemplateTileset = new Tileset
            {
              FirstGID = 1,
              Source = ext == "tmx" ? "tileset.tsx" : "tileset.tsj",
              Version = "1.10",
              TiledVersion = "1.11.2",
              Name = "tileset",
              TileWidth = 32,
              TileHeight = 32,
              TileCount = 0,
              Columns = 4,
              Image = new Image
              {
                Source = "tileset.png",
                Width = 128,
                Height = 160,
                Format = ImageFormat.Png
              }
            },
            X = 16,
            Y = 32,
            Width = 32,
            Height = 32,
            GID = 5, // Comes from template when cleared of flags
            FlippingFlags = FlippingFlags.FlippedHorizontally, // Comes from template
            Properties = [
              new StringProperty { Name = "template_prop", Value = "from_template" }
            ]
          },
          // Object 2: Overrides with gid=1073741829 (VerticalFlip only)
          new TileObject
          {
            ID = 2,
            Template = ext == "tmx" ? "template.tx" : "template.tj",
            TemplateTileset = new Tileset
            {
              FirstGID = 1,
              Source = ext == "tmx" ? "tileset.tsx" : "tileset.tsj",
              Version = "1.10",
              TiledVersion = "1.11.2",
              Name = "tileset",
              TileWidth = 32,
              TileHeight = 32,
              TileCount = 0,
              Columns = 4,
              Image = new Image
              {
                Source = "tileset.png",
                Width = 128,
                Height = 160,
                Format = ImageFormat.Png
              }
            },
            X = 64,
            Y = 32,
            Width = 32,
            Height = 32,
            GID = 5,
            FlippingFlags = FlippingFlags.FlippedVertically, // Has been overriden with GID 1073741829 (VerticalFlip only), not the template's flags
            Properties = [
              new StringProperty { Name = "template_prop", Value = "from_template" }
            ]
          },
          // Object 3: Overrides with gid=3221225477 (HorizontalFlip | VerticalFlip)
          new TileObject
          {
            ID = 3,
            Template = ext == "tmx" ? "template.tx" : "template.tj",
            TemplateTileset = new Tileset
            {
              FirstGID = 1,
              Source = ext == "tmx" ? "tileset.tsx" : "tileset.tsj",
              Version = "1.10",
              TiledVersion = "1.11.2",
              Name = "tileset",
              TileWidth = 32,
              TileHeight = 32,
              TileCount = 0,
              Columns = 4,
              Image = new Image
              {
                Source = "tileset.png",
                Width = 128,
                Height = 160,
                Format = ImageFormat.Png
              }
            },
            X = 112,
            Y = 32,
            Width = 32,
            Height = 32,
            GID = 5,
            FlippingFlags = FlippingFlags.FlippedHorizontally | FlippingFlags.FlippedVertically, // This should be overridden, not the template's flags
            Properties = [
              new StringProperty { Name = "template_prop", Value = "from_template" }
            ]
          },
          // Object 4: Overrides with gid=2147483653 (Override with horizontal flip only)
          new TileObject
          {
            ID = 4,
            Template = ext == "tmx" ? "template.tx" : "template.tj",
            TemplateTileset = new Tileset
            {
              FirstGID = 1,
              Source = ext == "tmx" ? "tileset.tsx" : "tileset.tsj",
              Version = "1.10",
              TiledVersion = "1.11.2",
              Name = "tileset",
              TileWidth = 32,
              TileHeight = 32,
              TileCount = 0,
              Columns = 4,
              Image = new Image
              {
                Source = "tileset.png",
                Width = 128,
                Height = 160,
                Format = ImageFormat.Png
              }
            },
            X = 16,
            Y = 80,
            Width = 32,
            Height = 32,
            GID = 5,
            FlippingFlags = FlippingFlags.FlippedHorizontally, // This should be overridden, not the template's flags
            Properties = [
              new StringProperty { Name = "template_prop", Value = "from_template" }
            ]
          },
          // Object 5: Overrides with gid=5 (No flipping flags at all)
          new TileObject
          {
            ID = 5,
            Template = ext == "tmx" ? "template.tx" : "template.tj",
            TemplateTileset = new Tileset
            {
              FirstGID = 1,
              Source = ext == "tmx" ? "tileset.tsx" : "tileset.tsj",
              Version = "1.10",
              TiledVersion = "1.11.2",
              Name = "tileset",
              TileWidth = 32,
              TileHeight = 32,
              TileCount = 0,
              Columns = 4,
              Image = new Image
              {
                Source = "tileset.png",
                Width = 128,
                Height = 160,
                Format = ImageFormat.Png
              }
            },
            X = 64,
            Y = 80,
            Width = 32,
            Height = 32,
            GID = 5,
            FlippingFlags = FlippingFlags.None, // This should be overridden, not the template's flags
            Properties = [
              new StringProperty { Name = "template_prop", Value = "from_template" }
            ]
          }
        ]
      }
    ]
  };
}
