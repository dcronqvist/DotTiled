namespace DotTiled.Model;

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
  public ImageFormat? Format { get; set; }

  /// <summary>
  /// The reference to the image file.
  /// </summary>
  public string? Source { get; set; }

  /// <summary>
  /// Defines a specific color that is treated as transparent.
  /// </summary>
  public Color? TransparentColor { get; set; }

  /// <summary>
  /// The image width in pixels, used for tile index correction when the image changes.
  /// </summary>
  public uint? Width { get; set; }

  /// <summary>
  /// The image height in pixels, used for tile index correction when the image changes.
  /// </summary>
  public uint? Height { get; set; }
}
