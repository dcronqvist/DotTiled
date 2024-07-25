using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DotTiled;

public static class Helpers
{
  public static void SetAtMostOnce<T>(ref T? field, T value, string fieldName)
  {
    if (field is not null)
      throw new XmlException($"{fieldName} already set");

    field = value;
  }
}

public enum Orientation
{
  [XmlEnum(Name = "orthogonal")]
  Orthogonal,

  [XmlEnum(Name = "isometric")]
  Isometric,

  [XmlEnum(Name = "staggered")]
  Staggered,

  [XmlEnum(Name = "hexagonal")]
  Hexagonal
}

public enum RenderOrder
{
  [XmlEnum(Name = "right-down")]
  RightDown,

  [XmlEnum(Name = "right-up")]
  RightUp,

  [XmlEnum(Name = "left-down")]
  LeftDown,

  [XmlEnum(Name = "left-up")]
  LeftUp
}

public enum StaggerAxis
{
  [XmlEnum(Name = "x")]
  X,

  [XmlEnum(Name = "y")]
  Y
}

public enum StaggerIndex
{
  [XmlEnum(Name = "even")]
  Even,

  [XmlEnum(Name = "odd")]
  Odd
}

public class TiledColor : IParsable<TiledColor>, IEquatable<TiledColor>
{
  public required byte R { get; set; }
  public required byte G { get; set; }
  public required byte B { get; set; }
  public byte A { get; set; } = 255;

  public static TiledColor Parse(string s, IFormatProvider? provider)
  {
    TryParse(s, provider, out var result);
    return result ?? throw new FormatException($"Invalid format for TiledColor: {s}");
  }

  public static bool TryParse(
    [NotNullWhen(true)] string? s,
    IFormatProvider? provider,
    [MaybeNullWhen(false)] out TiledColor result)
  {
    // Format: #RRGGBB or #AARRGGBB
    if (s is null || s.Length != 7 && s.Length != 9 || s[0] != '#')
    {
      result = default;
      return false;
    }

    if (s.Length == 7)
    {
      result = new TiledColor
      {
        R = byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        G = byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        B = byte.Parse(s[5..7], NumberStyles.HexNumber, provider)
      };
    }
    else
    {
      result = new TiledColor
      {
        A = byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        R = byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        G = byte.Parse(s[5..7], NumberStyles.HexNumber, provider),
        B = byte.Parse(s[7..9], NumberStyles.HexNumber, provider)
      };
    }

    return true;
  }

  public bool Equals(TiledColor? other)
  {
    if (other is null)
      return false;

    return R == other.R && G == other.G && B == other.B && A == other.A;
  }

  public override bool Equals(object? obj) => obj is TiledColor other && Equals(other);

  public override int GetHashCode() => HashCode.Combine(R, G, B, A);
}

public enum PropertyType
{
  [XmlEnum(Name = "string")]
  String,

  [XmlEnum(Name = "int")]
  Int,

  [XmlEnum(Name = "float")]
  Float,

  [XmlEnum(Name = "bool")]
  Bool,

  [XmlEnum(Name = "color")]
  Color,

  [XmlEnum(Name = "file")]
  File,

  [XmlEnum(Name = "object")]
  Object,

  [XmlEnum(Name = "class")]
  Class
}

[XmlRoot(ElementName = "property")]
public interface IProperty : IXmlSerializable
{
  public string Name { get; set; }
  public PropertyType Type { get; set; }
}

[XmlRoot(ElementName = "property")]
public class BooleanProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required bool Value { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    Value = reader.GetRequiredAttribute<bool>("value");
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "property")]
public class ColorProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required TiledColor Value { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    Value = reader.GetRequiredAttribute<TiledColor>("value");
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "property")]
public class FileProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required string Value { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    Value = reader.GetRequiredAttribute("value");
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "property")]
public class FloatProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required float Value { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    Value = reader.GetRequiredAttribute<float>("value");
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "property")]
public class IntProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required int Value { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    Value = reader.GetRequiredAttribute<int>("value");
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "property")]
public class ObjectProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required int Value { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    Value = reader.GetRequiredAttribute<int>("value");
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "property")]
public class StringProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required string Value { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    Value = reader.GetRequiredAttribute("value");
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "property")]
public class ClassProperty : IProperty
{
  public required string Name { get; set; }
  public required PropertyType Type { get; set; }
  public required string PropertyType { get; set; }
  public required Dictionary<string, IProperty> Value { get; set; }

  public T GetProperty<T>(string propertyName) where T : IProperty =>
    (T)Value[propertyName];

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    Name = reader.GetRequiredAttribute("name");
    Type = reader.GetRequiredAttributeEnum<PropertyType>("type");
    PropertyType = reader.GetRequiredAttribute("propertytype");

