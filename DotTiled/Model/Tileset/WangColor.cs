using System.Collections.Generic;

namespace DotTiled;

public class WangColor
{
  // Attributes
  public required string Name { get; set; }
  public string Class { get; set; } = "";
  public required Color Color { get; set; }
  public required uint Tile { get; set; }
  public float Probability { get; set; } = 0f;

  // Elements
  public Dictionary<string, IProperty>? Properties { get; set; }
}
