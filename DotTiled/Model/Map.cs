using System.Collections.Generic;
using System.Globalization;

namespace DotTiled;

public enum MapOrientation
{
  Orthogonal,
  Isometric,
  Staggered,
  Hexagonal
}

public enum RenderOrder
{
  RightDown,
  RightUp,
  LeftDown,
  LeftUp
}

public enum StaggerAxis
{
  X,
  Y
}

public enum StaggerIndex
{
  Odd,
  Even
}

public class Map
{
  // Attributes
  public required string Version { get; set; }
  public required string TiledVersion { get; set; }
  public string Class { get; set; } = "";
  public required MapOrientation Orientation { get; set; }
  public RenderOrder RenderOrder { get; set; } = RenderOrder.RightDown;
  public int CompressionLevel { get; set; } = -1;
  public required uint Width { get; set; }
  public required uint Height { get; set; }
  public required uint TileWidth { get; set; }
  public required uint TileHeight { get; set; }
  public uint? HexSideLength { get; set; }
  public StaggerAxis? StaggerAxis { get; set; }
  public StaggerIndex? StaggerIndex { get; set; }
  public float ParallaxOriginX { get; set; } = 0.0f;
  public float ParallaxOriginY { get; set; } = 0.0f;
  public Color BackgroundColor { get; set; } = Color.Parse("#00000000", CultureInfo.InvariantCulture);
  public required uint NextLayerID { get; set; }
  public required uint NextObjectID { get; set; }
  public bool Infinite { get; set; } = false;

  // At most one of
  public Dictionary<string, IProperty>? Properties { get; set; }

  // Any number of
  public List<Tileset> Tilesets { get; set; } = [];
  public List<BaseLayer> Layers { get; set; } = [];
  public List<Group> Groups { get; set; } = [];
}