    // First read the start element
    reader.ReadStartElement("property");
    // Then read the properties
    Value = XmlHelpers.ReadProperties(reader);
    // Finally read the end element
    reader.ReadEndElement();
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

public enum ObjectAlignment
{
  [XmlEnum(Name = "unspecified")]
  Unspecified,

  [XmlEnum(Name = "topleft")]
  TopLeft,

  [XmlEnum(Name = "top")]
  Top,

  [XmlEnum(Name = "topright")]
  TopRight,

  [XmlEnum(Name = "left")]
  Left,

  [XmlEnum(Name = "center")]
  Center,

  [XmlEnum(Name = "right")]
  Right,

  [XmlEnum(Name = "bottomleft")]
  BottomLeft,

  [XmlEnum(Name = "bottom")]
  Bottom,

  [XmlEnum(Name = "bottomright")]
  BottomRight
}

public enum TileRenderSize
{
  [XmlEnum(Name = "tile")]
  Tile,

  [XmlEnum(Name = "grid")]
  Grid
}

public enum FillMode
{
  [XmlEnum(Name = "stretch")]
  Stretch,

  [XmlEnum(Name = "preserve-aspect-fit")]
  PreserveAspectFit
}

public enum ImageFormat
{
  [XmlEnum(Name = "png")]
  Png,

  [XmlEnum(Name = "gif")]
  Gif,

  [XmlEnum(Name = "jpg")]
  Jpg,

  [XmlEnum(Name = "bmp")]
  Bmp
}

public enum TiledDataEncoding
{
  [XmlEnum(Name = "csv")]
  Csv,

  [XmlEnum(Name = "base64")]
  Base64
}

public enum TiledDataCompression
{
  [XmlEnum(Name = "gzip")]
  GZip,

  [XmlEnum(Name = "zlib")]
  ZLib,

  [XmlEnum(Name = "zstd")]
  ZStd
}

[XmlRoot(ElementName = "data")]
public class TiledData : IXmlSerializable
{
  public TiledDataEncoding? Encoding { get; set; }
  public TiledDataCompression? Compression { get; set; }
  public required int[] Data { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    ReadXmlAttributes(reader);
    ReadXmlElements(reader);
  }

  private void ReadXmlAttributes(XmlReader reader)
  {
    Encoding = reader.GetOptionalAttributeEnum<TiledDataEncoding>("encoding");
    Compression = reader.GetOptionalAttributeEnum<TiledDataCompression>("compression");
  }

  private void ReadXmlElements(XmlReader reader)
  {
    if (Encoding is null && Compression is null)
    {
      // Plain csv
      reader.ReadStartElement("data");
      var dataAsCsvStringFromFile = reader.ReadContentAsString();
      var data = dataAsCsvStringFromFile
        .Split((char[])['\n', '\r', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();
      Data = data;
      reader.ReadEndElement();
    }

    throw new NotImplementedException();
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "image")]
public class Image : IXmlSerializable
{
  public ImageFormat? Format { get; set; }
  public string? ID { get; set; } = null; // Deprecated and unsupported
  public string? Source { get; set; }
  public TiledColor? TransparentColor { get; set; }
  public uint? Width { get; set; }
  public uint? Height { get; set; }

  private TiledData? _data = null;
  public TiledData? Data
  {
    get => _data;
    set => _data = value;
  }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    ReadXmlAttributes(reader);
    ReadXmlElements(reader);
  }

  private void ReadXmlAttributes(XmlReader reader)
  {
    Format = reader.GetOptionalAttributeEnum<ImageFormat>("format");
    ID = reader.GetOptionalAttribute("id");
    Source = reader.GetOptionalAttribute("source");
    TransparentColor = reader.GetOptionalAttributeClass<TiledColor>("trans");
    Width = reader.GetOptionalAttribute<uint>("width");
    Height = reader.GetOptionalAttribute<uint>("height");
  }

  private void ReadXmlElements(XmlReader reader)
  {
    reader.ReadStartElement("image");

    while (reader.IsStartElement())
    {
      var name = reader.Name;
      Action action = name switch
      {
        "data" => () => Helpers.SetAtMostOnce(ref _data, reader.ReadElementAs<TiledData>(), "Data"),
        _ => reader.Skip
      };

      action();

      if (reader.NodeType == XmlNodeType.EndElement)
        return;
    }
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

public abstract class BaseTileset : IXmlSerializable
{
  public required string? FirstGID { get; set; } // Not set in tsx
  public required string? Source { get; set; } // Not set in tsx
  public required string Name { get; set; }
  public required string Class { get; set; }
  public required uint TileWidth { get; set; }
  public required uint TileHeight { get; set; }
  public required uint? Spacing { get; set; }
  public required uint? Margin { get; set; }
  public required uint TileCount { get; set; }
  public required uint Columns { get; set; }
  public required ObjectAlignment ObjectAlignment { get; set; }
  public required TileRenderSize TileRenderSize { get; set; }
  public required FillMode FillMode { get; set; }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    ReadXmlAttributes(reader);
    ReadXmlElements(reader);
  }

  private void ReadXmlAttributes(XmlReader reader)
  {
    FirstGID = reader.GetOptionalAttribute("firstgid");
    Source = reader.GetOptionalAttribute("source");
    Name = reader.GetRequiredAttribute("name");
    Class = reader.GetOptionalAttribute("class") ?? ""; // default value
    TileWidth = reader.GetRequiredAttribute<uint>("tilewidth");
    TileHeight = reader.GetRequiredAttribute<uint>("tileheight");
    Spacing = reader.GetOptionalAttribute<uint>("spacing");
    Margin = reader.GetOptionalAttribute<uint>("margin");
    TileCount = reader.GetRequiredAttribute<uint>("tilecount");
    Columns = reader.GetRequiredAttribute<uint>("columns");
    ObjectAlignment = reader.GetOptionalAttributeEnum<ObjectAlignment>("objectalignment") ?? ObjectAlignment.Unspecified;
    TileRenderSize = reader.GetOptionalAttributeEnum<TileRenderSize>("tilerendersize") ?? TileRenderSize.Tile;
    FillMode = reader.GetOptionalAttributeEnum<FillMode>("fillmode") ?? FillMode.Stretch;
  }

  protected abstract void ReadXmlElements(XmlReader reader);

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "tileset")]
public class ImageTileset : BaseTileset
{
  private Image? _image = null;
  public required Image Image
  {
    get => _image ?? throw new InvalidOperationException("Image not set"); // Should not be able to happen
    set => _image = value;
  }

