namespace DotTiled;

/// <summary>
/// Orientation of the grid for the tiles in this tileset.
/// </summary>
public enum GridOrientation
{
  /// <summary>
  /// The grid is orthogonal.
  /// </summary>
  Orthogonal,

  /// <summary>
  /// The grid is isometric.
  /// </summary>
  Isometric
}

/// <summary>
/// Used to specify how tile overlays for terrain and collision information are rendered in isometric maps.
/// </summary>
public class Grid
{
  /// <summary>
  /// Orientation of the grid for the tiles in this tileset.
  /// </summary>
  public GridOrientation Orientation { get; set; } = GridOrientation.Orthogonal;

  /// <summary>
  /// Width of a grid cell.
  /// </summary>
  public required int Width { get; set; }

  /// <summary>
  /// Height of a grid cell.
  /// </summary>
  public required int Height { get; set; }
}
