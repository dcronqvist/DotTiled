using System;
using DotTiled.Model;

namespace DotTiled.Serialization;

public interface ITemplateReader : IDisposable
{
  Template ReadTemplate();
}
