namespace DotTiled;

public class ImageLayer : BaseLayer
{
  // Attributes
  public uint X { get; set; } = 0;
  public uint Y { get; set; } = 0;
  public required bool RepeatX { get; set; }
  public required bool RepeatY { get; set; }

  // At most one of
  public Image? Image { get; set; }
}
