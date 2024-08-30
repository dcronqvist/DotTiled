using System.Collections.Generic;

namespace DotTiled;

/// <summary>
/// Defines a list of colors and any number of Wang tiles using these colors.
/// </summary>
public class Wangset : HasPropertiesBase
{
  /// <summary>
  /// The name of the Wang set.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  /// The class of the Wang set.
  /// </summary>
  public string Class { get; set; } = "";

  /// <summary>
  /// The tile ID of the tile representing the Wang set.
  /// </summary>
  public required int Tile { get; set; }

  /// <summary>
  /// The Wang set properties.
  /// </summary>
  public List<IProperty> Properties { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Properties;

  // Up to 254 Wang colors
  /// <summary>
  /// The Wang colors in the Wang set.
  /// </summary>
  public List<WangColor>? WangColors { get; set; } = [];

  /// <summary>
  /// The Wang tiles in the Wang set.
  /// </summary>
  public List<WangTile> WangTiles { get; set; } = [];
}
