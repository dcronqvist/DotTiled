namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  private static void AssertEqual<T>(T expected, T actual, string nameof)
  {
    if (expected == null)
    {
      Assert.Null(actual);
      return;
    }

    if (typeof(T).IsArray)
    {
      var expectedArray = (Array)(object)expected;
      var actualArray = (Array)(object)actual!;

      Assert.NotNull(actualArray);
      AssertEqual(expectedArray.Length, actualArray.Length, $"{nameof}.Length");

      for (var i = 0; i < expectedArray.Length; i++)
        AssertEqual(expectedArray.GetValue(i), actualArray.GetValue(i), $"{nameof}[{i}]");

      return;
    }

    Assert.True(expected.Equals(actual), $"Expected {nameof} '{expected}' but got '{actual}'");
  }

  internal static void AssertMap(Map expected, Map actual)
  {
    // Attributes
    AssertEqual(expected.Version, actual.Version, nameof(Map.Version));
    AssertEqual(expected.TiledVersion, actual.TiledVersion, nameof(Map.TiledVersion));
    AssertEqual(expected.Class, actual.Class, nameof(Map.Class));
    AssertEqual(expected.Orientation, actual.Orientation, nameof(Map.Orientation));
    AssertEqual(expected.RenderOrder, actual.RenderOrder, nameof(Map.RenderOrder));
    AssertEqual(expected.CompressionLevel, actual.CompressionLevel, nameof(Map.CompressionLevel));
    AssertEqual(expected.Width, actual.Width, nameof(Map.Width));
    AssertEqual(expected.Height, actual.Height, nameof(Map.Height));
    AssertEqual(expected.TileWidth, actual.TileWidth, nameof(Map.TileWidth));
    AssertEqual(expected.TileHeight, actual.TileHeight, nameof(Map.TileHeight));
    AssertEqual(expected.HexSideLength, actual.HexSideLength, nameof(Map.HexSideLength));
    AssertEqual(expected.StaggerAxis, actual.StaggerAxis, nameof(Map.StaggerAxis));
    AssertEqual(expected.StaggerIndex, actual.StaggerIndex, nameof(Map.StaggerIndex));
    AssertEqual(expected.ParallaxOriginX, actual.ParallaxOriginX, nameof(Map.ParallaxOriginX));
    AssertEqual(expected.ParallaxOriginY, actual.ParallaxOriginY, nameof(Map.ParallaxOriginY));
    AssertEqual(expected.BackgroundColor, actual.BackgroundColor, nameof(Map.BackgroundColor));
    AssertEqual(expected.NextLayerID, actual.NextLayerID, nameof(Map.NextLayerID));
    AssertEqual(expected.NextObjectID, actual.NextObjectID, nameof(Map.NextObjectID));
    AssertEqual(expected.Infinite, actual.Infinite, nameof(Map.Infinite));

    AssertProperties(actual.Properties, expected.Properties);

    Assert.NotNull(actual.Tilesets);
    AssertEqual(expected.Tilesets.Count, actual.Tilesets.Count, "Tilesets.Count");
    for (var i = 0; i < expected.Tilesets.Count; i++)
      AssertTileset(expected.Tilesets[i], actual.Tilesets[i]);

    Assert.NotNull(actual.Layers);
    AssertEqual(expected.Layers.Count, actual.Layers.Count, "Layers.Count");
    for (var i = 0; i < expected.Layers.Count; i++)
      AssertLayer(expected.Layers[i], actual.Layers[i]);
  }
}
