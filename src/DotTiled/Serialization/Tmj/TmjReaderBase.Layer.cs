using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal BaseLayer ReadLayer(JsonElement element)
  {
    var type = element.GetRequiredProperty<string>("type");

    return type switch
    {
      "tilelayer" => ReadTileLayer(element),
      "objectgroup" => ReadObjectLayer(element),
      "imagelayer" => ReadImageLayer(element),
      "group" => ReadGroup(element),
      _ => throw new JsonException($"Unsupported layer type '{type}'.")
    };
  }
}
