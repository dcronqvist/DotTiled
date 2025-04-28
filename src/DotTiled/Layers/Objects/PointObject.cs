using System.Linq;

namespace DotTiled;

/// <summary>
/// A point object in a map. The existing <see cref="Object.X"/> and <see cref="Object.Y"/> properties are used to
/// determine the position of the point.
/// </summary>
public class PointObject : Object
{
  internal override Object Clone() => new PointObject
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
  };
}
