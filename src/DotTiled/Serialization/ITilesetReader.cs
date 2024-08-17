using System;
using DotTiled.Model.Tilesets;

namespace DotTiled.Serialization;

public interface ITilesetReader : IDisposable
{
  Tileset ReadTileset();
}
