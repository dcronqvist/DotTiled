using System.Numerics;
using DotTiled.Serialization;
using NSubstitute;

namespace DotTiled.Tests;

public class LoaderTests
{
  [Fact]
  public void LoadMap_Always_ReadsFromResourceReader()
  {
    // Arrange
    var resourceReader = Substitute.For<IResourceReader>();
    resourceReader.Read("map.tmx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map version="1.10" tiledversion="1.11.0" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="2" nextobjectid="1">
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

    var resourceCache = Substitute.For<IResourceCache>();
    var customTypeDefinitions = Enumerable.Empty<ICustomTypeDefinition>();
    var loader = new Loader(resourceReader, resourceCache, customTypeDefinitions);

    // Act
    loader.LoadMap("map.tmx");

    // Assert
    resourceReader.Received(1).Read("map.tmx");
  }

  [Fact]
  public void LoadMap_MapReferencesExternalTileset_ReadsTilesetFromResourceReaderAndAttemptsToRetrieveFromCache()
  {
    // Arrange
    var resourceReader = Substitute.For<IResourceReader>();
    resourceReader.Read("map.tmx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map version="1.10" tiledversion="1.11.0" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="2" nextobjectid="1">
      <tileset firstgid="1" source="tileset.tsx"/>
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

    resourceReader.Read("tileset.tsx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <tileset version="1.2" tiledversion="1.11.0" name="Tileset" tilewidth="32" tileheight="32" tilecount="1" columns="1">
      <tile id="1">
        <image width="32" height="32" source="tile.png"/>
      </tile>
    </tileset>
    """);

    var resourceCache = Substitute.For<IResourceCache>();
    resourceCache.GetTileset(Arg.Any<string>()).Returns(Optional<Tileset>.Empty);
    resourceCache.GetTemplate(Arg.Any<string>()).Returns(Optional<Template>.Empty);

    var customTypeDefinitions = Enumerable.Empty<ICustomTypeDefinition>();
    var loader = new Loader(resourceReader, resourceCache, customTypeDefinitions);

    // Act
    loader.LoadMap("map.tmx");

    // Assert
    resourceReader.Received(1).Read("tileset.tsx");
    resourceCache.Received(1).GetTileset("tileset.tsx");
  }

  [Fact]
  public void LoadMap_MapReferencesExternalTemplate_ReadsTemplateFromResourceReaderAndAttemptsToRetrieveFromCache()
  {
    // Arrange
    var resourceReader = Substitute.For<IResourceReader>();
    resourceReader.Read("map.tmx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map version="1.10" tiledversion="1.11.0" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="2" nextobjectid="1">
      <tileset firstgid="1" source="tileset.tsx"/>
      <layer id="1" name="Tile Layer 1" width="5" height="5">
        <data encoding="csv">
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0
        </data>
      </layer>
      <objectgroup id="2" name="Object Layer 1" width="5" height="5">
        <object id="1" name="Template" template="template.tx" x="0" y="0" width="32" height="32" gid="1"/>
      </objectgroup>
    </map>
    """);

    resourceReader.Read("tileset.tsx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <tileset version="1.2" tiledversion="1.11.0" name="Tileset" tilewidth="32" tileheight="32" tilecount="1" columns="1">
      <tile id="1">
        <image width="32" height="32" source="tile.png"/>
      </tile>
    </tileset>
    """);

    resourceReader.Read("template.tx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <template>
      <object name="Poly">
        <properties>
          <property name="templateprop" value="helo there"/>
        </properties>
        <polygon points="0,0 104,20 35.6667,32.3333"/>
      </object>
    </template>
    """);

    var resourceCache = Substitute.For<IResourceCache>();
    resourceCache.GetTileset(Arg.Any<string>()).Returns(Optional<Tileset>.Empty);
    resourceCache.GetTemplate(Arg.Any<string>()).Returns(Optional<Template>.Empty);

    var customTypeDefinitions = Enumerable.Empty<ICustomTypeDefinition>();
    var loader = new Loader(resourceReader, resourceCache, customTypeDefinitions);

    // Act
    loader.LoadMap("map.tmx");

    // Assert
    resourceReader.Received(1).Read("template.tx");
    resourceCache.Received(1).GetTemplate("template.tx");
  }

  [Fact]
  public void LoadMap_CacheReturnsTileset_ReturnsTilesetFromCache()
  {
    // Arrange
    var resourceReader = Substitute.For<IResourceReader>();
    resourceReader.Read("map.tmx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map version="1.10" tiledversion="1.11.0" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="2" nextobjectid="1">
      <tileset firstgid="1" source="tileset.tsx"/>
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

    var resourceCache = Substitute.For<IResourceCache>();
    resourceCache.GetTileset("tileset.tsx").Returns(new Optional<Tileset>(new Tileset { Name = "Tileset", TileWidth = 32, TileHeight = 32, TileCount = 1, Columns = 1 }));

    var customTypeDefinitions = Enumerable.Empty<ICustomTypeDefinition>();
    var loader = new Loader(resourceReader, resourceCache, customTypeDefinitions);

    // Act
    loader.LoadMap("map.tmx");

    // Assert
    resourceReader.DidNotReceive().Read("tileset.tsx");
  }

  [Fact]
  public void LoadMap_CacheReturnsTemplate_ReturnsTemplateFromCache()
  {
    // Arrange
    var resourceReader = Substitute.For<IResourceReader>();
    resourceReader.Read("map.tmx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <map version="1.10" tiledversion="1.11.0" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="2" nextobjectid="1">
      <tileset firstgid="1" source="tileset.tsx"/>
      <layer id="1" name="Tile Layer 1" width="5" height="5">
        <data encoding="csv">
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0,
        0,0,0,0,0
        </data>
      </layer>
      <objectgroup id="2" name="Object Layer 1" width="5" height="5">
        <object id="1" name="Template" template="template.tx" x="0" y="0" width="32" height="32" gid="1"/>
      </objectgroup>
    </map>
    """);

    resourceReader.Read("tileset.tsx").Returns(
    """
    <?xml version="1.0" encoding="UTF-8"?>
    <tileset version="1.2" tiledversion="1.11.0" name="Tileset" tilewidth="32" tileheight="32" tilecount="1" columns="1">
      <tile id="1">
        <image width="32" height="32" source="tile.png"/>
      </tile>
    </tileset>
    """);

    var resourceCache = Substitute.For<IResourceCache>();
    resourceCache.GetTileset(Arg.Any<string>()).Returns(Optional<Tileset>.Empty);
    resourceCache.GetTemplate("template.tx").Returns(new Optional<Template>(new Template
    {
      Object = new PolygonObject
      {
        Points = [
          new Vector2(0,0),
          new Vector2(104,20),
          new Vector2(35.6667f,32.3333f)
        ],
        Properties = [
          new StringProperty { Name = "templateprop", Value = "helo there" }
        ]
      }
    }));

    var customTypeDefinitions = Enumerable.Empty<ICustomTypeDefinition>();
    var loader = new Loader(resourceReader, resourceCache, customTypeDefinitions);

    // Act
    loader.LoadMap("map.tmx");

    // Assert
    resourceReader.DidNotReceive().Read("template.tx");
  }

  [Fact]
  public void LoadMap_MapHasClassAndLoaderHasNoCustomTypes_ThrowsException()
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

    var resourceCache = Substitute.For<IResourceCache>();
    var customTypeDefinitions = Enumerable.Empty<ICustomTypeDefinition>();
    var loader = new Loader(resourceReader, resourceCache, customTypeDefinitions);

    // Act & Assert
    Assert.Throws<KeyNotFoundException>(() => loader.LoadMap("map.tmx"));
  }

  [Fact]
  public void LoadMap_MapHasClassAndLoaderHasCustomTypeWithSameName_ReturnsMapWithPropertiesFromCustomClass()
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
    var resourceCache = Substitute.For<IResourceCache>();
    var customClassDefinition = new CustomClassDefinition
    {
      Name = "TestClass",
      UseAs = CustomClassUseAs.All,
      Members = [
        new StringProperty { Name = "Test1", Value = "Hello" },
        new IntProperty { Name = "Test2", Value = 42 }
      ]
    };
    var loader = new Loader(resourceReader, resourceCache, [customClassDefinition]);

    // Act
    var result = loader.LoadMap("map.tmx");

    // Assert
    DotTiledAssert.AssertProperties(customClassDefinition.Members, result.Properties);
  }
}
