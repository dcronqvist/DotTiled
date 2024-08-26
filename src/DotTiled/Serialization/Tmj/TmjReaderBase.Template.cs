using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal Template ReadTemplate(JsonElement element)
  {
    var type = element.GetRequiredProperty<string>("type");
    var tileset = element.GetOptionalPropertyCustom<Tileset?>("tileset", ReadTileset, null);
    var @object = element.GetRequiredPropertyCustom<Model.Object>("object", ReadObject);

    return new Template
    {
      Tileset = tileset,
      Object = @object
    };
  }
}
