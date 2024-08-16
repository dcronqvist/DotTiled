using System;

namespace DotTiled;

public interface IMapReader : IDisposable
{
  Map ReadMap();
}
