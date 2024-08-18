using System.Collections.Generic;

namespace DotTiled.Model;

public enum ObjectAlignment
{
  Unspecified,
  TopLeft,
  Top,
  TopRight,
  Left,
  Center,
  Right,
  BottomLeft,
  Bottom,
  BottomRight
}

public enum TileRenderSize
{
  Tile,
  Grid
}

public enum FillMode
{
  Stretch,
  PreserveAspectFit
}

public class Tileset
{
  // Attributes
  public string? Version { get; set; }
  public string? TiledVersion { get; set; }
  public uint? FirstGID { get; set; }
  public string? Source { get; set; }
  public string? Name { get; set; }
  public string Class { get; set; } = "";
  public uint? TileWidth { get; set; }
  public uint? TileHeight { get; set; }
  public float? Spacing { get; set; } = 0f;
  public float? Margin { get; set; } = 0f;
  public uint? TileCount { get; set; }
  public uint? Columns { get; set; }
  public ObjectAlignment ObjectAlignment { get; set; } = ObjectAlignment.Unspecified;
  public TileRenderSize RenderSize { get; set; } = TileRenderSize.Tile;
  public FillMode FillMode { get; set; } = FillMode.Stretch;

  // At most one of
  public Image? Image { get; set; }
  public TileOffset? TileOffset { get; set; }
  public Grid? Grid { get; set; }
  public Dictionary<string, IProperty>? Properties { get; set; }
  // public List<Terrain>? TerrainTypes { get; set; } TODO: Implement Terrain -> Wangset conversion during deserialization
  public List<Wangset>? Wangsets { get; set; }
  public Transformations? Transformations { get; set; }

  // Any number of
  public List<Tile> Tiles { get; set; } = [];
}
