using DotTiled.Model;

namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertImage(Image? expected, Image? actual)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    AssertEqual(expected.Format, actual.Format, nameof(Image.Format));
    AssertEqual(expected.Source, actual.Source, nameof(Image.Source));
    AssertEqual(expected.TransparentColor, actual.TransparentColor, nameof(Image.TransparentColor));
    AssertEqual(expected.Width, actual.Width, nameof(Image.Width));
    AssertEqual(expected.Height, actual.Height, nameof(Image.Height));
  }
}
