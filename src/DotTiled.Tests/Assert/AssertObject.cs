using DotTiled.Model;

namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertObject(Model.Layers.Objects.Object expected, Model.Layers.Objects.Object actual)
  {
    // Attributes
    AssertEqual(expected.ID, actual.ID, nameof(Model.Layers.Objects.Object.ID));
    AssertEqual(expected.Name, actual.Name, nameof(Model.Layers.Objects.Object.Name));
    AssertEqual(expected.Type, actual.Type, nameof(Model.Layers.Objects.Object.Type));
    AssertEqual(expected.X, actual.X, nameof(Model.Layers.Objects.Object.X));
    AssertEqual(expected.Y, actual.Y, nameof(Model.Layers.Objects.Object.Y));
    AssertEqual(expected.Width, actual.Width, nameof(Model.Layers.Objects.Object.Width));
    AssertEqual(expected.Height, actual.Height, nameof(Model.Layers.Objects.Object.Height));
    AssertEqual(expected.Rotation, actual.Rotation, nameof(Model.Layers.Objects.Object.Rotation));
    AssertEqual(expected.Visible, actual.Visible, nameof(Model.Layers.Objects.Object.Visible));
    AssertEqual(expected.Template, actual.Template, nameof(Model.Layers.Objects.Object.Template));

    AssertProperties(expected.Properties, actual.Properties);

    Assert.True(expected.GetType() == actual.GetType(), $"Expected object type {expected.GetType()} but got {actual.GetType()}");
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
    AssertEqual(expected.Points, actual.Points, nameof(PolygonObject.Points));
  }

  private static void AssertObject(PolylineObject expected, PolylineObject actual)
  {
    AssertEqual(expected.Points, actual.Points, nameof(PolylineObject.Points));
  }

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

  private static void AssertObject(TileObject expected, TileObject actual)
  {
    // Attributes
    AssertEqual(expected.GID, actual.GID, nameof(TileObject.GID));
  }
}
