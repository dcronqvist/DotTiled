namespace DotTiled.Tests;

public partial class TmxSerializerImageTests
{
  public static void AssertImage(Image? actual, Image? expected)
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
