using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using DotTiled.Model;

namespace DotTiled.Serialization.Tmj;

internal partial class Tmj
{
  internal static TileLayer ReadTileLayer(
    JsonElement element,
    IReadOnlyCollection<ICustomTypeDefinition> customTypeDefinitions)
  {
    var compression = element.GetOptionalPropertyParseable<DataCompression?>("compression", s => s switch
    {
      "zlib" => DataCompression.ZLib,
      "gzip" => DataCompression.GZip,
      "" => null,
      _ => throw new JsonException($"Unsupported compression '{s}'.")
    }, null);
    var encoding = element.GetOptionalPropertyParseable<DataEncoding>("encoding", s => s switch
    {
      "csv" => DataEncoding.Csv,
      "base64" => DataEncoding.Base64,
      _ => throw new JsonException($"Unsupported encoding '{s}'.")
    }, DataEncoding.Csv);
    var chunks = element.GetOptionalPropertyCustom<Data?>("chunks", e => ReadDataAsChunks(e, compression, encoding), null);
    var @class = element.GetOptionalProperty<string>("class", "");
    var data = element.GetOptionalPropertyCustom<Data?>("data", e => ReadDataWithoutChunks(e, compression, encoding), null);
    var height = element.GetRequiredProperty<uint>("height");
    var id = element.GetRequiredProperty<uint>("id");
    var name = element.GetRequiredProperty<string>("name");
    var offsetX = element.GetOptionalProperty<float>("offsetx", 0.0f);
    var offsetY = element.GetOptionalProperty<float>("offsety", 0.0f);
    var opacity = element.GetOptionalProperty<float>("opacity", 1.0f);
    var parallaxx = element.GetOptionalProperty<float>("parallaxx", 1.0f);
    var parallaxy = element.GetOptionalProperty<float>("parallaxy", 1.0f);
    var properties = element.GetOptionalPropertyCustom<Dictionary<string, IProperty>?>("properties", e => ReadProperties(e, customTypeDefinitions), null);
    var repeatX = element.GetOptionalProperty<bool>("repeatx", false);
    var repeatY = element.GetOptionalProperty<bool>("repeaty", false);
    var startX = element.GetOptionalProperty<int>("startx", 0);
    var startY = element.GetOptionalProperty<int>("starty", 0);
    var tintColor = element.GetOptionalPropertyParseable<Color?>("tintcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var transparentColor = element.GetOptionalPropertyParseable<Color?>("transparentcolor", s => Color.Parse(s, CultureInfo.InvariantCulture), null);
    var visible = element.GetOptionalProperty<bool>("visible", true);
    var width = element.GetRequiredProperty<uint>("width");
    var x = element.GetRequiredProperty<uint>("x");
    var y = element.GetRequiredProperty<uint>("y");

    if ((data ?? chunks) is null)
      throw new JsonException("Tile layer does not contain data.");

    return new TileLayer
    {
      ID = id,
      Name = name,
      Class = @class,
      Opacity = opacity,
      Visible = visible,
      TintColor = tintColor,
      OffsetX = offsetX,
      OffsetY = offsetY,
      ParallaxX = parallaxx,
      ParallaxY = parallaxy,
      Properties = properties,
      X = x,
      Y = y,
      Width = width,
      Height = height,
      Data = data ?? chunks
    };
  }
}
