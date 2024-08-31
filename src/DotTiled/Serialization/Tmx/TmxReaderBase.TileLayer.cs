using System.Collections.Generic;
using System.Linq;

namespace DotTiled.Serialization.Tmx;

public abstract partial class TmxReaderBase
{
  internal TileLayer ReadTileLayer(bool dataUsesChunks)
  {
    var id = _reader.GetRequiredAttributeParseable<uint>("id");
    var name = _reader.GetOptionalAttribute("name") ?? "";
    var @class = _reader.GetOptionalAttribute("class") ?? "";
    var x = _reader.GetOptionalAttributeParseable<uint>("x") ?? 0;
    var y = _reader.GetOptionalAttributeParseable<uint>("y") ?? 0;
    var width = _reader.GetRequiredAttributeParseable<uint>("width");
    var height = _reader.GetRequiredAttributeParseable<uint>("height");
    var opacity = _reader.GetOptionalAttributeParseable<float>("opacity") ?? 1.0f;
    var visible = (_reader.GetOptionalAttributeParseable<uint>("visible") ?? 1) == 1;
    var tintColor = _reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = _reader.GetOptionalAttributeParseable<float>("offsetx") ?? 0.0f;
    var offsetY = _reader.GetOptionalAttributeParseable<float>("offsety") ?? 0.0f;
    var parallaxX = _reader.GetOptionalAttributeParseable<float>("parallaxx") ?? 1.0f;
    var parallaxY = _reader.GetOptionalAttributeParseable<float>("parallaxy") ?? 1.0f;

    List<IProperty>? properties = null;
    Data? data = null;

    _reader.ProcessChildren("layer", (r, elementName) => elementName switch
    {
      "data" => () => Helpers.SetAtMostOnce(ref data, ReadData(dataUsesChunks), "Data"),
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      _ => r.Skip
    });

    return new TileLayer
    {
      ID = id,
      Name = name,
      Class = @class,
      X = x,
      Y = y,
      Width = width,
      Height = height,
      Opacity = opacity,
      Visible = visible,
      TintColor = tintColor,
      OffsetX = offsetX,
      OffsetY = offsetY,
      ParallaxX = parallaxX,
      ParallaxY = parallaxY,
      Data = data,
      Properties = properties ?? []
    };
  }

  internal ImageLayer ReadImageLayer()
  {
    var id = _reader.GetRequiredAttributeParseable<uint>("id");
    var name = _reader.GetOptionalAttribute("name") ?? "";
    var @class = _reader.GetOptionalAttribute("class") ?? "";
    var x = _reader.GetOptionalAttributeParseable<uint>("x") ?? 0;
    var y = _reader.GetOptionalAttributeParseable<uint>("y") ?? 0;
    var opacity = _reader.GetOptionalAttributeParseable<float>("opacity") ?? 1.0f;
    var visible = _reader.GetOptionalAttributeParseable<bool>("visible") ?? true;
    var tintColor = _reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = _reader.GetOptionalAttributeParseable<float>("offsetx") ?? 0.0f;
    var offsetY = _reader.GetOptionalAttributeParseable<float>("offsety") ?? 0.0f;
    var parallaxX = _reader.GetOptionalAttributeParseable<float>("parallaxx") ?? 1.0f;
    var parallaxY = _reader.GetOptionalAttributeParseable<float>("parallaxy") ?? 1.0f;
    var repeatX = (_reader.GetOptionalAttributeParseable<uint>("repeatx") ?? 0) == 1;
    var repeatY = (_reader.GetOptionalAttributeParseable<uint>("repeaty") ?? 0) == 1;

    List<IProperty>? properties = null;
    Image? image = null;

    _reader.ProcessChildren("imagelayer", (r, elementName) => elementName switch
    {
      "image" => () => Helpers.SetAtMostOnce(ref image, ReadImage(), "Image"),
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      _ => r.Skip
    });

    return new ImageLayer
    {
      ID = id,
      Name = name,
      Class = @class,
      X = x,
      Y = y,
      Opacity = opacity,
      Visible = visible,
      TintColor = tintColor,
      OffsetX = offsetX,
      OffsetY = offsetY,
      ParallaxX = parallaxX,
      ParallaxY = parallaxY,
      Properties = properties ?? [],
      Image = image,
      RepeatX = repeatX,
      RepeatY = repeatY
    };
  }

  internal Group ReadGroup()
  {
    var id = _reader.GetRequiredAttributeParseable<uint>("id");
    var name = _reader.GetOptionalAttribute("name") ?? "";
    var @class = _reader.GetOptionalAttribute("class") ?? "";
    var opacity = _reader.GetOptionalAttributeParseable<float>("opacity") ?? 1.0f;
    var visible = (_reader.GetOptionalAttributeParseable<uint>("visible") ?? 1) == 1;
    var tintColor = _reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = _reader.GetOptionalAttributeParseable<float>("offsetx") ?? 0.0f;
    var offsetY = _reader.GetOptionalAttributeParseable<float>("offsety") ?? 0.0f;
    var parallaxX = _reader.GetOptionalAttributeParseable<float>("parallaxx") ?? 1.0f;
    var parallaxY = _reader.GetOptionalAttributeParseable<float>("parallaxy") ?? 1.0f;

    List<IProperty>? properties = null;
    List<BaseLayer> layers = [];

    _reader.ProcessChildren("group", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(), "Properties"),
      "layer" => () => layers.Add(ReadTileLayer(false)),
      "objectgroup" => () => layers.Add(ReadObjectLayer()),
      "imagelayer" => () => layers.Add(ReadImageLayer()),
      "group" => () => layers.Add(ReadGroup()),
      _ => r.Skip
    });

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
      Properties = properties ?? [],
      Layers = layers
    };
  }
}
