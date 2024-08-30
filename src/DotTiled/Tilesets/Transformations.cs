namespace DotTiled;

/// <summary>
/// Represents which transformations can be applied to a tile in a tileset.
/// </summary>
public class Transformations
{
  /// <summary>
  /// Whether the file in this can set be flipped horizontally.
  /// </summary>
  public bool HFlip { get; set; } = false;

  /// <summary>
  /// Whether the file in this can set be flipped vertically.
  /// </summary>
  public bool VFlip { get; set; } = false;

  /// <summary>
  /// Whether the file in this set can be rotated in 90 degree increments.
  /// </summary>
  public bool Rotate { get; set; } = false;

  /// <summary>
  /// Whether untransformed tiles remain preferred, otherwise transformed tiles are used to produce more vartiations.
  /// </summary>
  public bool PreferUntransformed { get; set; } = false;
}
