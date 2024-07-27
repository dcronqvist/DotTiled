using System;
using System.IO;
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

  public Map DeserializeMap(string xml)
  {
    using var stringReader = new StringReader(xml);
    using var reader = XmlReader.Create(stringReader);
    return DeserializeMap(reader);
  }
}
