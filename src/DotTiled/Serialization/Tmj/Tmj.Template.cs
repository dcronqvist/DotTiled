using System;
using System.Collections.Generic;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

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
    var @object = element.GetRequiredPropertyCustom<Model.Object>("object", el => ReadObject(el, externalTemplateResolver, customTypeDefinitions));

    return new Template
    {
      Tileset = tileset,
      Object = @object
    };
  }
}
