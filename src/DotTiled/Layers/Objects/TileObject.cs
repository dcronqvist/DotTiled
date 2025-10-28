using System.Linq;

namespace DotTiled;

/// <summary>
/// A tile object in a map.
/// </summary>
public class TileObject : Object
{
  /// <summary>
  /// A reference to a tile.
  /// </summary>
  public uint GID { get; set; }

  /// <summary>
  /// The flipping flags for the tile.
  /// </summary>
  public FlippingFlags FlippingFlags { get; set; }

  internal override Object Clone() => new TileObject
  {
    ID = ID,
    Name = Name,
    Type = Type,
    X = X,
    Y = Y,
    Width = Width,
    Height = Height,
    Rotation = Rotation,
    Visible = Visible,
    Template = Template,
    Properties = Properties.Select(p => p.Clone()).ToList(),
    GID = GID,
    FlippingFlags = FlippingFlags,
  };
}
