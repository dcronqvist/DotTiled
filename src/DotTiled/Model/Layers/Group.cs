using System.Collections.Generic;

namespace DotTiled.Model;

public class Group : BaseLayer
{
  // Uses same attributes as BaseLayer

  // Any number of
  public List<BaseLayer> Layers { get; set; } = [];
}
