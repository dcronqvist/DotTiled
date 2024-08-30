using System.Collections.Generic;
using System.Linq;

namespace DotTiled;

/// <summary>
/// Represents an enum property.
/// </summary>
public class EnumProperty : IProperty<ISet<string>>
{
  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <inheritdoc/>
  public PropertyType Type => DotTiled.PropertyType.Enum;

  /// <summary>
  /// The type of the class property. This will be the name of a custom defined
  /// type in Tiled.
  /// </summary>
  public required string PropertyType { get; set; }

  /// <summary>
  /// The value of the enum property.
  /// </summary>
  public required ISet<string> Value { get; set; }

  /// <inheritdoc/>
  public IProperty Clone() => new EnumProperty
  {
    Name = Name,
    PropertyType = PropertyType,
    Value = Value.ToHashSet()
  };

  /// <summary>
  /// Determines whether the enum property is equal to the specified value.
  /// For enums which have multiple values (e.g. flag enums), this method will only return true if it is the only value.
  /// </summary>
  /// <param name="value">The value to check.</param>
  /// <returns>True if the enum property is equal to the specified value; otherwise, false.</returns>
  public bool IsValue(string value) => Value.Contains(value) && Value.Count == 1;

  /// <summary>
  /// Determines whether the enum property has the specified value. This method is very similar to the common <see cref="System.Enum.HasFlag" /> method.
  /// </summary>
  /// <param name="value">The value to check.</param>
  /// <returns>True if the enum property has the specified value as one of its values; otherwise, false.</returns>
  public bool HasValue(string value) => Value.Contains(value);
}
