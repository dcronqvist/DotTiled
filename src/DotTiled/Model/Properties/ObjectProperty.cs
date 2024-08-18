namespace DotTiled.Model;

public class ObjectProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Object;
  public required uint Value { get; set; }

  public IProperty Clone() => new ObjectProperty
  {
    Name = Name,
    Value = Value
  };
}
