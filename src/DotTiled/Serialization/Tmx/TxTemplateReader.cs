using System;
using System.Xml;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

/// <summary>
/// A template reader for the Tiled XML format.
/// </summary>
public class TxTemplateReader : TmxReaderBase, ITemplateReader
{
  /// <summary>
  /// Constructs a new <see cref="TxTemplateReader"/>.
  /// </summary>
  /// <inheritdoc />
  public TxTemplateReader(
    XmlReader reader,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver) : base(
      reader, externalTilesetResolver, externalTemplateResolver, customTypeResolver)
  { }

  /// <inheritdoc/>
  public new Template ReadTemplate() => base.ReadTemplate();
}
