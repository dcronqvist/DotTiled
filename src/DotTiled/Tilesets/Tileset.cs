using System;
using System.Collections.Generic;
using System.Linq;

namespace DotTiled;

/// <summary>
/// The alignment of tile objects.
/// </summary>
public enum ObjectAlignment
{
  /// <summary>
  /// The alignment is unspecified. Tile objects will use <see cref="BottomLeft"/> in orthogonal maps, and <see cref="Bottom"/> in isometric maps.
  /// </summary>
  Unspecified,

  /// <summary>
  /// The tile object is aligned to the top left of the tile.
  /// </summary>
  TopLeft,

  /// <summary>
  /// The tile object is aligned to the top of the tile.
  /// </summary>
  Top,

  /// <summary>
  /// The tile object is aligned to the top right of the tile.
  /// </summary>
  TopRight,

  /// <summary>
  /// The tile object is aligned to the left of the tile.
  /// </summary>
  Left,

  /// <summary>
  /// The tile object is aligned to the center of the tile.
  /// </summary>
  Center,

  /// <summary>
  /// The tile object is aligned to the right of the tile.
  /// </summary>
  Right,

  /// <summary>
  /// The tile object is aligned to the bottom left of the tile.
  /// </summary>
  BottomLeft,

  /// <summary>
  /// The tile object is aligned to the bottom of the tile.
  /// </summary>
  Bottom,

  /// <summary>
  /// The tile object is aligned to the bottom right of the tile.
  /// </summary>
  BottomRight
}

/// <summary>
/// The size to use when rendering tiles from a tileset on a tile layer.
/// </summary>
public enum TileRenderSize
{
  /// <summary>
  /// The tile is drawn at the size of the tile in the tileset.
  /// </summary>
  Tile,

  /// <summary>
  /// The tile is drawn at the tile grid size of the map.
  /// </summary>
  Grid
}

/// <summary>
/// Determines how a tile is rendered in a tile set.
/// </summary>
public enum FillMode
{
  /// <summary>
  /// The tile is stretched to fill the tile size, possibly distorting the tile.
  /// </summary>
  Stretch,

  /// <summary>
  /// The tile's aspect ratio is preserved, and it is scaled to fit within the tile size.
  /// </summary>
  PreserveAspectFit
}

/// <summary>
/// A helper class to specify where in a tileset image a tile is located.
/// </summary>
public class SourceRectangle
{
  /// <summary>
  /// The X coordinate of the tile in the tileset image.
  /// </summary>
  public int X { get; set; } = 0;

  /// <summary>
  /// The Y coordinate of the tile in the tileset image.
  /// </summary>
  public int Y { get; set; } = 0;

  /// <summary>
  /// The width of the tile in the tileset image.
  /// </summary>
  public int Width { get; set; } = 0;

  /// <summary>
  /// The height of the tile in the tileset image.
  /// </summary>
  public int Height { get; set; } = 0;
}

/// <summary>
/// A tileset is a collection of tiles that can be used in a tile layer, or by tile objects.
/// </summary>
public class Tileset : HasPropertiesBase
{
  /// <summary>
  /// The TMX format version. Is incremented to match minor Tiled releases.
  /// </summary>
  public Optional<string> Version { get; set; }

  /// <summary>
  /// The Tiled version used to save the file in case it was loaded from an external tileset file.
  /// </summary>
  public Optional<string> TiledVersion { get; set; } = Optional.Empty;

  /// <summary>
  /// The first global tile ID of this tileset (this global ID maps to the first tile in this tileset).
  /// </summary>
  public Optional<uint> FirstGID { get; set; } = Optional.Empty;

  /// <summary>
  /// If this tileset is stored in an external TSX (Tile Set XML) file, this attribute refers to that file.
  /// </summary>
  public Optional<string> Source { get; set; } = Optional.Empty;

  /// <summary>
  /// The name of this tileset.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The class of this tileset.
  /// </summary>
  public string Class { get; set; } = "";

  /// <summary>
  /// The width of the tiles in this tileset, which should be at least 1 (non-zero) except in the case of image collection tilesets (in which case it stores the maximum tile width).
  /// </summary>
  public required int TileWidth { get; set; }

