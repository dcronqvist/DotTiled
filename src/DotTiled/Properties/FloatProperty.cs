namespace DotTiled;

/// <summary>
/// Represents a float property.
/// </summary>
public class FloatProperty : IProperty<float>
{
  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <inheritdoc/>
  public PropertyType Type => PropertyType.Float;

  /// <summary>
  /// The float value of the property.
  /// </summary>
  public required float Value { get; set; }

  /// <inheritdoc/>
  public IProperty Clone() => new FloatProperty
  {
    Name = Name,
    Value = Value
  };
}
