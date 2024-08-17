using System.Collections.Generic;
using DotTiled.Model.Properties;

namespace DotTiled.Model.Layers;

public abstract class BaseLayer
{
  // Attributes
  public required uint ID { get; set; }
  public string Name { get; set; } = "";
  public string Class { get; set; } = "";
  public float Opacity { get; set; } = 1.0f;
  public bool Visible { get; set; } = true;
  public Color? TintColor { get; set; }
  public float OffsetX { get; set; } = 0.0f;
  public float OffsetY { get; set; } = 0.0f;
  public float ParallaxX { get; set; } = 1.0f;
  public float ParallaxY { get; set; } = 1.0f;

  // At most one of
  public Dictionary<string, IProperty>? Properties { get; set; }
}
