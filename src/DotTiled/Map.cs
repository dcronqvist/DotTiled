using System.Collections.Generic;
using System.Globalization;

namespace DotTiled;

/// <summary>
/// Map orientation enumeration. The map orientation determines the alignment of the tiles in the map.
/// </summary>
public enum MapOrientation
{
  /// <summary>
  /// Orthogonal orientation. This is the typical top-down grid-based layout.
  /// </summary>
  Orthogonal,

  /// <summary>
  /// Isometric orientation. This is a type of axonometric projection where the tiles are shown as rhombuses, as seen from a side-on view.
  /// </summary>
  Isometric,

  /// <summary>
  /// Staggered orientation. This is an isometric projection with a side-on view where the tiles are arranged in a staggered grid.
  /// </summary>
  Staggered,

  /// <summary>
  /// Hexagonal orientation. This is a type of axial projection where the tiles are shown as hexagons, as seen from a top-down view.
  /// </summary>
  Hexagonal
}

/// <summary>
/// Render order enumeration. The order in which tiles on tile layers are rendered.
/// </summary>
public enum RenderOrder
{
  /// <summary>
  /// Right-down render order. Starts at top-left and proceeds right then down.
  /// </summary>
  RightDown,

  /// <summary>
  /// Right-up render order. Starts at bottom-left and proceeds right then up.
  /// </summary>
  RightUp,

  /// <summary>
  /// Left-down render order. Starts at top-right and proceeds left then down.
  /// </summary>
  LeftDown,

  /// <summary>
  /// Left-up render order. Starts at bottom-right and proceeds left then up.
  /// </summary>
  LeftUp
}

/// <summary>
/// Stagger axis enumeration. For staggered and hexagonal maps, determines which axis (X or Y) is staggered.
/// </summary>
public enum StaggerAxis
{
  /// <summary>
  /// X stagger axis.
  /// </summary>
  X,

  /// <summary>
  /// Y stagger axis.
  /// </summary>
  Y
}

/// <summary>
/// Stagger index enumeration. For staggered and hexagonal maps, determines whether the "even" or "odd" indexes along the staggered axis are shifted.
/// </summary>
public enum StaggerIndex
{
  /// <summary>
  /// Even stagger index.
  /// </summary>
  Odd,

  /// <summary>
  /// Odd stagger index.
  /// </summary>
  Even
}

/// <summary>
/// Represents a Tiled map.
/// </summary>
public class Map : HasPropertiesBase
{
  /// <summary>
  /// The TMX format version. Is incremented to match minor Tiled releases.
  /// </summary>
  public required string Version { get; set; }

  /// <summary>
  /// The Tiled version used to save the file.
  /// </summary>
  public Optional<string> TiledVersion { get; set; } = Optional<string>.Empty;

  /// <summary>
  /// The class of this map.
  /// </summary>
  public string Class { get; set; } = "";

  /// <summary>
  /// Map orientation.
  /// </summary>
  public required MapOrientation Orientation { get; set; }

  /// <summary>
  /// The order in which tiles on tile layers are rendered.
  /// </summary>
  public RenderOrder RenderOrder { get; set; } = RenderOrder.RightDown;

  /// <summary>
  /// The compression level to use for tile layer data (defaults to -1, which means to use the algorithm default).
  /// Typically only useful for parsing, but may be interesting for certain use cases.
  /// </summary>
  public int CompressionLevel { get; set; } = -1;

  /// <summary>
  /// The width of the map in tiles.
  /// </summary>
  public required uint Width { get; set; }

  /// <summary>
  /// The height of the map in tiles.
  /// </summary>
  public required uint Height { get; set; }

  /// <summary>
  /// The width of a tile.
  /// </summary>
  public required uint TileWidth { get; set; }

  /// <summary>
  /// The height of a tile.
  /// </summary>
  public required uint TileHeight { get; set; }

  /// <summary>
  /// Only for hexagonal maps. Determines the width or height (depending on the staggered axis) of the tile's edge, in pixels.
  /// </summary>
  public Optional<uint> HexSideLength { get; set; } = Optional<uint>.Empty;

  /// <summary>
  /// For staggered and hexagonal maps, determines which axis (X or Y) is staggered.
  /// </summary>
  public Optional<StaggerAxis> StaggerAxis { get; set; } = Optional<StaggerAxis>.Empty;

  /// <summary>
  /// For staggered and hexagonal maps, determines whether the "even" or "odd" indexes along the staggered axis are shifted.
  /// </summary>
  public Optional<StaggerIndex> StaggerIndex { get; set; } = Optional<StaggerIndex>.Empty;

  /// <summary>
  /// X coordinate of the parallax origin in pixels.
  /// </summary>
  public float ParallaxOriginX { get; set; } = 0.0f;

  /// <summary>
  /// Y coordinate of the parallax origin in pixels.
  /// </summary>
  public float ParallaxOriginY { get; set; } = 0.0f;

  /// <summary>
  /// The background color of the map.
  /// </summary>
  public Color BackgroundColor { get; set; } = Color.Parse("#00000000", CultureInfo.InvariantCulture);

  /// <summary>
  /// Stores the next available ID for new layers. This number is used to prevent reuse of the same ID after layers have been removed.
  /// </summary>
  public required uint NextLayerID { get; set; }

  /// <summary>
  /// Stores the next available ID for new objects. This number is used to prevent reuse of the same ID after objects have been removed.
  /// </summary>
  public required uint NextObjectID { get; set; }

  /// <summary>
  /// Whether this map is infinite. An infinite map has no fixed size and can grow in all directions. Its layer data is stored in chunks.
  /// </summary>
  public bool Infinite { get; set; } = false;

  /// <summary>
  /// Map properties.
  /// </summary>
  public List<IProperty> Properties { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Properties;

  /// <summary>
  /// List of tilesets used by the map.
  /// </summary>
  public List<Tileset> Tilesets { get; set; } = [];

  /// <summary>
  /// Hierarchical list of layers. <see cref="Group"/> is a layer type which can contain sub-layers to create a hierarchy.
  /// </summary>
  public List<BaseLayer> Layers { get; set; } = [];
}
