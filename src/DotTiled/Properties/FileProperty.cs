namespace DotTiled;

/// <summary>
/// Represents a file property.
/// </summary>
public class FileProperty : IProperty<string>
{
  /// <inheritdoc/>
  public required string Name { get; set; }

  /// <inheritdoc/>
  public PropertyType Type => PropertyType.File;

  /// <summary>
  /// The value of the property.
  /// </summary>
  public required string Value { get; set; }

  /// <inheritdoc/>
  public string ValueString => Value;

  /// <inheritdoc/>
  public IProperty Clone() => new FileProperty
  {
    Name = Name,
    Value = Value
  };
}
