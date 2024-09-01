namespace DotTiled;

/// <summary>
/// Represents an image layer in a map.
/// </summary>
public class ImageLayer : BaseLayer
{
  /// <summary>
  /// The X position of the image layer in pixels.
  /// </summary>
  public uint X { get; set; } = 0;

  /// <summary>
  /// The Y position of the image layer in pixels.
  /// </summary>
  public Optional<uint> Y { get; set; } = 0;

  /// <summary>
  /// Whether the image drawn by this layer is repeated along the X axis.
  /// </summary>
  public Optional<bool> RepeatX { get; set; } = false;

  /// <summary>
  /// Whether the image drawn by this layer is repeated along the Y axis.
  /// </summary>
  public Optional<bool> RepeatY { get; set; } = false;

  /// <summary>
  /// The image to be drawn by this image layer.
  /// </summary>
  public Optional<Image> Image { get; set; }
}
