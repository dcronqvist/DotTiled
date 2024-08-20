using System;
using DotTiled.Model;

namespace DotTiled.Serialization;

/// <summary>
/// Interface for reading a tileset from some source. Used by the different file format parsers to read a tileset.
/// </summary>
public interface ITilesetReader : IDisposable
{
  /// <summary>
  /// Reads a tileset from the source.
  /// </summary>
  /// <returns>The parsed tileset.</returns>
  Tileset ReadTileset();
}
