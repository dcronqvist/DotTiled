namespace DotTiled;

public class Template
{
  // At most one of (if the template is a tile object)
  public Tileset? Tileset { get; set; }
  public required Object Object { get; set; }
}
