# Useful Helpers

This section documents some useful helper methods provided by DotTiled to make working with Tiled maps easier.

## Retrieving Global Tile ID at coordinates

The <xref:DotTiled.TileLayer> class includes a helper method <xref:DotTiled.TileLayer.GetGlobalTileIDAtCoord(System.Int32,System.Int32)> that allows you to retrieve the Global Tile ID at specific coordinates within a tile layer. This method works for both finite and infinite maps, handling chunks appropriately.

## Resolving Tileset for a Global Tile ID

The <xref:DotTiled.Map> class provides a method <xref:DotTiled.Map.ResolveTilesetForGlobalTileID(System.UInt32,System.UInt32@)> that helps you find the corresponding tileset for a given Global Tile ID. This is particularly useful when you need to determine which tileset a specific tile belongs to. This will allow you to determine the corresponding texture that the tile uses.

Note that this method also provides the Local Tile ID within the tileset via an `out` parameter.

## Retrieving Source Rectangle for a local Tile ID

Once you have the tileset and the Local Tile ID, you can use the <xref:DotTiled.Tileset.GetSourceRectangleForLocalTileID(System.UInt32)> method to get the source rectangle of the tile within the tileset image. This is essential for rendering the correct portion of the tileset texture when drawing tiles.
