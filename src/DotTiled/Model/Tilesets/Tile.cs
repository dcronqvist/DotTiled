using System.Collections.Generic;

namespace DotTiled.Model;

public class Tile
{
  // Attributes
  public required uint ID { get; set; }
  public string Type { get; set; } = "";
  public float Probability { get; set; } = 0f;
  public uint X { get; set; } = 0;
  public uint Y { get; set; } = 0;
  public required uint Width { get; set; }
  public required uint Height { get; set; }

  // Elements
  public Dictionary<string, IProperty>? Properties { get; set; }
  public Image? Image { get; set; }
  public ObjectLayer? ObjectLayer { get; set; }
  public List<Frame>? Animation { get; set; }
}
