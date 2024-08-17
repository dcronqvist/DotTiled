namespace DotTiled.Model.Layers;

public class TileLayer : BaseLayer
{
  // Attributes
  public uint X { get; set; } = 0;
  public uint Y { get; set; } = 0;
  public required uint Width { get; set; }
  public required uint Height { get; set; }

  // At most one of
  public Data? Data { get; set; }
}