  protected override void ReadXmlElements(XmlReader reader)
  {
    // Different types of tilesets
    reader.ReadStartElement("tileset");

    while (reader.IsStartElement())
    {
      var name = reader.Name;
      Action action = name switch
      {
        "image" => () => Helpers.SetAtMostOnce(ref _image, reader.ReadElementAs<Image>(), "Image"),
        "tileoffset" => reader.Skip,
        "tile" => reader.Skip,
        "terraintypes" => reader.Skip,
        "wangsets" => reader.Skip,
        _ => reader.Skip
      };

      action();

      if (reader.NodeType == XmlNodeType.EndElement)
        return;
    }
  }
}

[XmlRoot(ElementName = "layer")]
public class Layer : IXmlSerializable
{
  public required string ID { get; set; }
  public required string Name { get; set; }
  public required string Class { get; set; }
  public required uint X { get; set; }
  public required uint Y { get; set; }
  public required uint Width { get; set; }
  public required uint Height { get; set; }
  public required float Opacity { get; set; }
  public required bool Visible { get; set; }
  public required TiledColor? TintColor { get; set; }
  public required float OffsetX { get; set; }
  public required float OffsetY { get; set; }
  public required float ParallaxX { get; set; }
  public required float ParallaxY { get; set; }

  private Dictionary<string, IProperty>? _properties = null;
  public required Dictionary<string, IProperty>? Properties
  {
    get => _properties;
    set => _properties = value;
  }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    ReadXmlAttributes(reader);
    ReadXmlElements(reader);
  }

  private void ReadXmlAttributes(XmlReader reader)
  {
    ID = reader.GetRequiredAttribute("id");
    Name = reader.GetRequiredAttribute("name");
    Class = reader.GetOptionalAttribute("class") ?? ""; // default value
    X = reader.GetRequiredAttribute<uint>("x");
    Y = reader.GetRequiredAttribute<uint>("y");
    Width = reader.GetRequiredAttribute<uint>("width");
    Height = reader.GetRequiredAttribute<uint>("height");
    Opacity = reader.GetRequiredAttribute<float>("opacity");
    Visible = reader.GetRequiredAttribute<uint>("visible") == 1;
    TintColor = reader.GetOptionalAttributeClass<TiledColor>("tintcolor");
    OffsetX = reader.GetRequiredAttribute<float>("offsetx");
    OffsetY = reader.GetRequiredAttribute<float>("offsety");
    ParallaxX = reader.GetRequiredAttribute<float>("parallaxx");
    ParallaxY = reader.GetRequiredAttribute<float>("parallaxy");
  }

