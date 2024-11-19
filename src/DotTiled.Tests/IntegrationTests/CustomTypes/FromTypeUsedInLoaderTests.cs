using DotTiled.Serialization;
using NSubstitute;

namespace DotTiled.Tests;

public class FromTypeUsedInLoaderTests
{
  private enum TestEnum
  {
    A,
    B,
    C
  }
  [Flags]
  private enum TestFlags
  {
    A = 0b001,
    B = 0b010,
    C = 0b100
  }
  private sealed class TestClass
  {
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 42;
    public TestEnum Enum { get; set; } = TestEnum.A;
    public TestFlags Flags { get; set; } = TestFlags.B | TestFlags.C;
  }

  [Fact]
  public void LoadMap_MapHasClassAndClassIsDefined_ReturnsCorrectMap()
  {
    // Arrange
    var resourceReader = Substitute.For<IResourceReader>();
    resourceReader.Read("map.tmx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map version="1.10" tiledversion="1.11.0" class="TestClass" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="2" nextobjectid="1">
      <layer id="1" name="Tile Layer 1" width="5" height="5">
        <data encoding="csv">
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0
        </data>
      </layer>
    </map>
    """);
    var classDefinition = CustomClassDefinition.FromClass<TestClass>();
    var loader = Loader.DefaultWith(
      resourceReader: resourceReader,
      customTypeDefinitions: [classDefinition]);
    var expectedMap = new Map
    {
      Class = "TestClass",
      Orientation = MapOrientation.Orthogonal,
      Width = 5,
      Height = 5,
      TileWidth = 32,
      TileHeight = 32,
      Infinite = false,
      ParallaxOriginX = 0,
      ParallaxOriginY = 0,
      RenderOrder = RenderOrder.RightDown,
      CompressionLevel = -1,
      BackgroundColor = new Color { R = 0, G = 0, B = 0, A = 0 },
      Version = "1.10",
      TiledVersion = "1.11.0",
      NextLayerID = 2,
      NextObjectID = 1,
      Layers = [
        new TileLayer
        {
          ID = 1,
          Name = "Tile Layer 1",
          Width = 5,
          Height = 5,
          Data = new Data
          {
            Encoding = DataEncoding.Csv,
            GlobalTileIDs = new Optional<uint[]>([
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0
            ]),
            FlippingFlags = new Optional<FlippingFlags[]>([
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
            ])
          }
        }
      ],
      Properties = [
        new StringProperty { Name = "Name", Value = "John Doe" },
        new IntProperty { Name = "Age", Value = 42 },
        new EnumProperty { Name = "Enum", PropertyType = "TestEnum", Value = new HashSet<string> { "A" } },
        new EnumProperty { Name = "Flags", PropertyType = "TestFlags", Value = new HashSet<string> { "B", "C" } }
      ]
    };

    // Act
    var result = loader.LoadMap("map.tmx");

    // Assert
    DotTiledAssert.AssertMap(expectedMap, result);
  }

  [Fact]
  public void LoadMap_MapHasClassAndClassIsDefinedAndFieldIsOverridenFromDefault_ReturnsCorrectMap()
  {
    // Arrange
    var resourceReader = Substitute.For<IResourceReader>();
    resourceReader.Read("map.tmx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map version="1.10" tiledversion="1.11.0" class="TestClass" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="2" nextobjectid="1">
      <properties>
        <property name="Name" value="John Doe"/>
      </properties>
      <layer id="1" name="Tile Layer 1" width="5" height="5">
        <data encoding="csv">
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0
        </data>
      </layer>
    </map>
    """);
    var classDefinition = CustomClassDefinition.FromClass<TestClass>();
    var loader = Loader.DefaultWith(
      resourceReader: resourceReader,
      customTypeDefinitions: [classDefinition]);
    var expectedMap = new Map
    {
      Class = "TestClass",
      Orientation = MapOrientation.Orthogonal,
      Width = 5,
      Height = 5,
      TileWidth = 32,
      TileHeight = 32,
      Infinite = false,
      ParallaxOriginX = 0,
      ParallaxOriginY = 0,
      RenderOrder = RenderOrder.RightDown,
      CompressionLevel = -1,
      BackgroundColor = new Color { R = 0, G = 0, B = 0, A = 0 },
      Version = "1.10",
      TiledVersion = "1.11.0",
      NextLayerID = 2,
      NextObjectID = 1,
      Layers = [
        new TileLayer
        {
          ID = 1,
          Name = "Tile Layer 1",
          Width = 5,
          Height = 5,
          Data = new Data
          {
            Encoding = DataEncoding.Csv,
            GlobalTileIDs = new Optional<uint[]>([
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0,
              0, 0, 0, 0, 0
            ]),
            FlippingFlags = new Optional<FlippingFlags[]>([
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None,
              FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None, FlippingFlags.None
            ])
          }
        }
      ],
      Properties = [
        new StringProperty { Name = "Name", Value = "John Doe" },
        new IntProperty { Name = "Age", Value = 42 },
        new EnumProperty { Name = "Enum", PropertyType = "TestEnum", Value = new HashSet<string> { "A" } },
        new EnumProperty { Name = "Flags", PropertyType = "TestFlags", Value = new HashSet<string> { "B", "C" } }
      ]
    };

    // Act
    var result = loader.LoadMap("map.tmx");

    // Assert
    DotTiledAssert.AssertMap(expectedMap, result);
  }
}
