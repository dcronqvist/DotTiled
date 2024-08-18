using System;
using System.Collections.Generic;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

public class TsjTilesetReader : ITilesetReader
{
  // External resolvers
  private readonly Func<string, Template> _externalTemplateResolver;

  private readonly string _jsonString;
  private bool disposedValue;

  private readonly IReadOnlyCollection<CustomTypeDefinition> _customTypeDefinitions;

  public TsjTilesetReader(
    string jsonString,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    _jsonString = jsonString ?? throw new ArgumentNullException(nameof(jsonString));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeDefinitions = customTypeDefinitions ?? throw new ArgumentNullException(nameof(customTypeDefinitions));
  }

  public Tileset ReadTileset()
  {
    var jsonDoc = System.Text.Json.JsonDocument.Parse(_jsonString);
    var rootElement = jsonDoc.RootElement;
    return Tmj.ReadTileset(
      rootElement,
      _ => throw new NotSupportedException("External tilesets cannot refer to other external tilesets."),
      _externalTemplateResolver,
      _customTypeDefinitions);
  }

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
  // ~TsjTilesetReader()
  // {
  //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
  //     Dispose(disposing: false);
  // }

  public void Dispose()
  {
    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    Dispose(disposing: true);
    System.GC.SuppressFinalize(this);
  }
}
