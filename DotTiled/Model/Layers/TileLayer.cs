namespace DotTiled;

public class TileLayer : BaseLayer
{
  // Attributes
  public required uint Width { get; set; }
  public required uint Height { get; set; }

  // Elements
  public Data? Data { get; set; }
}
