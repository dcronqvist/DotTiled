namespace DotTiled.Model;

/// <summary>
/// Represents an integer property.
/// </summary>
public class IntProperty : IProperty
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
  public IProperty Clone() => new IntProperty
  {
    Name = Name,
    Value = Value
  };
}
