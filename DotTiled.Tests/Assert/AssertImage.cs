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
    Assert.Equal(expected.Format, actual.Format);
    Assert.Equal(expected.Source, actual.Source);
    Assert.Equal(expected.TransparentColor, actual.TransparentColor);
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);
  }
}
