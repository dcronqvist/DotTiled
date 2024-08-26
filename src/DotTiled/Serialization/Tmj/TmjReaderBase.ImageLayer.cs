using System.Globalization;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal ImageLayer ReadImageLayer(JsonElement element)
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
    var properties = element.GetOptionalPropertyCustom("properties", ReadProperties, []);

    var image = element.GetRequiredProperty<string>("image");
    var repeatX = element.GetOptionalProperty<bool>("repeatx", false);
    var repeatY = element.GetOptionalProperty<bool>("repeaty", false);
    var transparentColor = element.GetOptionalPropertyParseable<Color?>("transparentcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var x = element.GetOptionalProperty<uint>("x", 0);
    var y = element.GetOptionalProperty<uint>("y", 0);

    var imgModel = new Image
    {
      Format = Helpers.ParseImageFormatFromSource(image),
      Height = 0,
      Width = 0,
      Source = image,
      TransparentColor = transparentColor
    };

    return new ImageLayer
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
      Image = imgModel,
      RepeatX = repeatX,
      RepeatY = repeatY,
      X = x,
      Y = y
    };
  }
}
