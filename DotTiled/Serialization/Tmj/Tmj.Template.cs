using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static Template ReadTemplate(
    JsonElement element,
    Func<string, Tileset> externalTilesetResolver,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    var type = element.GetRequiredProperty<string>("type");
    var tileset = element.GetOptionalPropertyCustom<Tileset?>("tileset", el => ReadTileset(el, externalTilesetResolver, externalTemplateResolver, customTypeDefinitions), null);
    var @object = element.GetRequiredPropertyCustom<Object>("object", el => ReadObject(el, externalTemplateResolver, customTypeDefinitions));

    return new Template
    {
      Tileset = tileset,
      Object = @object
    };
  }
}
