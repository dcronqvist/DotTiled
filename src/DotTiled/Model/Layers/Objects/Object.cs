using System.Collections.Generic;
using DotTiled.Model.Properties;

namespace DotTiled.Model.Layers.Objects;

public abstract class Object
{
  // Attributes
  public uint? ID { get; set; }
  public string Name { get; set; } = "";
  public string Type { get; set; } = "";
  public float X { get; set; } = 0f;
  public float Y { get; set; } = 0f;
  public float Width { get; set; } = 0f;
  public float Height { get; set; } = 0f;
  public float Rotation { get; set; } = 0f;
  public bool Visible { get; set; } = true;
  public string? Template { get; set; }

  // Elements
  public Dictionary<string, IProperty>? Properties { get; set; }
}
