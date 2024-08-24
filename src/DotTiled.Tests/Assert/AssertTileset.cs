using DotTiled.Model;

namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertTileset(Tileset expected, Tileset actual)
  {
    // Attributes
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

    // At most one of
    AssertImage(expected.Image, actual.Image);
    AssertTileOffset(expected.TileOffset, actual.TileOffset);
    AssertGrid(expected.Grid, actual.Grid);
    AssertProperties(expected.Properties, actual.Properties);
    // TODO: AssertTerrainTypes(actual.TerrainTypes, expected.TerrainTypes);
    if (expected.Wangsets is not null)
    {
      Assert.NotNull(actual.Wangsets);
      AssertEqual(expected.Wangsets.Count, actual.Wangsets.Count, "Wangsets.Count");
      for (var i = 0; i < expected.Wangsets.Count; i++)
        AssertWangset(expected.Wangsets[i], actual.Wangsets[i]);
    }
    AssertTransformations(expected.Transformations, actual.Transformations);

    // Any number of
    Assert.NotNull(actual.Tiles);
    AssertEqual(expected.Tiles.Count, actual.Tiles.Count, "Tiles.Count");
    for (var i = 0; i < expected.Tiles.Count; i++)
      AssertTile(expected.Tiles[i], actual.Tiles[i]);
  }

  private static void AssertTileOffset(TileOffset? expected, TileOffset? actual)
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
    // Attributes
    AssertEqual(expected.Name, actual.Name, nameof(Wangset.Name));
    AssertEqual(expected.Class, actual.Class, nameof(Wangset.Class));
    AssertEqual(expected.Tile, actual.Tile, nameof(Wangset.Tile));

    // At most one of
    AssertProperties(expected.Properties, actual.Properties);
    if (expected.WangColors is not null)
    {
      Assert.NotNull(actual.WangColors);
      AssertEqual(expected.WangColors.Count, actual.WangColors.Count, "WangColors.Count");
      for (var i = 0; i < expected.WangColors.Count; i++)
        AssertWangColor(expected.WangColors[i], actual.WangColors[i]);
    }
    for (var i = 0; i < expected.WangTiles.Count; i++)
      AssertWangTile(expected.WangTiles[i], actual.WangTiles[i]);
  }

  private static void AssertWangColor(WangColor expected, WangColor actual)
  {
    // Attributes
    AssertEqual(expected.Name, actual.Name, nameof(WangColor.Name));
    AssertEqual(expected.Class, actual.Class, nameof(WangColor.Class));
    AssertEqual(expected.Color, actual.Color, nameof(WangColor.Color));
    AssertEqual(expected.Tile, actual.Tile, nameof(WangColor.Tile));
    AssertEqual(expected.Probability, actual.Probability, nameof(WangColor.Probability));

    AssertProperties(expected.Properties, actual.Properties);
  }

  private static void AssertWangTile(WangTile expected, WangTile actual)
  {
    // Attributes
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

    // Attributes
    Assert.NotNull(actual);
    AssertEqual(expected.HFlip, actual.HFlip, nameof(Transformations.HFlip));
    AssertEqual(expected.VFlip, actual.VFlip, nameof(Transformations.VFlip));
    AssertEqual(expected.Rotate, actual.Rotate, nameof(Transformations.Rotate));
    AssertEqual(expected.PreferUntransformed, actual.PreferUntransformed, nameof(Transformations.PreferUntransformed));
  }

  private static void AssertTile(Tile expected, Tile actual)
  {
    // Attributes
    AssertEqual(expected.ID, actual.ID, nameof(Tile.ID));
    AssertEqual(expected.Type, actual.Type, nameof(Tile.Type));
    AssertEqual(expected.Probability, actual.Probability, nameof(Tile.Probability));
    AssertEqual(expected.X, actual.X, nameof(Tile.X));
    AssertEqual(expected.Y, actual.Y, nameof(Tile.Y));
    AssertEqual(expected.Width, actual.Width, nameof(Tile.Width));
    AssertEqual(expected.Height, actual.Height, nameof(Tile.Height));

    // Elements
    AssertProperties(actual.Properties, expected.Properties);
    AssertImage(actual.Image, expected.Image);
    AssertLayer((BaseLayer?)actual.ObjectLayer, (BaseLayer?)expected.ObjectLayer);
    if (expected.Animation is not null)
    {
      Assert.NotNull(actual.Animation);
      AssertEqual(expected.Animation.Count, actual.Animation.Count, "Animation.Count");
      for (var i = 0; i < expected.Animation.Count; i++)
        AssertFrame(expected.Animation[i], actual.Animation[i]);
    }
  }

  private static void AssertFrame(Frame expected, Frame actual)
  {
    // Attributes
    AssertEqual(expected.TileID, actual.TileID, nameof(Frame.TileID));
    AssertEqual(expected.Duration, actual.Duration, nameof(Frame.Duration));
  }
}
