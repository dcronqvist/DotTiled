using System;
using System.Collections.Generic;

namespace DotTiled.Model;

/// <summary>
/// Represents the types of objects that can use a custom class.
/// Uses the <see href="https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-flagsattribute">FlagsAttribute, for which there is plenty of documentation.</see>
/// </summary>
[Flags]
public enum CustomClassUseAs
{
  /// <summary>
  /// Any property on any kind of object.
  /// </summary>
  Property,

  /// <summary>
  /// A map.
  /// </summary>
  Map,

  /// <summary>
  /// A layer.
  /// </summary>
  Layer,

  /// <summary>
  /// An object.
  /// </summary>
  Object,

  /// <summary>
  /// A tile.
  /// </summary>
  Tile,

  /// <summary>
  /// A tileset.
  /// </summary>
  Tileset,

  /// <summary>
  /// A Wang color.
  /// </summary>
  WangColor,

  /// <summary>
  /// A Wangset.
  /// </summary>
  Wangset,

  /// <summary>
  /// A project.
  /// </summary>
  Project,

  /// <summary>
  /// All types.
  /// </summary>
  All = Property | Map | Layer | Object | Tile | Tileset | WangColor | Wangset | Project
}

/// <summary>
/// Represents a custom class definition in Tiled. Refer to the
/// <see href="https://doc.mapeditor.org/en/stable/manual/custom-properties/#custom-types">documentation of custom types to understand how they work</see>.
/// </summary>
public class CustomClassDefinition : CustomTypeDefinition
{
  /// <summary>
  /// The color of the custom class inside the Tiled editor.
  /// </summary>
  public Color? Color { get; set; }

  /// <summary>
  /// Whether the custom class should be drawn with a fill color.
  /// </summary>
  public bool DrawFill { get; set; }

  /// <summary>
  /// What the custom class can be used as, or rather, what types of objects can use it.
  /// </summary>
  public CustomClassUseAs UseAs { get; set; }

  /// <summary>
  /// The members of the custom class, with their names, types and default values.
  /// </summary>
  public List<IProperty> Members { get; set; } = [];
}
