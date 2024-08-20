using System.Collections.Generic;
using System.Linq;

namespace DotTiled.Model;

/// <summary>
/// Represents a class property.
/// </summary>
public class ClassProperty : IProperty
{
  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <inheritdoc/>
  public PropertyType Type => Model.PropertyType.Class;

  /// <summary>
  /// The type of the class property. This will be the name of a custom defined
  /// type in Tiled.
  /// </summary>
  public required string PropertyType { get; set; }

  /// <summary>
  /// The properties of the class property.
  /// </summary>
  public required Dictionary<string, IProperty> Properties { get; set; }

  /// <inheritdoc/>
  public IProperty Clone() => new ClassProperty
  {
    Name = Name,
    PropertyType = PropertyType,
    Properties = Properties.ToDictionary(p => p.Key, p => p.Value.Clone())
  };
}
