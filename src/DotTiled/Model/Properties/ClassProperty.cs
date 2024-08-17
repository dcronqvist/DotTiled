using System.Collections.Generic;
using System.Linq;

namespace DotTiled.Model.Properties;

public class ClassProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => Model.Properties.PropertyType.Class;
  public required string PropertyType { get; set; }
  public required Dictionary<string, IProperty> Properties { get; set; }

  public IProperty Clone() => new ClassProperty
  {
    Name = Name,
    PropertyType = PropertyType,
    Properties = Properties.ToDictionary(p => p.Key, p => p.Value.Clone())
  };
}
