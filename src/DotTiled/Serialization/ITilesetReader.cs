using System;
using DotTiled.Model;

namespace DotTiled.Serialization;

public interface ITilesetReader : IDisposable
{
  Tileset ReadTileset();
}
