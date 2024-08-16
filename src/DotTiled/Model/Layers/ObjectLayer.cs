using System.Collections.Generic;
using DotTiled.Model.Layers.Objects;

namespace DotTiled.Model.Layers;

public enum DrawOrder
{
  TopDown,
  Index
}

public class ObjectLayer : BaseLayer
{
  // Attributes
  public uint X { get; set; } = 0;
  public uint Y { get; set; } = 0;
  public uint? Width { get; set; }
  public uint? Height { get; set; }
  public Color? Color { get; set; }
  public DrawOrder DrawOrder { get; set; } = DrawOrder.TopDown;

  // Elements
  public required List<Object> Objects { get; set; }
}
