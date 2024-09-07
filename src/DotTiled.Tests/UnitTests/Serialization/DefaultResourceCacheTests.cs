using DotTiled.Serialization;

namespace DotTiled.Tests;

public class DefaultResourceCacheTests
{
  [Fact]
  public void GetTemplate_TemplateDoesNotExist_ReturnsEmptyOptional()
  {
    // Arrange
    var cache = new DefaultResourceCache();
    var path = "template.tsx";

    // Act
    var result = cache.GetTemplate(path);

    // Assert
    Assert.False(result.HasValue);
  }

  [Fact]
  public void GetTemplate_TemplateHasBeenInserted_ReturnsTemplate()
  {
    // Arrange
    var cache = new DefaultResourceCache();
    var path = "template.tsx";
    var template = new Template
    {
      Object = new EllipseObject { }
    };

    // Act
    cache.InsertTemplate(path, template);
    var result = cache.GetTemplate(path);

    // Assert
    Assert.True(result.HasValue);
    Assert.Same(template, result.Value);
  }

  [Fact]
  public void GetTileset_TilesetDoesNotExist_ReturnsEmptyOptional()
  {
    // Arrange
    var cache = new DefaultResourceCache();
    var path = "tileset.tsx";

    // Act
    var result = cache.GetTileset(path);

    // Assert
    Assert.False(result.HasValue);
  }

  [Fact]
  public void GetTileset_TilesetHasBeenInserted_ReturnsTileset()
  {
    // Arrange
    var cache = new DefaultResourceCache();
    var path = "tileset.tsx";
    var tileset = new Tileset
    {
      Name = "Tileset",
      TileWidth = 32,
      TileHeight = 32,
      TileCount = 1,
      Columns = 1
    };

    // Act
    cache.InsertTileset(path, tileset);
    var result = cache.GetTileset(path);

    // Assert
    Assert.True(result.HasValue);
    Assert.Same(tileset, result.Value);
  }
}