  /// <summary>
  /// The height of the tiles in this tileset, which should be at least 1 (non-zero) except in the case of image collection tilesets (in which case it stores the maximum tile height).
  /// </summary>
  public required int TileHeight { get; set; }

  /// <summary>
  /// The spacing in pixels between the tiles in this tileset (applies to the tileset image). Irrelevant for image collection tilesets.
  /// </summary>
  public int Spacing { get; set; } = 0;

  /// <summary>
  /// The margin around the tiles in this tileset (applies to the tileset image). Irrelevant for image collection tilesets.
  /// </summary>
  public int Margin { get; set; } = 0;

  /// <summary>
  /// The number of tiles in this tileset.
  /// </summary>
  public required int TileCount { get; set; }

  /// <summary>
  /// The number of tile columns in the tileset.
  /// </summary>
  public required int Columns { get; set; }

  /// <summary>
  /// Controls the aligntment for tile objects.
  /// </summary>
  public ObjectAlignment ObjectAlignment { get; set; } = ObjectAlignment.Unspecified;

  /// <summary>
  /// The size to use when rendering tiles from thie tileset on a tile layer. When set to <see cref="TileRenderSize.Grid"/>, the tile is drawn at the tile grid size of the map.
  /// </summary>
  public TileRenderSize RenderSize { get; set; } = TileRenderSize.Tile;

  /// <summary>
  /// The fill mode to use when rendering tiles from this tileset.
  /// </summary>
  public FillMode FillMode { get; set; } = FillMode.Stretch;

  /// <summary>
  /// If the tileset is based on a single image, which is cut into tiles based on the given attributes of the tileset, then this is that image.
  /// </summary>
  public Optional<Image> Image { get; set; } = Optional.Empty;

  /// <summary>
  /// This is used to specify an offset in pixels, to be applied when drawing a tile from the related tileset. When not present, no offset is applied.
  /// </summary>
  public Optional<TileOffset> TileOffset { get; set; } = Optional.Empty;

  /// <summary>
  /// Ths is only used in case of isometric orientation, and determines how tile overlays for terrain and collision information are rendered.
  /// </summary>
  public Optional<Grid> Grid { get; set; } = Optional.Empty;

  /// <summary>
  /// Tileset properties.
  /// </summary>
  public List<IProperty> Properties { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Properties;

  // public List<Terrain>? TerrainTypes { get; set; } TODO: Implement Terrain -> Wangset conversion during deserialization

  /// <summary>
  /// Contains the list of Wang sets defined for this tileset.
  /// </summary>
  public List<Wangset> Wangsets { get; set; } = [];

  /// <summary>
  /// Used to describe which transformations can be applied to the tiles (e.g. to extend a Wang set by transforming existing tiles).
  /// </summary>
  public Optional<Transformations> Transformations { get; set; } = Optional.Empty;

  /// <summary>
  /// If this tileset is based on a collection of images, then this list of tiles will contain the individual images that make up the tileset.
  /// </summary>
  public List<Tile> Tiles { get; set; } = [];

  /// <summary>
  /// Returns the source rectangle for a tile in this tileset given its local tile ID.
  /// </summary>
  /// <param name="localTileID">The local tile ID of the tile.</param>
  /// <returns>A source rectangle describing the tile's position in the tileset's </returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown when the local tile ID is out of range.</exception>
  public SourceRectangle GetSourceRectangleForLocalTileID(uint localTileID)
  {
    if (localTileID >= TileCount)
      throw new ArgumentException("The local tile ID is out of range.", nameof(localTileID));

    var tileInTiles = Tiles.FirstOrDefault(t => t.ID == localTileID);
    if (tileInTiles != null)
    {
      return new SourceRectangle
      {
        X = tileInTiles.X,
        Y = tileInTiles.Y,
        Width = tileInTiles.Width,
        Height = tileInTiles.Height
      };
    }

    var column = (int)(localTileID % Columns);
    var row = (int)(localTileID / Columns);

    var x = Margin + ((TileWidth + Spacing) * column);
    var y = Margin + ((TileHeight + Spacing) * row);

    return new SourceRectangle
    {
      X = x,
      Y = y,
      Width = TileWidth,
      Height = TileHeight
    };
  }
}
