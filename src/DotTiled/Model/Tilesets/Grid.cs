namespace DotTiled.Model;

public enum GridOrientation
{
  Orthogonal,
  Isometric
}

public class Grid
{
  // Attributes
  public GridOrientation Orientation { get; set; } = GridOrientation.Orthogonal;
  public required uint Width { get; set; }
  public required uint Height { get; set; }
}
