using System;
using System.Xml;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmx;

/// <summary>
/// A map reader for the Tiled XML format.
/// </summary>
public class TmxMapReader : TmxReaderBase, IMapReader
{
  /// <summary>
  /// Constructs a new <see cref="TmxMapReader"/>.
  /// </summary>
  /// <inheritdoc />
  public TmxMapReader(
    XmlReader reader,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver) : base(
      reader, externalTilesetResolver, externalTemplateResolver, customTypeResolver)
  { }

  /// <inheritdoc/>
  public new Map ReadMap() => base.ReadMap();
}
