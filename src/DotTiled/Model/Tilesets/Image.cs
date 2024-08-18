namespace DotTiled.Model;

public enum ImageFormat
{
  Png,
  Gif,
  Jpg,
  Bmp
}

public class Image
{
  // Attributes
  public ImageFormat? Format { get; set; }
  public string? Source { get; set; }
  public Color? TransparentColor { get; set; }
  public uint? Width { get; set; }
  public uint? Height { get; set; }
}
