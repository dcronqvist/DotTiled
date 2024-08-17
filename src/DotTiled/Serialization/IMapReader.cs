using System;
using DotTiled.Model;

namespace DotTiled.Serialization;

public interface IMapReader : IDisposable
{
  Map ReadMap();
}
