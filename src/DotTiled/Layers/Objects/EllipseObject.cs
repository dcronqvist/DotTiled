using System.Linq;

namespace DotTiled;

/// <summary>
/// An ellipse object in a map. The existing <see cref="Object.X"/>, <see cref="Object.Y"/>, <see cref="Object.Width"/>,
/// and <see cref="Object.Height"/> properties are used to determine the size of the ellipse.
/// </summary>
public class EllipseObject : Object
{
  internal override Object Clone() => new EllipseObject
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
