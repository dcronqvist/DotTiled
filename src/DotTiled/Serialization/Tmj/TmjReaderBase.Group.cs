using System.Collections.Generic;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal Group ReadGroup(JsonElement element)
  {
    var id = element.GetRequiredProperty<uint>("id");
    var name = element.GetRequiredProperty<string>("name");
    var @class = element.GetOptionalProperty<string>("class").GetValueOr("");
    var opacity = element.GetOptionalProperty<float>("opacity").GetValueOr(1.0f);
    var visible = element.GetOptionalProperty<bool>("visible").GetValueOr(true);
    var tintColor = element.GetOptionalPropertyParseable<TiledColor>("tintcolor");
    var offsetX = element.GetOptionalProperty<float>("offsetx").GetValueOr(0.0f);
    var offsetY = element.GetOptionalProperty<float>("offsety").GetValueOr(0.0f);
    var parallaxX = element.GetOptionalProperty<float>("parallaxx").GetValueOr(1.0f);
    var parallaxY = element.GetOptionalProperty<float>("parallaxy").GetValueOr(1.0f);
    var properties = ResolveAndMergeProperties(@class, element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));
    var layers = element.GetOptionalPropertyCustom<List<BaseLayer>>("layers", e => e.GetValueAsList<BaseLayer>(ReadLayer)).GetValueOr([]);

    return new Group
    {
      ID = id,
      Name = name,
      Class = @class,
      Opacity = opacity,
      Visible = visible,
      TintColor = tintColor,
      OffsetX = offsetX,
      OffsetY = offsetY,
      ParallaxX = parallaxX,
      ParallaxY = parallaxY,
      Properties = properties,
      Layers = layers
    };
  }
}
