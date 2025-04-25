using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

public abstract partial class TmjReaderBase
{
  internal TileLayer ReadTileLayer(JsonElement element)
  {
    var encoding = element.GetOptionalPropertyParseable<DataEncoding>("encoding", s => s switch
    {
      "csv" => DataEncoding.Csv,
      "base64" => DataEncoding.Base64,
      _ => throw new JsonException($"Unsupported encoding '{s}'.")
    }).GetValueOr(DataEncoding.Csv);
    var compression = element.GetOptionalPropertyParseable<DataCompression>("compression", s => s switch
    {
      "zlib" => DataCompression.ZLib,
      "gzip" => DataCompression.GZip,
      "" => Optional<DataCompression>.Empty,
      _ => throw new JsonException($"Unsupported compression '{s}'.")
    });
    var chunks = element.GetOptionalPropertyCustom<Data>("chunks", e => ReadDataAsChunks(e, compression, encoding));
    var @class = element.GetOptionalProperty<string>("class").GetValueOr("");
    var data = element.GetOptionalPropertyCustom<Data>("data", e => ReadDataWithoutChunks(e, compression, encoding));
    var height = element.GetRequiredProperty<int>("height");
    var id = element.GetRequiredProperty<uint>("id");
    var name = element.GetRequiredProperty<string>("name");
    var offsetX = element.GetOptionalProperty<float>("offsetx").GetValueOr(0.0f);
    var offsetY = element.GetOptionalProperty<float>("offsety").GetValueOr(0.0f);
    var opacity = element.GetOptionalProperty<float>("opacity").GetValueOr(1.0f);
    var parallaxx = element.GetOptionalProperty<float>("parallaxx").GetValueOr(1.0f);
    var parallaxy = element.GetOptionalProperty<float>("parallaxy").GetValueOr(1.0f);
    var properties = ResolveAndMergeProperties(@class, element.GetOptionalPropertyCustom("properties", ReadProperties).GetValueOr([]));
    var repeatX = element.GetOptionalProperty<bool>("repeatx").GetValueOr(false);
    var repeatY = element.GetOptionalProperty<bool>("repeaty").GetValueOr(false);
    var startX = element.GetOptionalProperty<int>("startx").GetValueOr(0);
    var startY = element.GetOptionalProperty<int>("starty").GetValueOr(0);
    var tintColor = element.GetOptionalPropertyParseable<TiledColor>("tintcolor");
    var transparentColor = element.GetOptionalPropertyParseable<TiledColor>("transparentcolor");
    var visible = element.GetOptionalProperty<bool>("visible").GetValueOr(true);
    var width = element.GetRequiredProperty<int>("width");
    var x = element.GetRequiredProperty<int>("x");
    var y = element.GetRequiredProperty<int>("y");

    if (!data.HasValue && !chunks.HasValue)
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
