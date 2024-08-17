using System;
using System.Collections.Generic;

namespace DotTiled.Model.Properties.CustomTypes;

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
  public Color? Color { get; set; }
  public bool DrawFill { get; set; }
  public CustomClassUseAs UseAs { get; set; }
  public List<IProperty> Members { get; set; } = [];
}
