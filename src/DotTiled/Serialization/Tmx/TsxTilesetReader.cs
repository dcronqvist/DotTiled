using System;
using System.Xml;

namespace DotTiled.Serialization.Tmx;

/// <summary>
/// A tileset reader for the Tiled XML format.
/// </summary>
public class TsxTilesetReader : TmxReaderBase, ITilesetReader
{
  /// <summary>
  /// Constructs a new <see cref="TsxTilesetReader"/>.
  /// </summary>
  /// <inheritdoc />
  public TsxTilesetReader(
    XmlReader reader,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver) : base(
      reader, externalTilesetResolver, externalTemplateResolver, customTypeResolver)
  { }

  /// <inheritdoc/>
  public Tileset ReadTileset() => base.ReadTileset();
}
