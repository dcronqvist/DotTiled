using System;

namespace DotTiled;

public interface ITilesetReader : IDisposable
{
  Tileset ReadTileset();
}
