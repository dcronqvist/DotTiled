using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DotTiled;

public partial class TmxSerializer
{
  private TileLayer ReadTileLayer(XmlReader reader, bool dataUsesChunks)
  {
    var id = reader.GetRequiredAttributeParseable<uint>("id");
    var name = reader.GetOptionalAttribute("name") ?? "";
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var x = reader.GetOptionalAttributeParseable<uint>("x") ?? 0;
    var y = reader.GetOptionalAttributeParseable<uint>("y") ?? 0;
    var width = reader.GetRequiredAttributeParseable<uint>("width");
    var height = reader.GetRequiredAttributeParseable<uint>("height");
    var opacity = reader.GetOptionalAttributeParseable<float>("opacity") ?? 1.0f;
    var visible = reader.GetOptionalAttributeParseable<bool>("visible") ?? true;
    var tintColor = reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = reader.GetOptionalAttributeParseable<float>("offsetx") ?? 0.0f;
    var offsetY = reader.GetOptionalAttributeParseable<float>("offsety") ?? 0.0f;
    var parallaxX = reader.GetOptionalAttributeParseable<float>("parallaxx") ?? 1.0f;
    var parallaxY = reader.GetOptionalAttributeParseable<float>("parallaxy") ?? 1.0f;

    Dictionary<string, IProperty>? properties = null;
    Data? data = null;

    reader.ProcessChildren("layer", (r, elementName) => elementName switch
    {
      "data" => () => Helpers.SetAtMostOnce(ref data, ReadData(r, dataUsesChunks), "Data"),
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r), "Properties"),
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
      Properties = properties
    };
  }

  private ImageLayer ReadImageLayer(XmlReader reader)
  {
    var id = reader.GetRequiredAttributeParseable<uint>("id");
    var name = reader.GetOptionalAttribute("name") ?? "";
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var x = reader.GetOptionalAttributeParseable<uint>("x") ?? 0;
    var y = reader.GetOptionalAttributeParseable<uint>("y") ?? 0;
    var opacity = reader.GetOptionalAttributeParseable<float>("opacity") ?? 1.0f;
    var visible = reader.GetOptionalAttributeParseable<bool>("visible") ?? true;
    var tintColor = reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = reader.GetOptionalAttributeParseable<float>("offsetx") ?? 0.0f;
    var offsetY = reader.GetOptionalAttributeParseable<float>("offsety") ?? 0.0f;
    var parallaxX = reader.GetOptionalAttributeParseable<float>("parallaxx") ?? 1.0f;
    var parallaxY = reader.GetOptionalAttributeParseable<float>("parallaxy") ?? 1.0f;
    var repeatX = reader.GetRequiredAttributeParseable<bool>("repeatx");
    var repeatY = reader.GetRequiredAttributeParseable<bool>("repeaty");

    Dictionary<string, IProperty>? properties = null;
    Image? image = null;

    reader.ProcessChildren("imagelayer", (r, elementName) => elementName switch
    {
      "image" => () => Helpers.SetAtMostOnce(ref image, ReadImage(r), "Image"),
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r), "Properties"),
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
      Properties = properties,
      Image = image,
      RepeatX = repeatX,
      RepeatY = repeatY
    };
  }

  private Group ReadGroup(XmlReader reader)
  {
    var id = reader.GetRequiredAttributeParseable<uint>("id");
    var name = reader.GetOptionalAttribute("name") ?? "";
    var @class = reader.GetOptionalAttribute("class") ?? "";
    var opacity = reader.GetOptionalAttributeParseable<float>("opacity") ?? 1.0f;
    var visible = reader.GetOptionalAttributeParseable<bool>("visible") ?? true;
    var tintColor = reader.GetOptionalAttributeClass<Color>("tintcolor");
    var offsetX = reader.GetOptionalAttributeParseable<float>("offsetx") ?? 0.0f;
    var offsetY = reader.GetOptionalAttributeParseable<float>("offsety") ?? 0.0f;
    var parallaxX = reader.GetOptionalAttributeParseable<float>("parallaxx") ?? 1.0f;
    var parallaxY = reader.GetOptionalAttributeParseable<float>("parallaxy") ?? 1.0f;

    Dictionary<string, IProperty>? properties = null;
    List<BaseLayer> layers = [];

    reader.ProcessChildren("group", (r, elementName) => elementName switch
    {
      "properties" => () => Helpers.SetAtMostOnce(ref properties, ReadProperties(r), "Properties"),
      "layer" => () => layers.Add(ReadTileLayer(r, dataUsesChunks: false)),
      "objectgroup" => () => layers.Add(ReadObjectLayer(r)),
      "imagelayer" => () => layers.Add(ReadImageLayer(r)),
      "group" => () => layers.Add(ReadGroup(r)),
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
      Properties = properties,
      Layers = layers
    };
  }
}
