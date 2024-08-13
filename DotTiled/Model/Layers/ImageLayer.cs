namespace DotTiled;

public class ImageLayer : BaseLayer
{
  // Attributes
  public uint X { get; set; } = 0;
  public uint Y { get; set; } = 0;
  public bool RepeatX { get; set; } = false;
  public bool RepeatY { get; set; } = false;

  // At most one of
  public Image? Image { get; set; }
}
