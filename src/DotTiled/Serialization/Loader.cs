using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotTiled.Serialization;

namespace DotTiled;

/// <summary>
/// Able to load Tiled resources from a given path.
/// </summary>
public class Loader
{
  private readonly IResourceReader _resourceReader;
  private readonly IResourceCache _resourceCache;
  private readonly IDictionary<string, ICustomTypeDefinition> _customTypeDefinitions;

  /// <summary>
  /// Initializes a new instance of the <see cref="Loader"/> class with the given <paramref name="resourceReader"/>, <paramref name="resourceCache"/>, and <paramref name="customTypeDefinitions"/>.
  /// </summary>
  /// <param name="resourceReader">A reader that is able to read Tiled resources from a given path.</param>
  /// <param name="resourceCache">A cache that stores Tiled resources for faster retrieval and reuse.</param>
  /// <param name="customTypeDefinitions">A collection of custom type definitions that can be used to resolve custom types in Tiled resources.</param>
  public Loader(
    IResourceReader resourceReader,
    IResourceCache resourceCache,
    IEnumerable<ICustomTypeDefinition> customTypeDefinitions)
  {
    _resourceReader = resourceReader;
    _resourceCache = resourceCache;
    _customTypeDefinitions = customTypeDefinitions.ToDictionary(ctd => ctd.Name);
  }

  /// <summary>
  /// Creates a new instance of a <see cref="Loader"/> with the default <see cref="FileSystemResourceReader"/> and <see cref="DefaultResourceCache"/>.
  /// </summary>
  /// <param name="customTypeDefinitions">An optional collection of custom type definitions that can be used to resolve custom types in Tiled resources.</param>
  /// <returns>A new instance of a <see cref="Loader"/>.</returns>
  public static Loader Default(IEnumerable<ICustomTypeDefinition> customTypeDefinitions = null) => new Loader(new FileSystemResourceReader(), new DefaultResourceCache(), customTypeDefinitions ?? []);

  /// <summary>
  /// Loads a map from the given <paramref name="mapPath"/>.
  /// </summary>
  /// <param name="mapPath">The path to the map file.</param>
  /// <returns>The loaded map.</returns>
  public Map LoadMap(string mapPath)
  {
    var basePath = Path.GetDirectoryName(mapPath);
    string mapContent = _resourceReader.Read(mapPath);
    using var mapReader = new MapReader(mapContent, GetTilesetResolver(basePath), GetTemplateResolver(basePath), CustomTypeResolver);
    return mapReader.ReadMap();
  }

  /// <summary>
  /// Loads a tileset from the given <paramref name="tilesetPath"/>.
  /// </summary>
  /// <param name="tilesetPath">The path to the tileset file.</param>
  /// <returns>The loaded tileset.</returns>
  public Tileset LoadTileset(string tilesetPath)
  {
    var basePath = Path.GetDirectoryName(tilesetPath);
    string tilesetContent = _resourceReader.Read(tilesetPath);
    using var tilesetReader = new TilesetReader(tilesetContent, GetTilesetResolver(basePath), GetTemplateResolver(basePath), CustomTypeResolver);
    return tilesetReader.ReadTileset();
  }

  private Func<string, Tileset> GetTilesetResolver(string basePath)
  {
    return source =>
    {
      var tilesetPath = Path.Combine(basePath, source);
      var cachedTileset = _resourceCache.GetTileset(source);
      if (cachedTileset.HasValue)
        return cachedTileset.Value;

      string tilesetContent = _resourceReader.Read(tilesetPath);
      using var tilesetReader = new TilesetReader(tilesetContent, GetTilesetResolver(basePath), GetTemplateResolver(basePath), CustomTypeResolver);
      var tileset = tilesetReader.ReadTileset();
      _resourceCache.InsertTileset(source, tileset);
      return tileset;
    };
  }

  private Func<string, Template> GetTemplateResolver(string basePath)
  {
    return source =>
    {
      var templatePath = Path.Combine(basePath, source);
      var cachedTemplate = _resourceCache.GetTemplate(source);
      if (cachedTemplate.HasValue)
        return cachedTemplate.Value;

      string templateContent = _resourceReader.Read(templatePath);
      using var templateReader = new TemplateReader(templateContent, GetTilesetResolver(basePath), GetTemplateResolver(basePath), CustomTypeResolver);
      var template = templateReader.ReadTemplate();
      _resourceCache.InsertTemplate(source, template);
      return template;
    };
  }

  private ICustomTypeDefinition CustomTypeResolver(string name) => _customTypeDefinitions[name];
}
