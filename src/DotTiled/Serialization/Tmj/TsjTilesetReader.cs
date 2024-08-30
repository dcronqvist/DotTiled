using System;

namespace DotTiled.Serialization.Tmj;

/// <summary>
/// A tileset reader for the Tiled JSON format.
/// </summary>
public class TsjTilesetReader : TmjReaderBase, ITilesetReader
{
  /// <summary>
  /// Constructs a new <see cref="TsjTilesetReader"/>.
  /// </summary>
  /// <param name="jsonString">A string containing a Tiled map in the Tiled JSON format.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeResolver">A function that resolves custom types given their name.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  public TsjTilesetReader(
    string jsonString,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver) : base(
      jsonString, externalTilesetResolver, externalTemplateResolver, customTypeResolver)
  { }

  /// <inheritdoc/>
  public Tileset ReadTileset() => ReadTileset(RootElement);
}
