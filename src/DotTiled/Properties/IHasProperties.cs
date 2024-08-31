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

  /// <summary>
  /// Maps a class property to a new instance of the specified type using reflection.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="name">The property which you want to map to a class</param>
  /// <returns></returns>
  T MapClassPropertyTo<T>(string name) where T : new();

  /// <summary>
  /// Maps all properties in this object to a new instance of the specified type using reflection.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  T MapPropertiesTo<T>() where T : new();
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

  /// <inheritdoc/>
  public T MapClassPropertyTo<T>(string name) where T : new()
  {
    var classProperty = GetProperty<ClassProperty>(name);
    return CreateMappedInstance<T>(classProperty.GetProperties());
  }

  /// <inheritdoc/>
  public T MapPropertiesTo<T>() where T : new()
  {
    var properties = GetProperties();
    return CreateMappedInstance<T>(properties);
  }

  private static object CreatedMappedInstance(Type type, IList<IProperty> properties)
  {
    var instance = Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Failed to create instance of '{type.Name}'.");

    foreach (var prop in properties)
    {
      if (type.GetProperty(prop.Name) == null)
        throw new KeyNotFoundException($"Property '{prop.Name}' not found in '{type.Name}'.");

      switch (prop)
      {
        case BoolProperty boolProp:
          type.GetProperty(prop.Name)?.SetValue(instance, boolProp.Value);
          break;
        case ColorProperty colorProp:
          type.GetProperty(prop.Name)?.SetValue(instance, colorProp.Value);
          break;
        case FloatProperty floatProp:
          type.GetProperty(prop.Name)?.SetValue(instance, floatProp.Value);
          break;
        case FileProperty fileProp:
          type.GetProperty(prop.Name)?.SetValue(instance, fileProp.Value);
          break;
        case IntProperty intProp:
          type.GetProperty(prop.Name)?.SetValue(instance, intProp.Value);
          break;
        case ObjectProperty objectProp:
          type.GetProperty(prop.Name)?.SetValue(instance, objectProp.Value);
          break;
        case StringProperty stringProp:
          type.GetProperty(prop.Name)?.SetValue(instance, stringProp.Value);
          break;
        case ClassProperty classProp:
          var subClassProp = type.GetProperty(prop.Name);
          subClassProp?.SetValue(instance, CreatedMappedInstance(subClassProp.PropertyType, classProp.GetProperties()));
          break;
        case EnumProperty enumProp:
          var enumPropInClass = type.GetProperty(prop.Name);
          var enumType = enumPropInClass?.PropertyType;
          enumPropInClass?.SetValue(instance, Enum.Parse(enumType!, string.Join(", ", enumProp.Value)));
          break;
        default:
          throw new ArgumentOutOfRangeException($"Unknown property type {prop.GetType().Name}");
      }
    }

    return instance;
  }

  private static T CreateMappedInstance<T>(IList<IProperty> properties) where T : new() => (T)CreatedMappedInstance(typeof(T), properties);
}
