namespace DotTiled;

/// <summary>
/// Represents a Tiled template. A template is a reusable object that can be placed in an <see cref="DotTiled"/> inside the Tiled editor.
/// </summary>
public class Template
{
  /// <summary>
  /// If the template represents a tile object, this property will contain the tileset that the tile belongs to.
  /// </summary>
  public Optional<Tileset> Tileset { get; set; } = Optional<Tileset>.Empty;

  /// <summary>
  /// The object that this template represents.
  /// </summary>
  public required Object Object { get; set; }
}
