namespace DotTiled.Serialization;

/// <summary>
/// Interface for a cache that stores Tiled resources for faster retrieval and reuse.
/// </summary>
public interface IResourceCache
{
  /// <summary>
  /// Inserts a tileset into the cache with the given <paramref name="path"/>.
  /// </summary>
  /// <param name="path">The path to the tileset file.</param>
  /// <param name="tileset">The tileset to insert into the cache.</param>
  void InsertTileset(string path, Tileset tileset);

  /// <summary>
  /// Retrieves a tileset from the cache with the given <paramref name="path"/>.
  /// </summary>
  /// <param name="path">The path to the tileset file.</param>
  /// <returns>The tileset if it exists in the cache; otherwise, <see cref="Optional{Tileset}.Empty"/>.</returns>
  Optional<Tileset> GetTileset(string path);

  /// <summary>
  /// Inserts a template into the cache with the given <paramref name="path"/>.
  /// </summary>
  /// <param name="path">The path to the template file.</param>
  /// <param name="template">The template to insert into the cache.</param>
  void InsertTemplate(string path, Template template);

  /// <summary>
  /// Retrieves a template from the cache with the given <paramref name="path"/>.
  /// </summary>
  /// <param name="path">The path to the template file.</param>
  /// <returns>The template if it exists in the cache; otherwise, <see cref="Optional{Template}.Empty"/>.</returns>
  Optional<Template> GetTemplate(string path);
}
