using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static BaseLayer ReadLayer(
    JsonElement element,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    var type = element.GetRequiredProperty<string>("type");

    return type switch
    {
      "tilelayer" => ReadTileLayer(element, customTypeDefinitions),
      "objectgroup" => ReadObjectLayer(element, externalTemplateResolver, customTypeDefinitions),
      "imagelayer" => ReadImageLayer(element, customTypeDefinitions),
      "group" => ReadGroup(element, externalTemplateResolver, customTypeDefinitions),
      _ => throw new JsonException($"Unsupported layer type '{type}'.")
    };
  }
}
