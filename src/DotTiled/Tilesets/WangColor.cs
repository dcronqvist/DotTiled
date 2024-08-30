using System.Collections.Generic;

namespace DotTiled;

/// <summary>
/// Represents a Wang color in a Wang set.
/// </summary>
public class WangColor : HasPropertiesBase
{
  /// <summary>
  /// The name of this color.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The class of the Wang color.
  /// </summary>
  public string Class { get; set; } = "";

  /// <summary>
  /// The color of the Wang color.
  /// </summary>
  public required Color Color { get; set; }

  /// <summary>
  /// The tile ID of the tile representing this color.
  /// </summary>
  public required int Tile { get; set; }

  /// <summary>
  /// The relative probability that this color is chosen over others in case of multiple options.
  /// </summary>
  public float Probability { get; set; } = 0f;

  /// <summary>
  /// The Wang color properties.
  /// </summary>
  public List<IProperty> Properties { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Properties;
}
