namespace DotTiled;

/// <summary>
/// The format of an image.
/// </summary>
public enum ImageFormat
{
  /// <summary>
  /// Portable Network Graphics.
  /// </summary>
  Png,

  /// <summary>
  /// Graphics Interchange Format.
  /// </summary>
  Gif,

  /// <summary>
  /// Joint Photographic Experts Group.
  /// </summary>
  Jpg,

  /// <summary>
  /// Windows Bitmap.
  /// </summary>
  Bmp
}

/// <summary>
/// Represents an image that is used by a tileset.
/// </summary>
public class Image
{
  /// <summary>
  /// The format of the image.
  /// </summary>
  public Optional<ImageFormat> Format { get; set; } = Optional<ImageFormat>.Empty;

  /// <summary>
  /// The reference to the image file.
  /// </summary>
  public Optional<string> Source { get; set; } = Optional<string>.Empty;

  /// <summary>
  /// Defines a specific color that is treated as transparent.
  /// </summary>
  public Optional<TiledColor> TransparentColor { get; set; } = Optional<TiledColor>.Empty;

  /// <summary>
  /// The image width in pixels, used for tile index correction when the image changes.
  /// </summary>
  public Optional<int> Width { get; set; } = Optional<int>.Empty;

  /// <summary>
  /// The image height in pixels, used for tile index correction when the image changes.
  /// </summary>
  public Optional<int> Height { get; set; } = Optional<int>.Empty;
}
