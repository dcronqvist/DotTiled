namespace DotTiled;

/// <summary>
/// Represents a string property.
/// </summary>
public class StringProperty : IProperty<string>
{
  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <inheritdoc/>
  public PropertyType Type => PropertyType.String;

  /// <summary>
  /// The string value of the property.
  /// </summary>
  public required string Value { get; set; }

  /// <inheritdoc/>
  public string ValueString => Value;

  /// <inheritdoc/>
  public IProperty Clone() => new StringProperty
  {
    Name = Name,
    Value = Value
  };
}
