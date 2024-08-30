using System;
using System.IO;
using System.Xml;
using DotTiled.Serialization.Tmj;
using DotTiled.Serialization.Tmx;

namespace DotTiled.Serialization;

/// <summary>
/// Reads a template from a string, regardless of format.
/// </summary>
public class TemplateReader : ITemplateReader
{
  // External resolvers
  private readonly Func<string, Tileset> _externalTilesetResolver;
  private readonly Func<string, Template> _externalTemplateResolver;
  private readonly Func<string, ICustomTypeDefinition> _customTypeResolver;

  private readonly StringReader? _templateStringReader;
  private readonly XmlReader? _xmlReader;
  private readonly ITemplateReader _templateReader;
  private bool disposedValue;

  /// <summary>
  /// Constructs a new <see cref="TemplateReader"/>, capable of reading a template from a string, regardless of format.
  /// </summary>
  /// <param name="template">The string containing the template data.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeResolver">A function that resolves custom types given their source.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  public TemplateReader(
    string template,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver)
  {
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeResolver = customTypeResolver ?? throw new ArgumentNullException(nameof(customTypeResolver));

    // Prepare reader
    if (Helpers.StringIsXml(template))
    {
      _templateStringReader = new StringReader(template);
      _xmlReader = XmlReader.Create(_templateStringReader);
      _templateReader = new TxTemplateReader(_xmlReader, _externalTilesetResolver, _externalTemplateResolver, _customTypeResolver);
    }
    else
    {
      _templateReader = new TjTemplateReader(template, _externalTilesetResolver, _externalTemplateResolver, _customTypeResolver);
    }
  }

  /// <inheritdoc />
  public Template ReadTemplate() => _templateReader.ReadTemplate();

  /// <inheritdoc />
  protected virtual void Dispose(bool disposing)
  {
    if (!disposedValue)
    {
      if (disposing)
      {
        // TODO: dispose managed state (managed objects)
        _templateStringReader?.Dispose();
        _xmlReader?.Dispose();
        _templateReader.Dispose();
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
