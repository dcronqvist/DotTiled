namespace DotTiled.Model.Properties;

public class BoolProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Bool;
  public required bool Value { get; set; }

  public IProperty Clone() => new BoolProperty
  {
    Name = Name,
    Value = Value
  };
}