  private void ReadXmlElements(XmlReader reader)
  {
    reader.ReadStartElement("layer");

    while (reader.IsStartElement())
    {
      var name = reader.Name;
      Action action = name switch
      {
        "properties" => () => Helpers.SetAtMostOnce(ref _properties, XmlHelpers.ReadProperties(reader), "Properties"),
        "data" => reader.Skip,
        _ => reader.Skip
      };

      action();
    }

    reader.ReadEndElement();
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}

[XmlRoot(ElementName = "map")]
public class Map : IXmlSerializable
{
  public required string Version { get; set; }
  public string? TiledVersion { get; set; }
  public required string Class { get; set; }
  public required Orientation Orientation { get; set; }
  public required RenderOrder RenderOrder { get; set; }
  public required int CompressionLevel { get; set; }
  public required uint Width { get; set; }
  public required uint Height { get; set; }
  public required uint TileWidth { get; set; }
  public required uint TileHeight { get; set; }
  public uint? HexSideLength { get; set; }
  public StaggerAxis? StaggerAxis { get; set; }
  public StaggerIndex? StaggerIndex { get; set; }
  public required float ParallaxOriginX { get; set; }
  public required float ParallaxOriginY { get; set; }
  public TiledColor? BackgroundColor { get; set; }
  public required uint NextLayerId { get; set; }
  public required uint NextObjectId { get; set; }
  public required bool Infinite { get; set; }

  private Dictionary<string, IProperty>? _properties = null;
  public required Dictionary<string, IProperty>? Properties
  {
    get => _properties;
    set => _properties = value;
  }

  public required List<BaseTileset> Tilesets { get; set; } = [];

  public T GetProperty<T>(string propertyName) where T : IProperty
  {
    if (Properties is null)
      throw new InvalidOperationException("Properties not set");

    return (T)Properties[propertyName];
  }

  public static Map LoadFromStream(Stream stream)
  {
    using var reader = new StreamReader(stream, Encoding.UTF8);
    var serializer = new XmlSerializer(typeof(Map));
    return (Map)serializer.Deserialize(reader)!;
  }

  public XmlSchema? GetSchema() => null;

  public void ReadXml(XmlReader reader)
  {
    ReadXmlAttributes(reader);
    ReadXmlElements(reader, (s) => null);
  }

  private void ReadXmlAttributes(XmlReader reader)
  {
    Version = reader.GetRequiredAttribute("version");
    TiledVersion = reader.GetOptionalAttribute("tiledversion");
    Class = reader.GetOptionalAttribute("class") ?? ""; // default value
    Orientation = reader.GetRequiredAttributeEnum<Orientation>("orientation");
    RenderOrder = reader.GetRequiredAttributeEnum<RenderOrder>("renderorder");
    CompressionLevel = reader.GetRequiredAttribute<int>("compressionlevel");
    Width = reader.GetRequiredAttribute<uint>("width");
    Height = reader.GetRequiredAttribute<uint>("height");
    TileWidth = reader.GetRequiredAttribute<uint>("tilewidth");
    TileHeight = reader.GetRequiredAttribute<uint>("tileheight");
    HexSideLength = reader.GetOptionalAttribute<uint>("hexsidelength");
    StaggerAxis = reader.GetOptionalAttributeEnum<StaggerAxis>("staggeraxis");
    StaggerIndex = reader.GetOptionalAttributeEnum<StaggerIndex>("staggerindex");
    ParallaxOriginX = reader.GetRequiredAttribute<float>("parallaxoriginx");
    ParallaxOriginY = reader.GetRequiredAttribute<float>("parallaxoriginy");
    BackgroundColor = reader.GetOptionalAttributeClass<TiledColor>("backgroundcolor");
    NextLayerId = reader.GetRequiredAttribute<uint>("nextlayerid");
    NextObjectId = reader.GetRequiredAttribute<uint>("nextobjectid");
    Infinite = reader.GetRequiredAttribute<uint>("infinite") == 1;
  }

  private void ReadXmlElements(XmlReader reader, Func<string, BaseTileset> tilesetResolver)
  {
    reader.ReadStartElement("map");

    while (reader.IsStartElement())
    {
      var name = reader.Name;
      Action action = name switch
      {
        "properties" => () => Helpers.SetAtMostOnce(ref _properties, XmlHelpers.ReadProperties(reader), "Properties"),
        "editorsettings" => reader.Skip,
        "tileset" => () => Tilesets.Add(XmlHelpers.ReadTileset(reader, tilesetResolver)),
        _ => reader.Skip
      };

      action();
    }

    reader.ReadEndElement();
  }

  public void WriteXml(XmlWriter writer)
  {
    throw new NotImplementedException();
  }
}
