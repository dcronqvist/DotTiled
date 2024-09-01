namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertTileset(Tileset expected, Tileset actual)
  {
    AssertEqual(expected.Version, actual.Version, nameof(Tileset.Version));
    AssertEqual(expected.TiledVersion, actual.TiledVersion, nameof(Tileset.TiledVersion));
    AssertEqual(expected.FirstGID, actual.FirstGID, nameof(Tileset.FirstGID));
    AssertEqual(expected.Source, actual.Source, nameof(Tileset.Source));
    AssertEqual(expected.Name, actual.Name, nameof(Tileset.Name));
    AssertEqual(expected.Class, actual.Class, nameof(Tileset.Class));
    AssertEqual(expected.TileWidth, actual.TileWidth, nameof(Tileset.TileWidth));
    AssertEqual(expected.TileHeight, actual.TileHeight, nameof(Tileset.TileHeight));
    AssertEqual(expected.Spacing, actual.Spacing, nameof(Tileset.Spacing));
    AssertEqual(expected.Margin, actual.Margin, nameof(Tileset.Margin));
    AssertEqual(expected.TileCount, actual.TileCount, nameof(Tileset.TileCount));
    AssertEqual(expected.Columns, actual.Columns, nameof(Tileset.Columns));
    AssertEqual(expected.ObjectAlignment, actual.ObjectAlignment, nameof(Tileset.ObjectAlignment));
    AssertEqual(expected.RenderSize, actual.RenderSize, nameof(Tileset.RenderSize));
    AssertEqual(expected.FillMode, actual.FillMode, nameof(Tileset.FillMode));

    AssertOptionalsEqual(expected.Image, actual.Image, nameof(Tileset.Image), AssertImage);
    AssertOptionalsEqual(expected.TileOffset, actual.TileOffset, nameof(Tileset.TileOffset), AssertTileOffset);
    AssertOptionalsEqual(expected.Grid, actual.Grid, nameof(Tileset.Grid), AssertGrid);
    AssertProperties(expected.Properties, actual.Properties);
    AssertListOrdered(expected.Wangsets, actual.Wangsets, nameof(Tileset.Wangsets), AssertWangset);
    AssertOptionalsEqual(expected.Transformations, actual.Transformations, nameof(Tileset.Transformations), AssertTransformations);
    AssertListOrdered(expected.Tiles, actual.Tiles, nameof(Tileset.Tiles), AssertTile);
  }

  private static void AssertTileOffset(TileOffset expected, TileOffset actual)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    AssertEqual(expected.X, actual.X, nameof(TileOffset.X));
    AssertEqual(expected.Y, actual.Y, nameof(TileOffset.Y));
  }

  private static void AssertGrid(Grid? expected, Grid? actual)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    // Attributes
    Assert.NotNull(actual);
    AssertEqual(expected.Orientation, actual.Orientation, nameof(Grid.Orientation));
    AssertEqual(expected.Width, actual.Width, nameof(Grid.Width));
    AssertEqual(expected.Height, actual.Height, nameof(Grid.Height));
  }

  private static void AssertWangset(Wangset expected, Wangset actual)
  {
    AssertEqual(expected.Name, actual.Name, nameof(Wangset.Name));
    AssertEqual(expected.Class, actual.Class, nameof(Wangset.Class));
    AssertEqual(expected.Tile, actual.Tile, nameof(Wangset.Tile));

    AssertProperties(expected.Properties, actual.Properties);
    AssertListOrdered(expected.WangColors, actual.WangColors, nameof(Wangset.WangColors), AssertWangColor);
    AssertListOrdered(expected.WangTiles, actual.WangTiles, nameof(Wangset.WangTiles), AssertWangTile);
  }

  private static void AssertWangColor(WangColor expected, WangColor actual)
  {
    AssertEqual(expected.Name, actual.Name, nameof(WangColor.Name));
    AssertEqual(expected.Class, actual.Class, nameof(WangColor.Class));
    AssertEqual(expected.Color, actual.Color, nameof(WangColor.Color));
    AssertEqual(expected.Tile, actual.Tile, nameof(WangColor.Tile));
    AssertEqual(expected.Probability, actual.Probability, nameof(WangColor.Probability));

    AssertProperties(expected.Properties, actual.Properties);
  }

  private static void AssertWangTile(WangTile expected, WangTile actual)
  {
    AssertEqual(expected.TileID, actual.TileID, nameof(WangTile.TileID));
    AssertEqual(expected.WangID, actual.WangID, nameof(WangTile.WangID));
  }

  private static void AssertTransformations(Transformations? expected, Transformations? actual)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    Assert.NotNull(actual);
    AssertEqual(expected.HFlip, actual.HFlip, nameof(Transformations.HFlip));
    AssertEqual(expected.VFlip, actual.VFlip, nameof(Transformations.VFlip));
    AssertEqual(expected.Rotate, actual.Rotate, nameof(Transformations.Rotate));
    AssertEqual(expected.PreferUntransformed, actual.PreferUntransformed, nameof(Transformations.PreferUntransformed));
  }

  private static void AssertTile(Tile expected, Tile actual)
  {
    AssertEqual(expected.ID, actual.ID, nameof(Tile.ID));
    AssertEqual(expected.Type, actual.Type, nameof(Tile.Type));
    AssertEqual(expected.Probability, actual.Probability, nameof(Tile.Probability));
    AssertEqual(expected.X, actual.X, nameof(Tile.X));
    AssertEqual(expected.Y, actual.Y, nameof(Tile.Y));
    AssertEqual(expected.Width, actual.Width, nameof(Tile.Width));
    AssertEqual(expected.Height, actual.Height, nameof(Tile.Height));

    AssertProperties(expected.Properties, actual.Properties);
    AssertOptionalsEqual(expected.Image, actual.Image, nameof(Tile.Image), AssertImage);
    AssertOptionalsEqual(expected.ObjectLayer, actual.ObjectLayer, nameof(Tile.ObjectLayer), (a, b) => AssertLayer((BaseLayer)a, (BaseLayer)b));
    AssertListOrdered(expected.Animation, actual.Animation, nameof(Tile.Animation), AssertFrame);
  }

  private static void AssertFrame(Frame expected, Frame actual)
  {
    AssertEqual(expected.TileID, actual.TileID, nameof(Frame.TileID));
    AssertEqual(expected.Duration, actual.Duration, nameof(Frame.Duration));
  }
}
