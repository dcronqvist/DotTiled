using System.Linq;

namespace DotTiled;

/// <summary>
/// A rectangle object in a map. The existing <see cref="Object.X"/>, <see cref="Object.Y"/>, <see cref="Object.Width"/>,
/// and <see cref="Object.Height"/> properties are used to determine the size of the rectangle.
/// </summary>
public class RectangleObject : Object
{
  internal override Object Clone() => new RectangleObject
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
