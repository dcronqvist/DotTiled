using System;
using System.Collections.Generic;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

internal partial class Tmj
{
  internal static BaseLayer ReadLayer(
    JsonElement element,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
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
