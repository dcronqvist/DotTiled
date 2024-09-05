using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

/// <summary>
/// Base class for Tiled JSON format readers.
/// </summary>
public abstract partial class TmjReaderBase : IDisposable
{
  // External resolvers
  private readonly Func<string, Tileset> _externalTilesetResolver;
  private readonly Func<string, Template> _externalTemplateResolver;
  private readonly Func<string, ICustomTypeDefinition> _customTypeResolver;

  /// <summary>
  /// The root element of the JSON document being read.
  /// </summary>
  protected JsonElement RootElement { get; private set; }

  private bool disposedValue;

  /// <summary>
  /// Constructs a new <see cref="TmjMapReader"/>.
  /// </summary>
  /// <param name="jsonString">A string containing a Tiled map in the Tiled JSON format.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeResolver">A collection of custom type definitions that can be used to resolve custom types when encountering <see cref="ClassProperty"/>.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  protected TmjReaderBase(
    string jsonString,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver)
  {
    RootElement = JsonDocument.Parse(jsonString ?? throw new ArgumentNullException(nameof(jsonString))).RootElement;
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeResolver = customTypeResolver ?? throw new ArgumentNullException(nameof(customTypeResolver));
  }

  private List<IProperty> ResolveAndMergeProperties(string className, List<IProperty> readProperties)
  {
    var classProps = Helpers.ResolveClassProperties(className, _customTypeResolver);
    return Helpers.MergeProperties(classProps, readProperties).ToList();
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
  // ~TmjMapReader()
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
