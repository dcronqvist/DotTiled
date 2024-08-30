using System;
using System.Xml;

namespace DotTiled.Serialization.Tmx;

/// <summary>
/// Base class for Tiled XML format readers.
/// </summary>
public abstract partial class TmxReaderBase : IDisposable
{
  // External resolvers
  private readonly Func<string, Tileset> _externalTilesetResolver;
  private readonly Func<string, Template> _externalTemplateResolver;
  private readonly Func<string, ICustomTypeDefinition> _customTypeResolver;

  private readonly XmlReader _reader;
  private bool disposedValue;

  /// <summary>
  /// Constructs a new <see cref="TmxReaderBase"/>, which is the base class for all Tiled XML format readers.
  /// </summary>
  /// <param name="reader">An XML reader for reading a Tiled map in the Tiled XML format.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeResolver">A function that resolves custom types given their source.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  protected TmxReaderBase(
    XmlReader reader,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver)
  {
    _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeResolver = customTypeResolver ?? throw new ArgumentNullException(nameof(customTypeResolver));

    // Prepare reader
    _ = _reader.MoveToContent();
  }

  /// <inheritdoc />
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
  // ~TmxReaderBase()
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
