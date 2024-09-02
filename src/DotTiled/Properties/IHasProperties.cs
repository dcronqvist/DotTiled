using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotTiled;

/// <summary>
/// Interface for objects that have properties attached to them.
/// </summary>
public interface IHasProperties
{
  /// <summary>
  /// The properties attached to the object.
  /// </summary>
  IList<IProperty> GetProperties();

  /// <summary>
  /// Tries to get a property of the specified type with the specified name.
  /// </summary>
  /// <typeparam name="T">The type of the property to get.</typeparam>
  /// <param name="name">The name of the property to get.</param>
  /// <param name="property">The property with the specified name, if found.</param>
  /// <returns>True if a property with the specified name was found; otherwise, false.</returns>
  bool TryGetProperty<T>(string name, out T property) where T : IProperty;

  /// <summary>
  /// Gets a property of the specified type with the specified name.
  /// </summary>
  /// <typeparam name="T">The type of the property to get.</typeparam>
  /// <param name="name">The name of the property to get.</param>
  /// <returns>The property with the specified name.</returns>
  T GetProperty<T>(string name) where T : IProperty;
}

/// <summary>
/// Interface for objects that have properties attached to them.
/// </summary>
public abstract class HasPropertiesBase : IHasProperties
{
  /// <inheritdoc/>
  public abstract IList<IProperty> GetProperties();

  /// <inheritdoc/>
  /// <exception cref="KeyNotFoundException">Thrown when a property with the specified name is not found.</exception>
  /// <exception cref="InvalidCastException">Thrown when a property with the specified name is not of the specified type.</exception>
  public T GetProperty<T>(string name) where T : IProperty
  {
    var properties = GetProperties();
    var property = properties.FirstOrDefault(_properties => _properties.Name == name) ?? throw new KeyNotFoundException($"Property '{name}' not found.");
    if (property is T prop)
    {
      return prop;
    }

    throw new InvalidCastException($"Property '{name}' is not of type '{typeof(T).Name}'.");
  }

  /// <inheritdoc/>
  public bool TryGetProperty<T>(string name, [NotNullWhen(true)] out T property) where T : IProperty
  {
    var properties = GetProperties();
    if (properties.FirstOrDefault(_properties => _properties.Name == name) is T prop)
    {
      property = prop;
      return true;
    }

    property = default;
    return false;
  }
}
