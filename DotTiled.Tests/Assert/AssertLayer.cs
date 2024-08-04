namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertLayer(BaseLayer? expected, BaseLayer? actual)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    Assert.Equal(expected.ID, actual.ID);
    Assert.Equal(expected.Name, actual.Name);
    Assert.Equal(expected.Class, actual.Class);
    Assert.Equal(expected.Opacity, actual.Opacity);
    Assert.Equal(expected.Visible, actual.Visible);
    Assert.Equal(expected.TintColor, actual.TintColor);
    Assert.Equal(expected.OffsetX, actual.OffsetX);
    Assert.Equal(expected.OffsetY, actual.OffsetY);
    Assert.Equal(expected.ParallaxX, actual.ParallaxX);
    Assert.Equal(expected.ParallaxY, actual.ParallaxY);

    AssertProperties(expected.Properties, actual.Properties);
    AssertLayer((dynamic)expected, (dynamic)actual);
  }

  private static void AssertLayer(TileLayer expected, TileLayer actual)
  {
    // Attributes
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);

    Assert.NotNull(actual.Data);
    AssertData(expected.Data, actual.Data);
  }

  private static void AssertLayer(ObjectLayer expected, ObjectLayer actual)
  {
    // Attributes
    Assert.Equal(expected.DrawOrder, actual.DrawOrder);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);

    Assert.NotNull(actual.Objects);
    Assert.Equal(expected.Objects.Count, actual.Objects.Count);
    for (var i = 0; i < expected.Objects.Count; i++)
      AssertObject(expected.Objects[i], actual.Objects[i]);
  }

  private static void AssertLayer(ImageLayer expected, ImageLayer actual)
  {
    // Attributes
    Assert.Equal(expected.RepeatX, actual.RepeatX);
    Assert.Equal(expected.RepeatY, actual.RepeatY);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);

    Assert.NotNull(actual.Image);
    AssertImage(expected.Image, actual.Image);
  }

  private static void AssertLayer(Group expected, Group actual)
  {
    // Attributes
    Assert.NotNull(actual.Layers);
    Assert.Equal(expected.Layers.Count, actual.Layers.Count);
    for (var i = 0; i < expected.Layers.Count; i++)
      AssertLayer(expected.Layers[i], actual.Layers[i]);
  }
}
