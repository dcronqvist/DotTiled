using System;

namespace DotTiled.Serialization.Tmj;

/// <summary>
/// A template reader for reading Tiled JSON templates.
/// </summary>
public class TjTemplateReader : TmjReaderBase, ITemplateReader
{
  /// <summary>
  /// Constructs a new <see cref="TjTemplateReader"/>.
  /// </summary>
  /// <param name="jsonString">A string containing a Tiled map in the Tiled JSON format.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeResolver">A function that resolves custom types given their name.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  public TjTemplateReader(
    string jsonString,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver) : base(
      jsonString, externalTilesetResolver, externalTemplateResolver, customTypeResolver)
  { }

  /// <inheritdoc/>
  public Template ReadTemplate() => ReadTemplate(RootElement);
}
