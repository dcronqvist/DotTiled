using System;

namespace DotTiled;

public interface ITemplateReader : IDisposable
{
  Template ReadTemplate();
}
