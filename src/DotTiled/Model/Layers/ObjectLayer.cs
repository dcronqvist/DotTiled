using System.Collections.Generic;

namespace DotTiled.Model;

/// <summary>
/// Represents the order in which objects can be drawn.
/// </summary>
public enum DrawOrder
{
  /// <summary>
  /// Objects are drawn sorted by their Y coordinate.
  /// </summary>
  TopDown,

  /// <summary>
  /// Objects are drawn in the order of appearance in the object layer.
  /// </summary>
  Index
}

/// <summary>
/// Represents an object layer in a map. In Tiled documentation, it is often called an "object group".
/// </summary>
public class ObjectLayer : BaseLayer
{
  /// <summary>
  /// The X coordinate of the object layer in tiles.
  /// </summary>
  public uint X { get; set; } = 0;

  /// <summary>
  /// The Y coordinate of the object layer in tiles.
  /// </summary>
  public uint Y { get; set; } = 0;

  /// <summary>
  /// The width of the object layer in tiles. Meaningless.
  /// </summary>
  public uint? Width { get; set; }

  /// <summary>
  /// The height of the object layer in tiles. Meaningless.
  /// </summary>
  public uint? Height { get; set; }

  /// <summary>
  /// A color that is multiplied with any tile objects drawn by this layer.
  /// </summary>
  public Color? Color { get; set; }

  /// <summary>
  /// Whether the objects are drawn according to the order of appearance (<see cref="DrawOrder.Index"/>) or sorted by their Y coordinate (<see cref="DrawOrder.TopDown"/>).
  /// </summary>
  public DrawOrder DrawOrder { get; set; } = DrawOrder.TopDown;

  /// <summary>
  /// The objects in the object layer.
  /// </summary>
  public required List<Object> Objects { get; set; }
}
