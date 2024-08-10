namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertObject(Object expected, Object actual)
  {
    // Attributes
    Assert.Equal(expected.ID, actual.ID);
    Assert.Equal(expected.Name, actual.Name);
    Assert.Equal(expected.Type, actual.Type);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);
    Assert.Equal(expected.Rotation, actual.Rotation);
    Assert.Equal(expected.GID, actual.GID);
    Assert.Equal(expected.Visible, actual.Visible);
    Assert.Equal(expected.Template, actual.Template);

    AssertProperties(expected.Properties, actual.Properties);
    AssertObject((dynamic)expected, (dynamic)actual);
  }

  private static void AssertObject(RectangleObject expected, RectangleObject actual)
  {
    Assert.True(true); // A rectangle object is the same as the abstract Object
  }

  private static void AssertObject(EllipseObject expected, EllipseObject actual)
  {
    Assert.True(true); // An ellipse object is the same as the abstract Object
  }

  private static void AssertObject(PointObject expected, PointObject actual)
  {
    Assert.True(true); // A point object is the same as the abstract Object
  }

  private static void AssertObject(PolygonObject expected, PolygonObject actual)
  {
    Assert.Equal(expected.Points, actual.Points);
  }

  private static void AssertObject(PolylineObject expected, PolylineObject actual)
  {
    Assert.Equal(expected.Points, actual.Points);
  }

  private static void AssertObject(TextObject expected, TextObject actual)
  {
    // Attributes
    Assert.Equal(expected.FontFamily, actual.FontFamily);
    Assert.Equal(expected.PixelSize, actual.PixelSize);
    Assert.Equal(expected.Wrap, actual.Wrap);
    Assert.Equal(expected.Color, actual.Color);
    Assert.Equal(expected.Bold, actual.Bold);
    Assert.Equal(expected.Italic, actual.Italic);
    Assert.Equal(expected.Underline, actual.Underline);
    Assert.Equal(expected.Strikeout, actual.Strikeout);
    Assert.Equal(expected.Kerning, actual.Kerning);
    Assert.Equal(expected.HorizontalAlignment, actual.HorizontalAlignment);
    Assert.Equal(expected.VerticalAlignment, actual.VerticalAlignment);

    Assert.Equal(expected.Text, actual.Text);
  }
}
