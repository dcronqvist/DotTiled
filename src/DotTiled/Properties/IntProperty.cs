namespace DotTiled;

/// <summary>
/// Represents an integer property.
/// </summary>
public class IntProperty : IProperty<int>
{
  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <inheritdoc/>
  public PropertyType Type => PropertyType.Int;

  /// <summary>
  /// The integer value of the property.
  /// </summary>
  public required int Value { get; set; }

  /// <inheritdoc/>
  public string ValueString => Value.ToString();

  /// <inheritdoc/>
  public IProperty Clone() => new IntProperty
  {
    Name = Name,
    Value = Value
  };
}
