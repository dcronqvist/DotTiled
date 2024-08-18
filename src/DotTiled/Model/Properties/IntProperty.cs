namespace DotTiled.Model;

public class IntProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Int;
  public required int Value { get; set; }

  public IProperty Clone() => new IntProperty
  {
    Name = Name,
    Value = Value
  };
}
