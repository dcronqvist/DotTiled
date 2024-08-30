namespace DotTiled;

/// <summary>
/// Interface for properties that can be attached to objects, tiles, tilesets, maps etc.
/// </summary>
public interface IProperty
{
  /// <summary>
  /// The name of the property.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// The type of the property.
  /// </summary>
  public PropertyType Type { get; }

  /// <summary>
  /// Clones the property, only used for copying properties when performing overriding
  /// with templates.
  /// </summary>
  /// <returns>An identical, but non-reference-equal, instance of the same property.</returns>
  IProperty Clone();
}

/// <summary>
/// Interface for properties that can be attached to objects, tiles, tilesets, maps etc.
/// </summary>
/// <typeparam name="T">The type of the property value.</typeparam>
public interface IProperty<T> : IProperty
{
  /// <summary>
  /// The value of the property.
  /// </summary>
  public T Value { get; set; }
}
