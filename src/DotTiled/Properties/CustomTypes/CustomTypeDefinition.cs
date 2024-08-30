namespace DotTiled;

/// <summary>
/// Base class for custom type definitions.
/// </summary>
public interface ICustomTypeDefinition
{
  /// <summary>
  /// The ID of the custom type.
  /// </summary>
  public uint ID { get; set; }

  /// <summary>
  /// The name of the custom type.
  /// </summary>
  public string Name { get; set; }
}
