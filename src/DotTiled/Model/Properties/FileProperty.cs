namespace DotTiled.Model;

public class FileProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.File;
  public required string Value { get; set; }

  public IProperty Clone() => new FileProperty
  {
    Name = Name,
    Value = Value
  };
}
