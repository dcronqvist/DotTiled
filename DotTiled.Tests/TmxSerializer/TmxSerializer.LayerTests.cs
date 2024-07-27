namespace DotTiled.Tests;

public partial class TmxSerializerLayerTests
{
  public static void AssertLayer(BaseLayer? actual, BaseLayer? expected)
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

    TmxSerializerPropertiesTests.AssertProperties(actual.Properties, expected.Properties);
    AssertLayer((dynamic)actual, (dynamic)expected);
  }

  private static void AssertLayer(TileLayer actual, TileLayer expected)
  {
    // Attributes
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);

    Assert.NotNull(actual.Data);
    TmxSerializerDataTests.AssertData(actual.Data, expected.Data);
  }

  private static void AssertLayer(ObjectLayer actual, ObjectLayer expected)
  {
    // Attributes
    Assert.Equal(expected.DrawOrder, actual.DrawOrder);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);

    Assert.NotNull(actual.Objects);
    Assert.Equal(expected.Objects.Count, actual.Objects.Count);
    for (var i = 0; i < expected.Objects.Count; i++)
      TmxSerializerObjectTests.AssertObject(actual.Objects[i], expected.Objects[i]);
  }

  private static void AssertLayer(ImageLayer actual, ImageLayer expected)
  {
    // Attributes
    Assert.Equal(expected.RepeatX, actual.RepeatX);
    Assert.Equal(expected.RepeatY, actual.RepeatY);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);

    Assert.NotNull(actual.Image);
    TmxSerializerImageTests.AssertImage(actual.Image, expected.Image);
  }

  private static void AssertLayer(Group actual, Group expected)
  {
    // Attributes
    Assert.NotNull(actual.Layers);
    Assert.Equal(expected.Layers.Count, actual.Layers.Count);
    for (var i = 0; i < expected.Layers.Count; i++)
      AssertLayer(actual.Layers[i], expected.Layers[i]);
  }
}
