namespace DotTiled;

public class ImageLayer : BaseLayer
{
  // Attributes
  public required bool RepeatX { get; set; }
  public required bool RepeatY { get; set; }

  // At most one of
  public Image? Image { get; set; }
}
