using System;
using System.IO;
using System.Xml;
using DotTiled.Model;
using DotTiled.Serialization.Tmj;
using DotTiled.Serialization.Tmx;

namespace DotTiled.Serialization;

/// <summary>
/// Reads a map from a string, regardless of format.
/// </summary>
public class MapReader : IMapReader
{
  // External resolvers
  private readonly Func<string, Tileset> _externalTilesetResolver;
  private readonly Func<string, Template> _externalTemplateResolver;
  private readonly Func<string, ICustomTypeDefinition> _customTypeResolver;

  private readonly StringReader? _mapStringReader;
  private readonly XmlReader? _xmlReader;
  private readonly IMapReader _mapReader;
  private bool disposedValue;

  /// <summary>
  /// Constructs a new <see cref="MapReader"/>, capable of reading a map from a string, regardless of format.
  /// </summary>
  /// <param name="map">The string containing the map data.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeResolver">A function that resolves custom types given their source.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  public MapReader(
    string map,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver)
  {
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeResolver = customTypeResolver ?? throw new ArgumentNullException(nameof(customTypeResolver));

    // Prepare reader
    if (Helpers.StringIsXml(map))
    {
      _mapStringReader = new StringReader(map);
      _xmlReader = XmlReader.Create(_mapStringReader);
      _mapReader = new TmxMapReader(_xmlReader, _externalTilesetResolver, _externalTemplateResolver, _customTypeResolver);
    }
    else
    {
      _mapReader = new TmjMapReader(map, _externalTilesetResolver, _externalTemplateResolver, _customTypeResolver);
    }
  }

  /// <inheritdoc />
  public Map ReadMap() => _mapReader.ReadMap();

  /// <inheritdoc />
  protected virtual void Dispose(bool disposing)
  {
    if (!disposedValue)
    {
      if (disposing)
      {
        // TODO: dispose managed state (managed objects)
        _mapStringReader?.Dispose();
        _xmlReader?.Dispose();
        _mapReader.Dispose();
      }

      // TODO: free unmanaged resources (unmanaged objects) and override finalizer
      // TODO: set large fields to null
      disposedValue = true;
    }
  }

  // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
  // ~MapReader()
  // {
  //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
  //     Dispose(disposing: false);
  // }

  /// <inheritdoc />
  public void Dispose()
  {
    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }
}
