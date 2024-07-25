using System.Text;
using System.Xml.Serialization;
using DotTiled;

namespace DotTiled.Tests;

public class MapTests
{
  [Fact]
  public void ReadXml_Always_SetsRequiredAttributes()
  {
    // Arrange
    var xml =
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map 
      version="1.2" 
      class="class"
      orientation="orthogonal" 
      renderorder="right-down" 
      compressionlevel="5"
      width="10" 
      height="10" 
      tilewidth="32" 
      tileheight="32" 
      parallaxoriginx="0.5"
      parallaxoriginy="0.5"
      nextlayerid="1"
      nextobjectid="1"
      infinite="1"
    >
    </map>
    """;
    var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

    // Act
    var map = Map.LoadFromStream(xmlStream);

    // Assert
    // Assert all required properties are set
    Assert.Equal("1.2", map.Version);
    Assert.Equal("class", map.Class);
    Assert.Equal(Orientation.Orthogonal, map.Orientation);
    Assert.Equal(RenderOrder.RightDown, map.RenderOrder);
    Assert.Equal(5, map.CompressionLevel);
    Assert.Equal(10u, map.Width);
    Assert.Equal(10u, map.Height);
    Assert.Equal(32u, map.TileWidth);
    Assert.Equal(32u, map.TileHeight);
    Assert.Equal(0.5f, map.ParallaxOriginX);
    Assert.Equal(0.5f, map.ParallaxOriginY);
    Assert.Equal(1u, map.NextLayerId);
    Assert.Equal(1u, map.NextObjectId);
    Assert.True(map.Infinite);

    // Assert all optional properties are set to their default values
    Assert.Null(map.TiledVersion);
    Assert.Null(map.HexSideLength);
    Assert.Null(map.StaggerAxis);
    Assert.Null(map.StaggerIndex);
    Assert.Null(map.BackgroundColor);
  }

  public static IEnumerable<object[]> ColorData =>
    new List<object[]>
    {
      new object[] { "#ff0000", new TiledColor { R = 255, G = 0, B = 0, A = 255 } },
      new object[] { "#00ff00", new TiledColor { R = 0, G = 255, B = 0, A = 255 } },
      new object[] { "#0000ff", new TiledColor { R = 0, G = 0, B = 255, A = 255 } },
      new object[] { "#ffffff", new TiledColor { R = 255, G = 255, B = 255, A = 255 } },
      new object[] { "#000000", new TiledColor { R = 0, G = 0, B = 0, A = 255 } },
      new object[] { "#ff000000", new TiledColor { R = 0, G = 0, B = 0, A = 255 } },
      new object[] { "#fe000000", new TiledColor { R = 0, G = 0, B = 0, A = 254 } },
      new object[] { "#fe00ff00", new TiledColor { R = 0, G = 255, B = 0, A = 254 } },
    };

  [Theory]
  [MemberData(nameof(ColorData))]
  public void ReadXml_WhenPresent_SetsOptionalAttributes(string color, TiledColor expectedColor)
  {
    // Arrange
    var xml =
    $"""
    <?xml version="1.0" encoding="UTF-8"?>
    <map 
      version="1.2" 
      class="class"
      orientation="orthogonal" 
      renderorder="right-down" 
      compressionlevel="5"
      width="10" 
      height="10" 
      tilewidth="32" 
      tileheight="32" 
      hexsidelength="10"
      staggeraxis="y"
      staggerindex="odd"
      parallaxoriginx="0.5"
      parallaxoriginy="0.5"
      backgroundcolor="{color}"
      nextlayerid="1"
      nextobjectid="1"
      infinite="1"
    >
    </map>
    """;
    var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

    // Act
    var map = Map.LoadFromStream(xmlStream);

    // Assert
    // Assert all required properties are set
    Assert.Equal("1.2", map.Version);
    Assert.Equal("class", map.Class);
    Assert.Equal(Orientation.Orthogonal, map.Orientation);
    Assert.Equal(RenderOrder.RightDown, map.RenderOrder);
    Assert.Equal(5, map.CompressionLevel);
    Assert.Equal(10u, map.Width);
    Assert.Equal(10u, map.Height);
    Assert.Equal(32u, map.TileWidth);
    Assert.Equal(32u, map.TileHeight);
    Assert.Equal(10u, map.HexSideLength);
    Assert.Equal(StaggerAxis.Y, map.StaggerAxis);
    Assert.Equal(StaggerIndex.Odd, map.StaggerIndex);
    Assert.Equal(0.5f, map.ParallaxOriginX);
    Assert.Equal(0.5f, map.ParallaxOriginY);
    Assert.Equal(expectedColor, map.BackgroundColor);
    Assert.Equal(1u, map.NextLayerId);
    Assert.Equal(1u, map.NextObjectId);
    Assert.True(map.Infinite);
  }

  [Fact]
  public void ReadXml_Always_ReadsPropertiesCorrectly()
  {
    // Arrange
    var xml =
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map 
      version="1.2" 
      class="class"
      orientation="orthogonal" 
      renderorder="right-down" 
      compressionlevel="5"
      width="10" 
      height="10" 
      tilewidth="32" 
      tileheight="32" 
      parallaxoriginx="0.5"
      parallaxoriginy="0.5"
      nextlayerid="1"
      nextobjectid="1"
      infinite="1"
    >
      <properties>
        <property name="string" type="string" value="string"/>
        <property name="int" type="int" value="42"/>
        <property name="float" type="float" value="42.42"/>
        <property name="bool" type="bool" value="true"/>
        <property name="color" type="color" value="#ff0000"/>
        <property name="file" type="file" value="file"/>
        <property name="object" type="object" value="5"/>
        <property name="class" type="class" propertytype="TestClass">
          <properties>
            <property name="TestClassString" type="string" value="string"/>
            <property name="TestClassInt" type="int" value="43"/>
          </properties>
        </property>
      </properties>
    </map>
    """;
    var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

    // Act
    var map = Map.LoadFromStream(xmlStream);

    // Assert
    Assert.NotNull(map.Properties);
    Assert.Equal(8, map.Properties.Count);

    Assert.Equal(PropertyType.String, map.Properties["string"].Type);
    Assert.Equal("string", map.GetProperty<StringProperty>("string").Value);

    Assert.Equal(PropertyType.Int, map.Properties["int"].Type);
    Assert.Equal(42, map.GetProperty<IntProperty>("int").Value);

    Assert.Equal(PropertyType.Float, map.Properties["float"].Type);
    Assert.Equal(42.42f, map.GetProperty<FloatProperty>("float").Value);

    Assert.Equal(PropertyType.Bool, map.Properties["bool"].Type);
    Assert.True(map.GetProperty<BooleanProperty>("bool").Value);

    Assert.Equal(PropertyType.Color, map.Properties["color"].Type);
    Assert.Equal(new TiledColor { R = 255, G = 0, B = 0, A = 255 }, map.GetProperty<ColorProperty>("color").Value);

    Assert.Equal(PropertyType.File, map.Properties["file"].Type);
    Assert.Equal("file", map.GetProperty<FileProperty>("file").Value);

    Assert.Equal(PropertyType.Object, map.Properties["object"].Type);
    Assert.Equal(5, map.GetProperty<ObjectProperty>("object").Value);

    Assert.Equal(PropertyType.Class, map.Properties["class"].Type);
    var classProperty = map.GetProperty<ClassProperty>("class");
    Assert.Equal("TestClass", classProperty.PropertyType);
    Assert.Equal(2, classProperty.Value.Count);
    Assert.Equal("string", classProperty.GetProperty<StringProperty>("TestClassString").Value);
    Assert.Equal(43, classProperty.GetProperty<IntProperty>("TestClassInt").Value);
  }

  [Fact]
  public void ReadXml_Always_1()
  {
    // Arrange
    var xml =
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map 
      version="1.2" 
      class="class"
      orientation="orthogonal" 
      renderorder="right-down" 
      compressionlevel="5"
      width="10" 
      height="10" 
      tilewidth="32" 
      tileheight="32" 
      parallaxoriginx="0.5"
      parallaxoriginy="0.5"
      nextlayerid="1"
      nextobjectid="1"
      infinite="1"
    >
      <properties>
        <property name="string" type="string" value="string"/>
        <property name="int" type="int" value="42"/>
        <property name="float" type="float" value="42.42"/>
        <property name="bool" type="bool" value="true"/>
        <property name="color" type="color" value="#ff0000"/>
        <property name="file" type="file" value="file"/>
        <property name="object" type="object" value="5"/>
        <property name="class" type="class" propertytype="TestClass">
          <properties>
            <property name="TestClassString" type="string" value="string"/>
            <property name="TestClassInt" type="int" value="43"/>
          </properties>
        </property>
      </properties>
      <tileset firstgid="1" source="textures/tiles.tsx"/>
    </map>
    """;
    var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

    // Act
    var map = Map.LoadFromStream(xmlStream);
  }
}

