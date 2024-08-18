namespace DotTiled.Model;

public class FloatProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Float;
  public required float Value { get; set; }

  public IProperty Clone() => new FloatProperty
  {
    Name = Name,
    Value = Value
  };
}
