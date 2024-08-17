namespace DotTiled;

public interface IProperty
{
  public string Name { get; set; }
  public PropertyType Type { get; }

  IProperty Clone();
}
