using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
  /// <param name="storageType">The storage type of the custom enum. Defaults to <see cref="CustomEnumStorageType.String"/> to be consistent with Tiled.</param>
  /// <returns></returns>
  [RequiresUnreferencedCode("Use manually defined enum properties.", Url = "https://dcronqvist.github.io/DotTiled/docs/essentials/custom-properties.html#enum-properties")]
  [RequiresDynamicCode("Use manually defined enum properties.", Url = "https://dcronqvist.github.io/DotTiled/docs/essentials/custom-properties.html#enum-properties")]
  public static CustomEnumDefinition FromEnum<T>(CustomEnumStorageType storageType = CustomEnumStorageType.String) where T : Enum
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
  /// <param name="type">The enum type to create a custom enum definition from.</param>
  /// <param name="storageType">The storage type of the custom enum. Defaults to <see cref="CustomEnumStorageType.String"/> to be consistent with Tiled.</param>
  /// <returns></returns>
  [RequiresUnreferencedCode("Use manually defined enum properties.", Url = "https://dcronqvist.github.io/DotTiled/docs/essentials/custom-properties.html#enum-properties")]
  [RequiresDynamicCode("Use manually defined enum properties.", Url = "https://dcronqvist.github.io/DotTiled/docs/essentials/custom-properties.html#enum-properties")]
  public static CustomEnumDefinition FromEnum(Type type, CustomEnumStorageType storageType = CustomEnumStorageType.String)
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
