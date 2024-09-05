using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotTiled;

/// <summary>
/// Represents the types of objects that can use a custom class.
/// Uses the <see href="https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-flagsattribute">FlagsAttribute, for which there is plenty of documentation.</see>
/// </summary>
[Flags]
public enum CustomClassUseAs
{
  /// <summary>
  /// Any property on any kind of object.
  /// </summary>
  Property,

  /// <summary>
  /// A map.
  /// </summary>
  Map,

  /// <summary>
  /// A layer.
  /// </summary>
  Layer,

  /// <summary>
  /// An object.
  /// </summary>
  Object,

  /// <summary>
  /// A tile.
  /// </summary>
  Tile,

  /// <summary>
  /// A tileset.
  /// </summary>
  Tileset,

  /// <summary>
  /// A Wang color.
  /// </summary>
  WangColor,

  /// <summary>
  /// A Wangset.
  /// </summary>
  Wangset,

  /// <summary>
  /// A project.
  /// </summary>
  Project,

  /// <summary>
  /// All types.
  /// </summary>
  All = Property | Map | Layer | Object | Tile | Tileset | WangColor | Wangset | Project
}

/// <summary>
/// Represents a custom class definition in Tiled. Refer to the
/// <see href="https://doc.mapeditor.org/en/stable/manual/custom-properties/#custom-types">documentation of custom types to understand how they work</see>.
/// </summary>
public class CustomClassDefinition : HasPropertiesBase, ICustomTypeDefinition
{
  /// <inheritdoc/>
  public uint ID { get; set; }

  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <summary>
  /// The color of the custom class inside the Tiled editor.
  /// </summary>
  public Color Color { get; set; }

  /// <summary>
  /// Whether the custom class should be drawn with a fill color.
  /// </summary>
  public bool DrawFill { get; set; }

  /// <summary>
  /// What the custom class can be used as, or rather, what types of objects can use it.
  /// </summary>
  public CustomClassUseAs UseAs { get; set; }

  /// <summary>
  /// The members of the custom class, with their names, types and default values.
  /// </summary>
  public List<IProperty> Members { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Members;

  /// <summary>
  /// Creates a new <see cref="CustomClassDefinition"/> from the specified class type.
  /// </summary>
  /// <param name="type">The type of the class to create a custom class definition from.</param>
  /// <returns>A new <see cref="CustomClassDefinition"/> instance.</returns>
  /// <exception cref="ArgumentException">Thrown when the specified type is not a class.</exception>
  public static CustomClassDefinition FromClassType(Type type)
  {
    if (type == typeof(string) || !type.IsClass)
      throw new ArgumentException("Type must be a class.", nameof(type));

    return FromClass(() => Activator.CreateInstance(type));
  }

  /// <summary>
  /// Creates a new <see cref="CustomClassDefinition"/> from the specified instance of a class.
  /// </summary>
  /// <param name="instance">The instance of the class to create a custom class definition from.</param>
  /// <returns>A new <see cref="CustomClassDefinition"/> instance.</returns>
  public static CustomClassDefinition FromClassInstance(dynamic instance)
  {
    ArgumentNullException.ThrowIfNull(instance);
    return FromClass(() => instance);
  }

  /// <summary>
  /// Creates a new <see cref="CustomClassDefinition"/> from the specified constructible class type.
  /// </summary>
  /// <typeparam name="T">The type of the class to create a custom class definition from.</typeparam>
  /// <returns>A new <see cref="CustomClassDefinition"/> instance.</returns>
  public static CustomClassDefinition FromClass<T>() where T : class, new() => FromClass(() => new T());

  /// <summary>
  /// Creates a new <see cref="CustomClassDefinition"/> from the specified factory function of a class instance.
  /// </summary>
  /// <typeparam name="T">The type of the class to create a custom class definition from.</typeparam>
  /// <param name="factory">The factory function that creates an instance of the class.</param>
  /// <returns>A new <see cref="CustomClassDefinition"/> instance.</returns>
  public static CustomClassDefinition FromClass<T>(Func<T> factory) where T : class
  {
    var instance = factory();
    var type = instance.GetType();
    var properties = type.GetProperties();

    return new CustomClassDefinition
    {
      Name = type.Name,
      UseAs = CustomClassUseAs.All,
      Members = properties.Select(p => ConvertPropertyInfoToIProperty(instance, p)).ToList()
    };
  }

  private static IProperty ConvertPropertyInfoToIProperty(object instance, PropertyInfo propertyInfo)
  {
    switch (propertyInfo.PropertyType)
    {
      case Type t when t == typeof(bool):
        return new BoolProperty { Name = propertyInfo.Name, Value = (bool)propertyInfo.GetValue(instance) };
      case Type t when t == typeof(Color):
        return new ColorProperty { Name = propertyInfo.Name, Value = (Color)propertyInfo.GetValue(instance) };
      case Type t when t == typeof(float):
        return new FloatProperty { Name = propertyInfo.Name, Value = (float)propertyInfo.GetValue(instance) };
      case Type t when t == typeof(string):
        return new StringProperty { Name = propertyInfo.Name, Value = (string)propertyInfo.GetValue(instance) };
      case Type t when t == typeof(int):
        return new IntProperty { Name = propertyInfo.Name, Value = (int)propertyInfo.GetValue(instance) };
      case Type t when t.IsClass:
        return new ClassProperty { Name = propertyInfo.Name, PropertyType = t.Name, Value = GetNestedProperties(propertyInfo.PropertyType, propertyInfo.GetValue(instance)) };
      default:
        break;
    }

    throw new NotSupportedException($"Type '{propertyInfo.PropertyType.Name}' is not supported in custom classes.");
  }

  private static List<IProperty> GetNestedProperties(Type type, object instance)
  {
    var defaultInstance = Activator.CreateInstance(type);
    var properties = type.GetProperties();

    bool IsPropertyDefaultValue(PropertyInfo propertyInfo)
    {
      var defaultValue = propertyInfo.GetValue(defaultInstance);
      var value = propertyInfo.GetValue(instance);
      return value.Equals(defaultValue);
    }

    return properties
      .Where(p => !IsPropertyDefaultValue(p))
      .Select(p => ConvertPropertyInfoToIProperty(instance, p)).ToList();
  }
}
