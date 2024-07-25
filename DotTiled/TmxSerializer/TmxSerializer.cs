using System;
using System.Xml;

namespace DotTiled;

public partial class TmxSerializer
{
  private readonly Func<string, Tileset> _externalTilesetResolver;

  public TmxSerializer(Func<string, Tileset> externalTilesetResolver)
  {
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
  }

  public Map DeserializeMap(XmlReader reader)
  {
    reader.ReadToFollowing("map");
    return ReadMap(reader);
  }
}
