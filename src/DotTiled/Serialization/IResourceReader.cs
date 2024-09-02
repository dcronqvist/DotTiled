namespace DotTiled;

/// <summary>
/// Able to read resources from a given path.
/// </summary>
public interface IResourceReader
{
  /// <summary>
  /// Reads a Tiled resource from a given path.
  /// </summary>
  /// <param name="resourcePath">The path to the Tiled resource, which can be a Map file, Tileset file, Template file, etc.</param>
  /// <returns>The content of the resource as a string.</returns>
  string Read(string resourcePath);
}
