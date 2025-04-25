using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal ImageLayer ReadImageLayer(JsonElement element)
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

    var image = element.GetRequiredProperty<string>("image");
    var repeatX = element.GetOptionalProperty<bool>("repeatx").GetValueOr(false);
    var repeatY = element.GetOptionalProperty<bool>("repeaty").GetValueOr(false);
    var transparentColor = element.GetOptionalPropertyParseable<TiledColor>("transparentcolor");
    var x = element.GetOptionalProperty<int>("x").GetValueOr(0);
    var y = element.GetOptionalProperty<int>("y").GetValueOr(0);

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
