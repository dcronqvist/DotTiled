using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal static Group ReadGroup(
    JsonElement element,
    Func<string, Template> externalTemplateResolver,
    IReadOnlyCollection<CustomTypeDefinition> customTypeDefinitions)
  {
    var id = element.GetRequiredProperty<uint>("id");
    var name = element.GetRequiredProperty<string>("name");
    var @class = element.GetOptionalProperty<string>("class", "");
    var opacity = element.GetOptionalProperty<float>("opacity", 1.0f);
    var visible = element.GetOptionalProperty<bool>("visible", true);
    var tintColor = element.GetOptionalPropertyParseable<Color?>("tintcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var offsetX = element.GetOptionalProperty<float>("offsetx", 0.0f);
    var offsetY = element.GetOptionalProperty<float>("offsety", 0.0f);
    var parallaxX = element.GetOptionalProperty<float>("parallaxx", 1.0f);
    var parallaxY = element.GetOptionalProperty<float>("parallaxy", 1.0f);
    var properties = element.GetOptionalPropertyCustom<Dictionary<string, IProperty>?>("properties", e => ReadProperties(e, customTypeDefinitions), null);
    var layers = element.GetOptionalPropertyCustom<List<BaseLayer>>("layers", e => e.GetValueAsList<BaseLayer>(el => ReadLayer(el, externalTemplateResolver, customTypeDefinitions)), []);

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
