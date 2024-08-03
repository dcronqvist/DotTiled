namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertMap(Map expected, Map actual)
  {
    // Attributes
    Assert.Equal(expected.Version, actual.Version);
    Assert.Equal(expected.TiledVersion, actual.TiledVersion);
    Assert.Equal(expected.Class, actual.Class);
    Assert.Equal(expected.Orientation, actual.Orientation);
    Assert.Equal(expected.RenderOrder, actual.RenderOrder);
    Assert.Equal(expected.CompressionLevel, actual.CompressionLevel);
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);
    Assert.Equal(expected.TileWidth, actual.TileWidth);
    Assert.Equal(expected.TileHeight, actual.TileHeight);
    Assert.Equal(expected.HexSideLength, actual.HexSideLength);
    Assert.Equal(expected.StaggerAxis, actual.StaggerAxis);
    Assert.Equal(expected.StaggerIndex, actual.StaggerIndex);
    Assert.Equal(expected.ParallaxOriginX, actual.ParallaxOriginX);
    Assert.Equal(expected.ParallaxOriginY, actual.ParallaxOriginY);
    Assert.Equal(expected.BackgroundColor, actual.BackgroundColor);
    Assert.Equal(expected.NextLayerID, actual.NextLayerID);
    Assert.Equal(expected.NextObjectID, actual.NextObjectID);
    Assert.Equal(expected.Infinite, actual.Infinite);

    AssertProperties(actual.Properties, expected.Properties);

    Assert.NotNull(actual.Tilesets);
    Assert.Equal(expected.Tilesets.Count, actual.Tilesets.Count);
    for (var i = 0; i < expected.Tilesets.Count; i++)
      AssertTileset(actual.Tilesets[i], expected.Tilesets[i]);

    Assert.NotNull(actual.Layers);
    Assert.Equal(expected.Layers.Count, actual.Layers.Count);
    for (var i = 0; i < expected.Layers.Count; i++)
      AssertLayer(actual.Layers[i], expected.Layers[i]);
  }
}
