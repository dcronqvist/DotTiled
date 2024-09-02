namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertLayer(BaseLayer expected, BaseLayer actual)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    AssertEqual(expected.ID, actual.ID, nameof(BaseLayer.ID));
    AssertEqual(expected.Name, actual.Name, nameof(BaseLayer.Name));
    AssertEqual(expected.Class, actual.Class, nameof(BaseLayer.Class));
    AssertEqual(expected.Opacity, actual.Opacity, nameof(BaseLayer.Opacity));
    AssertEqual(expected.Visible, actual.Visible, nameof(BaseLayer.Visible));
    AssertEqual(expected.TintColor, actual.TintColor, nameof(BaseLayer.TintColor));
    AssertEqual(expected.OffsetX, actual.OffsetX, nameof(BaseLayer.OffsetX));
    AssertEqual(expected.OffsetY, actual.OffsetY, nameof(BaseLayer.OffsetY));
    AssertEqual(expected.ParallaxX, actual.ParallaxX, nameof(BaseLayer.ParallaxX));
    AssertEqual(expected.ParallaxY, actual.ParallaxY, nameof(BaseLayer.ParallaxY));

    AssertProperties(expected.Properties, actual.Properties);
    AssertLayer((dynamic)expected, (dynamic)actual);
  }

  private static void AssertLayer(TileLayer expected, TileLayer actual)
  {
    // Attributes
    AssertEqual(expected.Width, actual.Width, nameof(TileLayer.Width));
    AssertEqual(expected.Height, actual.Height, nameof(TileLayer.Height));
    AssertEqual(expected.X, actual.X, nameof(TileLayer.X));
    AssertEqual(expected.Y, actual.Y, nameof(TileLayer.Y));

    Assert.NotNull(actual.Data);
    AssertData(expected.Data, actual.Data);
  }

  private static void AssertLayer(ObjectLayer expected, ObjectLayer actual)
  {
    // Attributes
    AssertEqual(expected.DrawOrder, actual.DrawOrder, nameof(ObjectLayer.DrawOrder));
    AssertEqual(expected.X, actual.X, nameof(ObjectLayer.X));
    AssertEqual(expected.Y, actual.Y, nameof(ObjectLayer.Y));

    Assert.NotNull(actual.Objects);
    AssertEqual(expected.Objects.Count, actual.Objects.Count, "Objects.Count");
    for (var i = 0; i < expected.Objects.Count; i++)
      AssertObject(expected.Objects[i], actual.Objects[i]);
  }

  private static void AssertLayer(ImageLayer expected, ImageLayer actual)
  {
    // Attributes
    AssertEqual(expected.RepeatX, actual.RepeatX, nameof(ImageLayer.RepeatX));
    AssertEqual(expected.RepeatY, actual.RepeatY, nameof(ImageLayer.RepeatY));
    AssertEqual(expected.X, actual.X, nameof(ImageLayer.X));
    AssertEqual(expected.Y, actual.Y, nameof(ImageLayer.Y));

    Assert.NotNull(actual.Image);
    AssertImage(expected.Image, actual.Image);
  }

  private static void AssertLayer(Group expected, Group actual)
  {
    // Attributes
    Assert.NotNull(actual.Layers);
    AssertEqual(expected.Layers.Count, actual.Layers.Count, "Layers.Count");
    for (var i = 0; i < expected.Layers.Count; i++)
      AssertLayer(expected.Layers[i], actual.Layers[i]);
  }
}
