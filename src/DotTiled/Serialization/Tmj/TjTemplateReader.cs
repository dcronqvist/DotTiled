using System;
using System.Collections.Generic;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

/// <summary>
/// A template reader for reading Tiled JSON templates.
/// </summary>
public class TjTemplateReader : ITemplateReader
{
  // External resolvers
  private readonly Func<string, Tileset> _externalTilesetResolver;
  private readonly Func<string, Template> _externalTemplateResolver;

  private readonly string _jsonString;
  private bool disposedValue;

  private readonly IReadOnlyCollection<ICustomTypeDefinition> _customTypeDefinitions;

  /// <summary>
  /// Constructs a new <see cref="TjTemplateReader"/>.
  /// </summary>
  /// <param name="jsonString">A string containing a Tiled template in the Tiled JSON format.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeDefinitions">A collection of custom type definitions that can be used to resolve custom types when encountering <see cref="ClassProperty"/>.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  public TjTemplateReader(
    string jsonString,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    _jsonString = jsonString ?? throw new ArgumentNullException(nameof(jsonString));
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeDefinitions = customTypeDefinitions ?? throw new ArgumentNullException(nameof(customTypeDefinitions));
  }

  /// <inheritdoc/>
  public Template ReadTemplate()
  {
    var jsonDoc = System.Text.Json.JsonDocument.Parse(_jsonString);
    var rootElement = jsonDoc.RootElement;
    return Tmj.ReadTemplate(rootElement, _externalTilesetResolver, _externalTemplateResolver, _customTypeDefinitions);
  }

  /// <inheritdoc/>
  protected virtual void Dispose(bool disposing)
  {
    if (!disposedValue)
    {
      if (disposing)
      {
        // TODO: dispose managed state (managed objects)
      }

      // TODO: free unmanaged resources (unmanaged objects) and override finalizer
      // TODO: set large fields to null
      disposedValue = true;
    }
  }

  // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
  // ~TjTemplateReader()
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
