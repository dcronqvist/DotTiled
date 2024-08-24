using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotTiled.Model;

/// <summary>
/// Represents a class property.
/// </summary>
public class ClassProperty : IHasProperties, IProperty<IList<IProperty>>
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
  public required IList<IProperty> Value { get; set; }

  /// <inheritdoc/>
  public IProperty Clone() => new ClassProperty
  {
    Name = Name,
    PropertyType = PropertyType,
    Value = Value.Select(property => property.Clone()).ToList()
  };

  /// <inheritdoc/>
  public IList<IProperty> GetProperties() => Value;

  /// <inheritdoc/>
  public T GetProperty<T>(string name) where T : IProperty => throw new System.NotImplementedException();

  /// <inheritdoc/>
  public bool TryGetProperty<T>(string name, [NotNullWhen(true)] out T? property) where T : IProperty
  {
    if (Value.FirstOrDefault(_properties => _properties.Name == name) is T prop)
    {
      property = prop;
      return true;
    }

    property = default;
    return false;
  }
}
