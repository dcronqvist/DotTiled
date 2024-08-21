namespace DotTiled.Model;

/// <summary>
/// Represents a Tiled template. A template is a reusable object that can be placed in an <see cref="DotTiled.Model"/> inside the Tiled editor.
/// </summary>
public class Template
{
  /// <summary>
  /// If the template represents a tile object, this property will contain the tileset that the tile belongs to.
  /// </summary>
  public Tileset? Tileset { get; set; }

  /// <summary>
  /// The object that this template represents.
  /// </summary>
  public required Object Object { get; set; }
}
