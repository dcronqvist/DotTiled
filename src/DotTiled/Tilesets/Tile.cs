using System.Collections.Generic;

namespace DotTiled;

/// <summary>
/// Represents a single tile in a tileset, when using a collection of images to represent the tileset.
/// <see href="https://doc.mapeditor.org/en/stable/reference/tmx-map-format/#tile">Tiled documentation for Tileset tiles</see>
/// </summary>
public class Tile : HasPropertiesBase
{
  /// <summary>
  /// The local tile ID within its tileset.
  /// </summary>
  public required uint ID { get; set; }

  /// <summary>
  /// The class of the tile. Is inherited by tile objects
  /// </summary>
  public string Type { get; set; } = "";

  /// <summary>
  /// A percentage indicating the probability that this tile is chosen when it competes with others while editing with the terrain tool.
  /// </summary>
  public float Probability { get; set; } = 0f;

  /// <summary>
  /// The X position of the sub-rectangle representing this tile within the tileset image.
  /// </summary>
  public int X { get; set; } = 0;

  /// <summary>
  /// The Y position of the sub-rectangle representing this tile within the tileset image.
  /// </summary>
  public int Y { get; set; } = 0;

  /// <summary>
  /// The width of the sub-rectangle representing this tile within the tileset image.
  /// </summary>
  public required int Width { get; set; }

  /// <summary>
  /// The height of the sub-rectangle representing this tile within the tileset image.
  /// </summary>
  public required int Height { get; set; }

  /// <summary>
  /// Tile properties.
  /// </summary>
  public List<IProperty> Properties { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Properties;

  /// <summary>
  /// The image representing this tile. Only used for tilesets that composed of a collection of images.
  /// </summary>
  public Optional<Image> Image { get; set; } = Optional.Empty;

  /// <summary>
  /// Used when the tile contains e.g. collision information.
  /// </summary>
  public Optional<ObjectLayer> ObjectLayer { get; set; } = Optional.Empty;

  /// <summary>
  /// The animation frames for this tile.
  /// </summary>
  public List<Frame> Animation { get; set; } = [];
}
