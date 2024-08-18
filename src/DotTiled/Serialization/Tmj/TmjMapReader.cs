using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

public class TmjMapReader : IMapReader
{
  // External resolvers
  private readonly Func<string, Tileset> _externalTilesetResolver;
  private readonly Func<string, Template> _externalTemplateResolver;

  private string _jsonString;
  private bool disposedValue;

  private readonly IReadOnlyCollection<CustomTypeDefinition> _customTypeDefinitions;

  public TmjMapReader(
    string jsonString,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    _jsonString = jsonString ?? throw new ArgumentNullException(nameof(jsonString));
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
    _customTypeDefinitions = customTypeDefinitions ?? throw new ArgumentNullException(nameof(customTypeDefinitions));
  }

  public Map ReadMap()
  {
    var jsonDoc = JsonDocument.Parse(_jsonString);
    var rootElement = jsonDoc.RootElement;
    return Tmj.ReadMap(rootElement, _externalTilesetResolver, _externalTemplateResolver, _customTypeDefinitions);
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
  // ~TmjMapReader()
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
