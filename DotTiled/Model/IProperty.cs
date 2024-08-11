using System;
using System.Collections.Generic;
using System.Linq;

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

  IProperty Clone();
}

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

public class ClassProperty : IProperty
{
  public required string Name { get; set; }
  public PropertyType Type => DotTiled.PropertyType.Class;
  public required string PropertyType { get; set; }
  public required Dictionary<string, IProperty> Properties { get; set; }

  public IProperty Clone() => new ClassProperty
  {
    Name = Name,
    PropertyType = PropertyType,
    Properties = Properties.ToDictionary(p => p.Key, p => p.Value.Clone())
  };
}

public abstract class CustomTypeDefinition
{
  public uint ID { get; set; }
  public string Name { get; set; } = "";
}

[Flags]
public enum CustomClassUseAs
{
  Property,
  Map,
  Layer,
  Object,
  Tile,
  Tileset,
  WangColor,
  Wangset,
  Project,
  All = Property | Map | Layer | Object | Tile | Tileset | WangColor | Wangset | Project
}

public class CustomClassDefinition : CustomTypeDefinition
{
  public Color Color { get; set; }
  public bool DrawFill { get; set; }
  public CustomClassUseAs UseAs { get; set; }
  public List<IProperty> Members { get; set; }
}

public enum CustomEnumStorageType
{
  Int,
  String
}

public class CustomEnumDefinition : CustomTypeDefinition
{
  public CustomEnumStorageType StorageType { get; set; }
  public List<string> Values { get; set; } = [];
  public bool ValueAsFlags { get; set; }
}
