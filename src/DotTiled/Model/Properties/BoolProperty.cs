namespace DotTiled.Model;

/// <summary>
/// Represents a boolean property.
/// </summary>
public class BoolProperty : IProperty<bool>
{
  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <inheritdoc/>
  public PropertyType Type => PropertyType.Bool;

  /// <summary>
  /// The boolean value of the property.
  /// </summary>
  public required bool Value { get; set; }

  /// <inheritdoc/>
  public IProperty Clone() => new BoolProperty
  {
    Name = Name,
    Value = Value
  };
}
