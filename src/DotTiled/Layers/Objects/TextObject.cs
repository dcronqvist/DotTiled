using System.Globalization;

namespace DotTiled;

/// <summary>
/// The horizontal alignment of text.
/// </summary>
public enum TextHorizontalAlignment
{
  /// <summary>
  /// The text is aligned to the left.
  /// </summary>
  Left,

  /// <summary>
  /// The text is aligned to the center.
  /// </summary>
  Center,

  /// <summary>
  /// The text is aligned to the right.
  /// </summary>
  Right,

  /// <summary>
  /// The text is justified.
  /// </summary>
  Justify
}

/// <summary>
/// The vertical alignment of text.
/// </summary>
public enum TextVerticalAlignment
{
  /// <summary>
  /// The text is aligned to the top.
  /// </summary>
  Top,

  /// <summary>
  /// The text is aligned to the center.
  /// </summary>
  Center,

  /// <summary>
  /// The text is aligned to the bottom.
  /// </summary>
  Bottom
}

/// <summary>
/// A text object in a map.
/// </summary>
public class TextObject : Object
{
  /// <summary>
  /// The font family used for the text.
  /// </summary>
  public string FontFamily { get; set; } = "sans-serif";

  /// <summary>
  /// The size of the font in pixels.
  /// </summary>
  public int PixelSize { get; set; } = 16;

  /// <summary>
  /// Whether word wrapping is enabled.
  /// </summary>
  public bool Wrap { get; set; } = false;

  /// <summary>
  /// The color of the text.
  /// </summary>
  public TiledColor Color { get; set; } = TiledColor.Parse("#000000", CultureInfo.InvariantCulture);

  /// <summary>
  /// Whether the text is bold.
  /// </summary>
  public bool Bold { get; set; } = false;

  /// <summary>
  /// Whether the text is italic.
  /// </summary>
  public bool Italic { get; set; } = false;

  /// <summary>
  /// Whether a line should be drawn below the text.
  /// </summary>
  public bool Underline { get; set; } = false;

  /// <summary>
  /// Whether a line should be drawn through the text.
  /// </summary>
  public bool Strikeout { get; set; } = false;

  /// <summary>
  /// Whether kerning should be used while rendering the text.
  /// </summary>
  public bool Kerning { get; set; } = true;

  /// <summary>
  /// The horizontal alignment of the text.
  /// </summary>
  public TextHorizontalAlignment HorizontalAlignment { get; set; } = TextHorizontalAlignment.Left;

  /// <summary>
  /// The vertical alignment of the text.
  /// </summary>
  public TextVerticalAlignment VerticalAlignment { get; set; } = TextVerticalAlignment.Top;

  /// <summary>
  /// The text to be displayed.
  /// </summary>
  public string Text { get; set; } = "";
}
