namespace DotTiled.Model;

/// <summary>
/// Base class for custom type definitions.
/// </summary>
public abstract class CustomTypeDefinition
{
  /// <summary>
  /// The ID of the custom type.
  /// </summary>
  public uint ID { get; set; }

  /// <summary>
  /// The name of the custom type.
  /// </summary>
  public string Name { get; set; } = "";
}
