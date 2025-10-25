namespace DotTiled;

/// <summary>
/// Base class for custom type definitions.
/// </summary>
public interface ICustomTypeDefinition
{
  /// <summary>
  /// The ID of the custom type.
  /// </summary>
  uint ID { get; set; }

  /// <summary>
  /// The name of the custom type.
  /// </summary>
  string Name { get; set; }
}
