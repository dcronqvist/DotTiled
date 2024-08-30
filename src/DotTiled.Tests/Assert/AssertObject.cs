namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertObject(DotTiled.Object expected, DotTiled.Object actual)
  {
    // Attributes
#pragma warning disable IDE0002
    AssertEqual(expected.ID, actual.ID, nameof(DotTiled.Object.ID));
    AssertEqual(expected.Name, actual.Name, nameof(DotTiled.Object.Name));
    AssertEqual(expected.Type, actual.Type, nameof(DotTiled.Object.Type));
    AssertEqual(expected.X, actual.X, nameof(DotTiled.Object.X));
    AssertEqual(expected.Y, actual.Y, nameof(DotTiled.Object.Y));
    AssertEqual(expected.Width, actual.Width, nameof(DotTiled.Object.Width));
    AssertEqual(expected.Height, actual.Height, nameof(DotTiled.Object.Height));
    AssertEqual(expected.Rotation, actual.Rotation, nameof(DotTiled.Object.Rotation));
    AssertEqual(expected.Visible, actual.Visible, nameof(DotTiled.Object.Visible));
    AssertEqual(expected.Template, actual.Template, nameof(DotTiled.Object.Template));
#pragma warning restore IDE0002

    AssertProperties(expected.Properties, actual.Properties);

    Assert.True(expected.GetType() == actual.GetType(), $"Expected object type {expected.GetType()} but got {actual.GetType()}");
    AssertObject((dynamic)expected, (dynamic)actual);
  }

  private static void AssertObject(RectangleObject _, RectangleObject __) => Assert.True(true); // A rectangle object is the same as the abstract Object

  private static void AssertObject(EllipseObject _, EllipseObject __) => Assert.True(true); // An ellipse object is the same as the abstract Object

  private static void AssertObject(PointObject _, PointObject __) => Assert.True(true); // A point object is the same as the abstract Object

  private static void AssertObject(PolygonObject expected, PolygonObject actual) => AssertEqual(expected.Points, actual.Points, nameof(PolygonObject.Points));

  private static void AssertObject(PolylineObject expected, PolylineObject actual) => AssertEqual(expected.Points, actual.Points, nameof(PolylineObject.Points));

  private static void AssertObject(TextObject expected, TextObject actual)
  {
    // Attributes
    AssertEqual(expected.FontFamily, actual.FontFamily, nameof(TextObject.FontFamily));
    AssertEqual(expected.PixelSize, actual.PixelSize, nameof(TextObject.PixelSize));
    AssertEqual(expected.Wrap, actual.Wrap, nameof(TextObject.Wrap));
    AssertEqual(expected.Color, actual.Color, nameof(TextObject.Color));
    AssertEqual(expected.Bold, actual.Bold, nameof(TextObject.Bold));
    AssertEqual(expected.Italic, actual.Italic, nameof(TextObject.Italic));
    AssertEqual(expected.Underline, actual.Underline, nameof(TextObject.Underline));
    AssertEqual(expected.Strikeout, actual.Strikeout, nameof(TextObject.Strikeout));
    AssertEqual(expected.Kerning, actual.Kerning, nameof(TextObject.Kerning));
    AssertEqual(expected.HorizontalAlignment, actual.HorizontalAlignment, nameof(TextObject.HorizontalAlignment));
    AssertEqual(expected.VerticalAlignment, actual.VerticalAlignment, nameof(TextObject.VerticalAlignment));

    AssertEqual(expected.Text, actual.Text, nameof(TextObject.Text));
  }

  private static void AssertObject(TileObject expected, TileObject actual) => AssertEqual(expected.GID, actual.GID, nameof(TileObject.GID));
}
