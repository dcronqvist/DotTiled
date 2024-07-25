namespace DotTiled.Tests;

public partial class TmxSerializerTilesetTests
{
  public static void AssertTileset(Tileset actual, Tileset expected)
  {
    // Attributes
    Assert.Equal(expected.Version, actual.Version);
    Assert.Equal(expected.TiledVersion, actual.TiledVersion);
    Assert.Equal(expected.FirstGID, actual.FirstGID);
    Assert.Equal(expected.Source, actual.Source);
    Assert.Equal(expected.Name, actual.Name);
    Assert.Equal(expected.Class, actual.Class);
    Assert.Equal(expected.TileWidth, actual.TileWidth);
    Assert.Equal(expected.TileHeight, actual.TileHeight);
    Assert.Equal(expected.Spacing, actual.Spacing);
    Assert.Equal(expected.Margin, actual.Margin);
    Assert.Equal(expected.TileCount, actual.TileCount);
    Assert.Equal(expected.Columns, actual.Columns);
    Assert.Equal(expected.ObjectAlignment, actual.ObjectAlignment);
    Assert.Equal(expected.RenderSize, actual.RenderSize);
    Assert.Equal(expected.FillMode, actual.FillMode);

    // At most one of
    TmxSerializerImageTests.AssertImage(actual.Image, expected.Image);
    AssertTileOffset(actual.TileOffset, expected.TileOffset);
    AssertGrid(actual.Grid, expected.Grid);
    TmxSerializerPropertiesTests.AssertProperties(actual.Properties, expected.Properties);
    // TODO: AssertTerrainTypes(actual.TerrainTypes, expected.TerrainTypes);
    if (expected.Wangsets is not null)
    {
      Assert.NotNull(actual.Wangsets);
      Assert.Equal(expected.Wangsets.Count, actual.Wangsets.Count);
      for (var i = 0; i < expected.Wangsets.Count; i++)
        AssertWangset(actual.Wangsets[i], expected.Wangsets[i]);
    }
    AssertTransformations(actual.Transformations, expected.Transformations);

    // Any number of
    Assert.NotNull(actual.Tiles);
    Assert.Equal(expected.Tiles.Count, actual.Tiles.Count);
    for (var i = 0; i < expected.Tiles.Count; i++)
      AssertTile(actual.Tiles[i], expected.Tiles[i]);
  }

  private static void AssertTileOffset(TileOffset? actual, TileOffset? expected)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);
  }

  private static void AssertGrid(Grid? actual, Grid? expected)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    Assert.Equal(expected.Orientation, actual.Orientation);
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);
  }

  private static void AssertWangset(Wangset actual, Wangset expected)
  {
    // Attributes
    Assert.Equal(expected.Name, actual.Name);
    Assert.Equal(expected.Class, actual.Class);
    Assert.Equal(expected.Tile, actual.Tile);

    // At most one of
    TmxSerializerPropertiesTests.AssertProperties(actual.Properties, expected.Properties);
    if (expected.WangColors is not null)
    {
      Assert.NotNull(actual.WangColors);
      Assert.Equal(expected.WangColors.Count, actual.WangColors.Count);
      for (var i = 0; i < expected.WangColors.Count; i++)
        AssertWangColor(actual.WangColors[i], expected.WangColors[i]);
    }
    for (var i = 0; i < expected.WangTiles.Count; i++)
      AssertWangTile(actual.WangTiles[i], expected.WangTiles[i]);
  }

  private static void AssertWangColor(WangColor actual, WangColor expected)
  {
    // Attributes
    Assert.Equal(expected.Name, actual.Name);
    Assert.Equal(expected.Class, actual.Class);
    Assert.Equal(expected.Color, actual.Color);
    Assert.Equal(expected.Tile, actual.Tile);
    Assert.Equal(expected.Probability, actual.Probability);

    TmxSerializerPropertiesTests.AssertProperties(actual.Properties, expected.Properties);
  }

  private static void AssertWangTile(WangTile actual, WangTile expected)
  {
    // Attributes
    Assert.Equal(expected.TileID, actual.TileID);
    Assert.Equal(expected.WangID, actual.WangID);
  }

  private static void AssertTransformations(Transformations? actual, Transformations? expected)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    Assert.Equal(expected.HFlip, actual.HFlip);
    Assert.Equal(expected.VFlip, actual.VFlip);
    Assert.Equal(expected.Rotate, actual.Rotate);
    Assert.Equal(expected.PreferUntransformed, actual.PreferUntransformed);
  }

  private static void AssertTile(Tile actual, Tile expected)
  {
    // Attributes
    Assert.Equal(expected.ID, actual.ID);
    Assert.Equal(expected.Type, actual.Type);
    Assert.Equal(expected.Probability, actual.Probability);
    Assert.Equal(expected.X, actual.X);
    Assert.Equal(expected.Y, actual.Y);
    Assert.Equal(expected.Width, actual.Width);
    Assert.Equal(expected.Height, actual.Height);

    // Elements
    TmxSerializerPropertiesTests.AssertProperties(actual.Properties, expected.Properties);
    TmxSerializerImageTests.AssertImage(actual.Image, expected.Image);
    TmxSerializerLayerTests.AssertLayer(actual.ObjectLayer, expected.ObjectLayer);
    if (expected.Animation is not null)
    {
      Assert.NotNull(actual.Animation);
      Assert.Equal(expected.Animation.Count, actual.Animation.Count);
      for (var i = 0; i < expected.Animation.Count; i++)
        AssertFrame(actual.Animation[i], expected.Animation[i]);
    }
  }

  private static void AssertFrame(Frame actual, Frame expected)
  {
    // Attributes
    Assert.Equal(expected.TileID, actual.TileID);
    Assert.Equal(expected.Duration, actual.Duration);
  }
}
