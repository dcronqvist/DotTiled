namespace DotTiled;

public class ColorProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Color;
  public required Color Value { get; set; }

  public IProperty Clone() => new ColorProperty
  {
    Name = Name,
    Value = Value
  };
}
