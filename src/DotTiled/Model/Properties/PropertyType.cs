namespace DotTiled.Model;

/// <summary>
/// Represents the type of a property.
/// </summary>
public enum PropertyType
{
  /// <summary>
  /// A string property.
  /// </summary>
  String,

  /// <summary>
  /// An integer property.
  /// </summary>
  Int,

  /// <summary>
  /// A float property.
  /// </summary>
  Float,

  /// <summary>
  /// A boolean property.
  /// </summary>
  Bool,

  /// <summary>
  /// A color property.
  /// </summary>
  Color,

  /// <summary>
  /// A file property.
  /// </summary>
  File,

  /// <summary>
  /// An object property.
  /// </summary>
  Object,

  /// <summary>
  /// A class property.
  /// </summary>
  Class,

  /// <summary>
  /// An enum property.
  /// </summary>
  Enum
}
