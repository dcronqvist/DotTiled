using System;

namespace DotTiled.Serialization;

/// <summary>
/// Interface for reading a template from some source. Used by the different file format parsers to read a template.
/// </summary>
public interface ITemplateReader : IDisposable
{
  /// <summary>
  /// Reads a template from the source.
  /// </summary>
  /// <returns>The parsed template.</returns>
  Template ReadTemplate();
}
