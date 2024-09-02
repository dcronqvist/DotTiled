using System.Collections.Generic;

namespace DotTiled.Serialization;

/// <summary>
/// A default implementation of <see cref="IResourceCache"/> that uses an in-memory dictionary to cache resources.
/// </summary>
public class DefaultResourceCache : IResourceCache
{
  private readonly Dictionary<string, Template> _templates = [];
  private readonly Dictionary<string, Tileset> _tilesets = [];

  /// <inheritdoc/>
  public Optional<Template> GetTemplate(string path)
  {
    if (_templates.TryGetValue(path, out var template))
      return new Optional<Template>(template);

    return Optional<Template>.Empty;
  }

  /// <inheritdoc/>
  public Optional<Tileset> GetTileset(string path)
  {
    if (_tilesets.TryGetValue(path, out var tileset))
      return new Optional<Tileset>(tileset);

    return Optional<Tileset>.Empty;
  }

  /// <inheritdoc/>
  public void InsertTemplate(string path, Template template) => _templates[path] = template;

  /// <inheritdoc/>
  public void InsertTileset(string path, Tileset tileset) => _tilesets[path] = tileset;
}
