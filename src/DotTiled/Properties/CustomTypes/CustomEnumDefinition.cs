using System.Collections.Generic;

namespace DotTiled;

/// <summary>
/// Represents the storage type of a custom enum.
/// </summary>
public enum CustomEnumStorageType
{
  /// <summary>
  /// The backing value is an integer.
  /// </summary>
  Int,

  /// <summary>
  /// The backing value is a string.
  /// </summary>
  String
}

/// <summary>
/// Represents a custom enum definition in Tiled. Refer to the
/// <see href="https://doc.mapeditor.org/en/stable/manual/custom-properties/#custom-types">documentation of custom types to understand how they work</see>.
/// </summary>
public class CustomEnumDefinition : ICustomTypeDefinition
{
  /// <inheritdoc/>
  public uint ID { get; set; }

  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <summary>
  /// The storage type of the custom enum.
  /// </summary>
  public CustomEnumStorageType StorageType { get; set; }

  /// <summary>
  /// The values of the custom enum.
  /// </summary>
  public List<string> Values { get; set; } = [];

  /// <summary>
  /// Whether the value should be treated as flags.
  /// </summary>
  public bool ValueAsFlags { get; set; }
}
