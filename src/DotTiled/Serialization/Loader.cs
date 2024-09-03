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
  /// Creates a new instance of a <see cref="Loader"/> with the default <see cref="FileSystemResourceReader"/> and <see cref="DefaultResourceCache"/>, and no available custom type definitions.
  /// </summary>
  /// <returns>A new instance of a <see cref="Loader"/>.</returns>
  public static Loader Default() => new Loader(new FileSystemResourceReader(), new DefaultResourceCache(), []);

  /// <summary>
  /// Creates a new instance of a <see cref="Loader"/> with the ability to override the default <see cref="FileSystemResourceReader"/>, <see cref="DefaultResourceCache"/>, and custom type definitions.
  /// </summary>
  /// <param name="resourceReader">A reader that is able to read Tiled resources from a given path.</param>
  /// <param name="resourceCache">A cache that stores Tiled resources for faster retrieval and reuse.</param>
  /// <param name="customTypeDefinitions">A collection of custom type definitions that can be used to resolve custom types in Tiled resources.</param>
  /// <returns></returns>
  public static Loader DefaultWith(
    IResourceReader resourceReader = null,
    IResourceCache resourceCache = null,
    IEnumerable<ICustomTypeDefinition> customTypeDefinitions = null) =>
    new Loader(
      resourceReader ?? new FileSystemResourceReader(),
      resourceCache ?? new DefaultResourceCache(),
      customTypeDefinitions ?? Array.Empty<ICustomTypeDefinition>());

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

  private Func<string, T> GetResolverFunc<T>(
    string basePath,
    Func<string, Optional<T>> cacheResolver,
    Action<string, T> cacheInsert,
    Func<string, T> resolveFromContent)
  {
    return source =>
    {
      var resourcePath = Path.Combine(basePath, source);
      var cachedResource = cacheResolver(resourcePath);
      if (cachedResource.HasValue)
        return cachedResource.Value;

      string tilesetContent = _resourceReader.Read(resourcePath);
      var resource = resolveFromContent(tilesetContent);
      cacheInsert(resourcePath, resource);
      return resource;
    };
  }

  private Func<string, Tileset> GetTilesetResolver(string basePath) =>
    GetResolverFunc<Tileset>(basePath, _resourceCache.GetTileset, _resourceCache.InsertTileset,
      tilesetContent =>
      {
        using var tilesetReader = new TilesetReader(tilesetContent, GetTilesetResolver(basePath), GetTemplateResolver(basePath), CustomTypeResolver);
        return tilesetReader.ReadTileset();
      });

  private Func<string, Template> GetTemplateResolver(string basePath) =>
    GetResolverFunc<Template>(basePath, _resourceCache.GetTemplate, _resourceCache.InsertTemplate,
      templateContent =>
      {
        using var templateReader = new TemplateReader(templateContent, GetTilesetResolver(basePath), GetTemplateResolver(basePath), CustomTypeResolver);
        return templateReader.ReadTemplate();
      });

  private ICustomTypeDefinition CustomTypeResolver(string name) => _customTypeDefinitions[name];
}
