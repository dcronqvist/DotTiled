namespace DotTiled.Tests;

public class TmxSerializerTests
{
  [Fact]
  public void TmxSerializerConstructor_ExternalTilesetResolverIsNull_ThrowsArgumentNullException()
  {
    // Arrange
    Func<string, Tileset> externalTilesetResolver = null!;

    // Act
    Action act = () => _ = new TmxSerializer(externalTilesetResolver);

    // Assert
    Assert.Throws<ArgumentNullException>(act);
  }

  [Fact]
  public void TmxSerializerConstructor_ExternalTilesetResolverIsNotNull_DoesNotThrow()
  {
    // Arrange
    Func<string, Tileset> externalTilesetResolver = _ => new Tileset();

    // Act
    var tmxSerializer = new TmxSerializer(externalTilesetResolver);

    // Assert
    Assert.NotNull(tmxSerializer);
  }
}
