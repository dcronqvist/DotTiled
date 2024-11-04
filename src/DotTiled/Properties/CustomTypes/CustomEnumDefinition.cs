using System;
using System.Collections.Generic;
using System.Linq;

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

  /// <summary>
  /// Creates a custom enum definition from the specified enum type.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public static CustomEnumDefinition FromEnum<T>(CustomEnumStorageType storageType = CustomEnumStorageType.Int) where T : Enum
  {
    var type = typeof(T);
    var isFlags = type.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0;

    return new CustomEnumDefinition
    {
      Name = type.Name,
      StorageType = storageType,
      Values = Enum.GetNames(type).ToList(),
      ValueAsFlags = isFlags
    };
  }

  /// <summary>
  /// Creates a custom enum definition from the specified enum type.
  /// </summary>
  /// <returns></returns>
  public static CustomEnumDefinition FromEnum(Type type, CustomEnumStorageType storageType = CustomEnumStorageType.Int)
  {
    if (!type.IsEnum)
      throw new ArgumentException("Type must be an enum.", nameof(type));

    var isFlags = type.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0;

    return new CustomEnumDefinition
    {
      Name = type.Name,
      StorageType = storageType,
      Values = Enum.GetNames(type).ToList(),
      ValueAsFlags = isFlags
    };
  }
}
