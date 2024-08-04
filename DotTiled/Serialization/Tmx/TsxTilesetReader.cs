using System;
using System.Xml;

namespace DotTiled;

public class TsxTilesetReader : ITilesetReader
{
  // External resolvers
  private readonly Func<string, Template> _externalTemplateResolver;

  private readonly XmlReader _reader;
  private bool disposedValue;

  public TsxTilesetReader(XmlReader reader, Func<string, Template> externalTemplateResolver)
  {
    _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
  }

  public Tileset ReadTileset() => Tmx.ReadTileset(_reader, null, _externalTemplateResolver);

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
  // ~TsxTilesetReader()
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
