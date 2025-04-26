namespace DotTiled.Tests;

public class OptionalTests
{
  // Constructor Tests

  [Fact]
  public void Constructor_WithNonNullValue_ShouldSetHasValueToTrue()
  {
    // Arrange
    string expectedValue = "test";

    // Act
    var optional = new Optional<string>(expectedValue);

    // Assert
    Assert.True(optional.HasValue);
    Assert.Equal(expectedValue, optional.Value);
  }

  [Fact]
  public void Constructor_WithNullValue_ShouldSetHasValueToTrue()
  {
    // Arrange
    string expectedValue = null;

    // Act
    var optional = new Optional<string>(expectedValue);

    // Assert
    Assert.True(optional.HasValue);
    Assert.Null(optional.Value);
  }

  [Fact]
  public void DefaultConstructor_ShouldSetHasValueToFalse()
  {
    // Arrange & Act
    var optional = new Optional<string>();

    // Assert
    Assert.False(optional.HasValue);
    _ = Assert.Throws<InvalidOperationException>(() => optional.Value);
  }

  // Implicit Conversion Tests

  [Fact]
  public void ImplicitConversion_FromValueToOptional_ShouldSetHasValueToTrue()
  {
    // Arrange
    int expectedValue = 5;

    // Act
    Optional<int> optional = expectedValue;

    // Assert
    Assert.True(optional.HasValue);
    Assert.Equal(expectedValue, optional.Value);
  }

  // ToString Method Tests

  [Fact]
  public void ToString_WithValue_ShouldReturnValueToString()
  {
    // Arrange
    int expectedValue = 42;
    var optional = new Optional<int>(expectedValue);

    // Act
    string result = optional.ToString();

    // Assert
#pragma warning disable CA1305 // Specify IFormatProvider
    Assert.Equal(expectedValue.ToString(), result);
#pragma warning restore CA1305 // Specify IFormatProvider
  }

  [Fact]
  public void ToString_WithoutValue_ShouldReturnEmpty()
  {
    // Arrange & Act
    var optional = new Optional<int>();
    string result = optional.ToString();

    // Assert
    Assert.Equal("Empty", result);
  }

  // Equality Tests

  [Fact]
  public void Equals_WithSameValue_ShouldReturnTrue()
  {
    // Arrange
    var optional1 = new Optional<int>(10);
    var optional2 = new Optional<int>(10);

    // Act
    bool areEqual = optional1.Equals(optional2);

    // Assert
    Assert.True(areEqual);
  }

  [Fact]
  public void Equals_WithDifferentValues_ShouldReturnFalse()
  {
    // Arrange
    var optional1 = new Optional<int>(10);
    var optional2 = new Optional<int>(20);

    // Act
    bool areEqual = optional1.Equals(optional2);

    // Assert
    Assert.False(areEqual);
  }

  [Fact]
  public void Equals_WithNull_ShouldReturnFalse()
  {
    // Arrange
    var optional = new Optional<string>("test");

    // Act
    bool areEqual = optional.Equals(null);

    // Assert
    Assert.False(areEqual);
  }

  [Fact]
  public void Equals_WithEmptyOptional_ShouldReturnTrue()
  {
    // Arrange
    var optional1 = new Optional<string>();
    var optional2 = new Optional<string>();

    // Act
    bool areEqual = optional1.Equals(optional2);

    // Assert
    Assert.True(areEqual);
  }

  [Fact]
  public void Equals_WithSameReferenceTypeValue_ShouldReturnTrue()
  {
    // Arrange
    var value = new object();
    var optional1 = new Optional<object>(value);
    var optional2 = new Optional<object>(value);

    // Act
    bool areEqual = optional1.Equals(optional2);

    // Assert
    Assert.True(areEqual);
  }

  [Fact]
  public void Equals_WithDifferentReferenceTypeValues_ShouldReturnFalse()
  {
    // Arrange
    var optional1 = new Optional<object>(new object());
    var optional2 = new Optional<object>(new object());

    // Act
    bool areEqual = optional1.Equals(optional2);

    // Assert
    Assert.False(areEqual);
  }

  // GetHashCode Method Tests

  [Fact]
  public void GetHashCode_WithValue_ShouldReturnValueHashCode()
  {
    // Arrange
    int value = 42;
    var optional = new Optional<int>(value);

    // Act
    int hashCode = optional.GetHashCode();

    // Assert
    Assert.Equal(value.GetHashCode(), hashCode);
  }

  [Fact]
  public void GetHashCode_WithoutValue_ShouldReturnZero()
  {
    // Arrange & Act
    var optional = new Optional<int>();
    int hashCode = optional.GetHashCode();

    // Assert
    Assert.Equal(0, hashCode);
  }

  // Exception Tests

  [Fact]
  public void Value_WhenHasValueIsFalse_ShouldThrowInvalidOperationException()
  {
    // Arrange
    var optional = new Optional<string>();

    // Act & Assert
    _ = Assert.Throws<InvalidOperationException>(() => optional.Value);
  }

  [Fact]
  public void EmptyOptionalEquality_ShouldReturnTrue()
  {
    // Arrange
    var optional1 = new Optional<int>();
    var optional2 = new Optional<int>();

    // Act
    bool areEqual = optional1.Equals(optional2);

    // Assert
    Assert.True(areEqual);
  }

  // Special Scenarios

  public struct CustomStruct
  {
    public int X { get; set; }
  }

  [Fact]
  public void OptionalWithCustomStruct_ShouldBehaveCorrectly()
  {
    // Arrange
    var structValue = new CustomStruct { X = 10 };
    var optional = new Optional<CustomStruct>(structValue);

    // Act
    CustomStruct value = optional.Value;

    // Assert
    Assert.True(optional.HasValue);
    Assert.Equal(structValue, value);
  }

  [Fact]
  public void OptionalWithNullableInt_ShouldBehaveCorrectly()
  {
    // Arrange
    int? nullableValue = 5;
    var optional = new Optional<int?>(nullableValue);

    // Act
    int? value = optional.Value;

    // Assert
    Assert.True(optional.HasValue);
    Assert.Equal(nullableValue, value);
  }

  [Fact]
  public void OptionalWithNullNullableInt_ShouldBehaveCorrectly()
  {
    // Arrange
    int? nullableValue = null;
    var optional = new Optional<int?>(nullableValue);

    // Act
    int? value = optional.Value;

    // Assert
    Assert.True(optional.HasValue);
    Assert.Null(value);
  }
}
