using System;
using System.IO;
using System.Xml;

namespace DotTiled;

public partial class TmxSerializer
{
  private readonly Func<TmxSerializer, string, Tileset> _externalTilesetResolver;
  private readonly Func<TmxSerializer, string, Template> _externalTemplateResolver;

  public TmxSerializer(
    Func<TmxSerializer, string, Tileset> externalTilesetResolver,
    Func<TmxSerializer, string, Template> externalTemplateResolver
  )
  {
    _externalTilesetResolver = externalTilesetResolver ?? throw new ArgumentNullException(nameof(externalTilesetResolver));
    _externalTemplateResolver = externalTemplateResolver ?? throw new ArgumentNullException(nameof(externalTemplateResolver));
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

  public Tileset DeserializeTileset(XmlReader reader)
  {
    reader.ReadToFollowing("tileset");
    return ReadTileset(reader);
  }

  public Template DeserializeTemplate(XmlReader reader)
  {
    reader.ReadToFollowing("template");
    return ReadTemplate(reader);
  }
}
