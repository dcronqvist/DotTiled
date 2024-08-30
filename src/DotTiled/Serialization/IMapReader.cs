using System;

namespace DotTiled.Serialization;

/// <summary>
/// Interface for reading a map from some source. Used by the different file format parsers to read a map.
/// </summary>
public interface IMapReader : IDisposable
{
  /// <summary>
  /// Reads a map from the source.
  /// </summary>
  /// <returns>The parsed map.</returns>
  Map ReadMap();
}
