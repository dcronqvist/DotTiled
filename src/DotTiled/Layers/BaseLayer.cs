using System.Collections.Generic;

namespace DotTiled;

/// <summary>
/// Base class for all layer types in a map.
/// To check the type of a layer, <see href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching">use C# pattern matching</see>,
/// or some other mechanism to determine the type of the layer at runtime.
/// </summary>
public abstract class BaseLayer : HasPropertiesBase
{
  /// <summary>
  /// Unique ID of the layer. Each layer that is added to a map gets a unique ID. Even if a layer is deleted, no layer ever gets the same ID.
  /// </summary>
  public required uint ID { get; set; }

  /// <summary>
  /// The name of the layer.
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// The class of the layer.
  /// </summary>
  public string Class { get; set; } = "";

  /// <summary>
  /// The opacity of the layer as a value from 0 (fully transparent) to 1 (fully opaque).
  /// </summary>
  public float Opacity { get; set; } = 1.0f;

  /// <summary>
  /// Whether the layer is shown (true) or hidden (false).
  /// </summary>
  public bool Visible { get; set; } = true;

  /// <summary>
  /// A tint color that is multiplied with any tiles drawn by this layer.
  /// </summary>
  public Optional<TiledColor> TintColor { get; set; } = Optional<TiledColor>.Empty;

  /// <summary>
  /// Horizontal offset for this layer in pixels.
  /// </summary>
  public float OffsetX { get; set; } = 0.0f;

  /// <summary>
  /// Vertical offset for this layer in pixels.
  /// </summary>
  public float OffsetY { get; set; } = 0.0f;

  /// <summary>
  /// Horizontal parallax factor for this layer.
  /// </summary>
  public float ParallaxX { get; set; } = 1.0f;

  /// <summary>
  /// Vertical parallax factor for this layer.
  /// </summary>
  public float ParallaxY { get; set; } = 1.0f;

  /// <summary>
  /// Layer properties.
  /// </summary>
  public List<IProperty> Properties { get; set; } = [];

  /// <inheritdoc/>
  public override IList<IProperty> GetProperties() => Properties;
}
