using System;
using System.Xml;

namespace DotTiled;

public class TxTemplateReader : ITemplateReader
{
  // Resolvers
  private readonly Func<string, Tileset> _externalTilesetResolver;
  private readonly Func<string, Template> _externalTemplateResolver;

  private readonly XmlReader _reader;
  private bool disposedValue;

  public TxTemplateReader(XmlReader reader, Func<string, Tileset> externalTilesetResolver, Func<string, Template> externalTemplateResolver)
  {
    _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));

    // Prepare reader
    _reader.MoveToContent();
  }

  public Template ReadTemplate() => Tmx.ReadTemplate(_reader, _externalTilesetResolver, _externalTemplateResolver);

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
  // ~TxTemplateReader()
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
