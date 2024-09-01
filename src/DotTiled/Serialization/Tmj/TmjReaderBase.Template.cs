using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal Template ReadTemplate(JsonElement element)
  {
    var type = element.GetRequiredProperty<string>("type");
    var tileset = element.GetOptionalPropertyCustom<Tileset>("tileset", e => ReadTileset(e));
    var @object = element.GetRequiredPropertyCustom<DotTiled.Object>("object", ReadObject);

    return new Template
    {
      Tileset = tileset,
      Object = @object
    };
  }
}
