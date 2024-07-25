using System.Collections.Generic;

namespace DotTiled;

public enum PropertyType
{
  String,
  Int,
  Float,
  Bool,
  Color,
  File,
  Object,
  Class
}

public interface IProperty
{
  public string Name { get; set; }
  public PropertyType Type { get; }
}

public class StringProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.String;
  public required string Value { get; set; }
}

public class IntProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Int;
  public required int Value { get; set; }
}

public class FloatProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Float;
  public required float Value { get; set; }
}

public class BoolProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Bool;
  public required bool Value { get; set; }
}

public class ColorProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Color;
  public required Color Value { get; set; }
}

public class FileProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.File;
  public required string Value { get; set; }
}

public class ObjectProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => PropertyType.Object;
  public required uint Value { get; set; }
}

public class ClassProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => DotTiled.PropertyType.Class;
  public required string PropertyType { get; set; }
  public required Dictionary<string, IProperty> Properties { get; set; }
}
