namespace DotTiled.Model;

public class StringProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.String;
  public required string Value { get; set; }

  public IProperty Clone() => new StringProperty
  {
    Name = Name,
    Value = Value
  };
}
