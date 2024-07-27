namespace DotTiled.Tests;

public class TmxSerializerTests
{
  [Fact]
  public void TmxSerializerConstructor_ExternalTilesetResolverIsNull_ThrowsArgumentNullException()
  {
    // Arrange
    Func<TmxSerializer, string, Tileset> externalTilesetResolver = null!;
    Func<TmxSerializer, string, Template> externalTemplateResolver = null!;

    // Act
    Action act = () => _ = new TmxSerializer(externalTilesetResolver, externalTemplateResolver);

    // Assert
    Assert.Throws<ArgumentNullException>(act);
  }

  [Fact]
  public void TmxSerializerConstructor_ExternalTilesetResolverIsNotNull_DoesNotThrow()
  {
    // Arrange
    Func<TmxSerializer, string, Tileset> externalTilesetResolver = (_, _) => new Tileset();
    Func<TmxSerializer, string, Template> externalTemplateResolver = (_, _) => new Template { Object = new RectangleObject { } };

    // Act
    var tmxSerializer = new TmxSerializer(externalTilesetResolver, externalTemplateResolver);

    // Assert
    Assert.NotNull(tmxSerializer);
  }
}
