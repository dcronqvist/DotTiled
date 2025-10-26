using System.Collections.Generic;

namespace DotTiled;

/// <summary>
/// Base class for objects in object layers.
/// </summary>
public abstract class Object : HasPropertiesBase
{
  /// <summary>
  /// Unique ID of the objects. Each object that is placed on a map gets a unique ID. Even if an object was deleted, no object gets the same ID.
  /// </summary>
  public Optional<uint> ID { get; set; } = Optional.Empty;

  /// <summary>
  /// The name of the object. An arbitrary string.
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// The class of the object. An arbitrary string.
  /// </summary>
  public string Type { get; set; } = "";

  /// <summary>
  /// The X coordinate of the object in pixels.
  /// </summary>
  public float X { get; set; } = 0f;

  /// <summary>
  /// The Y coordinate of the object in pixels.
  /// </summary>
  public float Y { get; set; } = 0f;

  /// <summary>
  /// The width of the object in pixels.
  /// </summary>
  public float Width { get; set; } = 0f;

  /// <summary>
  /// The height of the object in pixels.
  /// </summary>
  public float Height { get; set; } = 0f;

  /// <summary>
  /// The rotation of the object in degrees clockwise around (X, Y).
  /// </summary>
  public float Rotation { get; set; } = 0f;

  /// <summary>
  /// Whether the object is shown (true) or hidden (false).
  /// </summary>
  public bool Visible { get; set; } = true;

  /// <summary>
  /// A reference to a template file.
  /// </summary>
  public Optional<string> Template { get; set; } = Optional.Empty;

  /// <summary>
  /// If the object is created from a template and the template represents a tile object, this property will contain the tileset that the tile belongs to.
  /// </summary>
  public Optional<Tileset> TemplateTileset { get; set; } = Optional.Empty;

  /// <summary>
  /// Object properties.
  /// </summary>
  public List<IProperty> Properties { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Properties;

  /// <summary>
  /// Creates a deep copy of the object.
  /// </summary>
  /// <returns></returns>
  internal abstract Object Clone();
}
