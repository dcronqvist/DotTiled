using System;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

/// <summary>
/// A map reader for reading Tiled JSON maps.
/// </summary>
public class TmjMapReader : TmjReaderBase, IMapReader
{
  /// <summary>
  /// Constructs a new <see cref="TmjMapReader"/>.
  /// </summary>
  /// <param name="jsonString">A string containing a Tiled map in the Tiled JSON format.</param>
  /// <param name="externalTilesetResolver">A function that resolves external tilesets given their source.</param>
  /// <param name="externalTemplateResolver">A function that resolves external templates given their source.</param>
  /// <param name="customTypeResolver">A function that resolves custom types given their name.</param>
  /// <exception cref="ArgumentNullException">Thrown when any of the arguments are null.</exception>
  public TmjMapReader(
    string jsonString,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    Func<string, ICustomTypeDefinition> customTypeResolver) : base(
      jsonString, externalTilesetResolver, externalTemplateResolver, customTypeResolver)
  { }

  /// <inheritdoc/>
  public Map ReadMap() => ReadMap(RootElement);
}
