using System;
using System.Collections.Generic;
using System.Xml;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

/// <summary>
/// A tileset reader for the Tiled XML format.
/// </summary>
public class TsxTilesetReader : ITilesetReader
{
  // External resolvers
  private readonly Func<string, Template> _externalTemplateResolver;

  private readonly XmlReader _reader;
  private bool disposedValue;

  private readonly IReadOnlyCollection<ICustomTypeDefinition> _customTypeDefinitions;

  /// <summary>
  /// Constructs a new <see cref="TsxTilesetReader"/>.
  /// </summary>
  /// <param name="reader">An XML reader for reading a Tiled tileset in the Tiled XML format.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeDefinitions">A collection of custom type definitions that can be used to resolve custom types when encountering <see cref="ClassProperty"/>.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  public TsxTilesetReader(
    XmlReader reader,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeDefinitions = customTypeDefinitions ?? throw new ArgumentNullException(nameof(customTypeDefinitions));

    // Prepare reader
    _ = _reader.MoveToContent();
  }

  /// <inheritdoc/>
  public Tileset ReadTileset() => Tmx.ReadTileset(_reader, null, _externalTemplateResolver, _customTypeDefinitions);

  /// <inheritdoc/>
  protected virtual void Dispose(bool disposing)
  {
    if (!disposedValue)
    {
      if (disposing)
      {
        // TODO: dispose managed state (managed objects)
        _reader.Dispose();
      }

      // TODO: free unmanaged resources (unmanaged objects) and override finalizer
      // TODO: set large fields to null
      disposedValue = true;
    }
  }

  // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
  // ~TsxTilesetReader()
  // {
  //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
  //     Dispose(disposing: false);
  // }

  /// <inheritdoc/>
  public void Dispose()
  {
    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }
}
