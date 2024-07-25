using System.Globalization;

namespace DotTiled;


public enum TextHorizontalAlignment
{
  Left,
  Center,
  Right,
  Justify
}

public enum TextVerticalAlignment
{
  Top,
  Center,
  Bottom
}

public class TextObject : Object
{
  // Attributes
  public string FontFamily { get; set; } = "sans-serif";
  public int PixelSize { get; set; } = 16;
  public bool Wrap { get; set; } = false;
  public Color Color { get; set; } = Color.Parse("#000000", CultureInfo.InvariantCulture);
  public bool Bold { get; set; } = false;
  public bool Italic { get; set; } = false;
  public bool Underline { get; set; } = false;
  public bool Strikeout { get; set; } = false;
  public bool Kerning { get; set; } = true;
  public TextHorizontalAlignment HorizontalAlignment { get; set; } = TextHorizontalAlignment.Left;
  public TextVerticalAlignment VerticalAlignment { get; set; } = TextVerticalAlignment.Top;

  // Elements
  public string Text { get; set; } = "";
}
