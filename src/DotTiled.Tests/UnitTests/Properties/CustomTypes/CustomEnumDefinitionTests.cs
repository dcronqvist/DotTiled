namespace DotTiled.Tests;

public class CustomEnumDefinitionTests
{
  [Fact]
  public void FromEnum_Type_WhenTypeIsNotEnum_ThrowsArgumentException()
  {
    // Arrange
    var type = typeof(string);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => CustomEnumDefinition.FromEnum(type));
  }

  private enum TestEnum1 { Value1, Value2, Value3 }

  [Fact]
  public void FromEnum_Type_WhenTypeIsEnum_ReturnsCustomEnumDefinition()
  {
    // Arrange
    var type = typeof(TestEnum1);
    var expected = new CustomEnumDefinition
    {
      ID = 0,
      Name = "TestEnum1",
      StorageType = CustomEnumStorageType.Int,
      Values = ["Value1", "Value2", "Value3"],
      ValueAsFlags = false
    };

    // Act
    var result = CustomEnumDefinition.FromEnum(type);

    // Assert
    DotTiledAssert.AssertCustomEnumDefinitionEqual(expected, result);
  }

  [Flags]
  private enum TestEnum2 { Value1, Value2, Value3 }

  [Fact]
  public void FromEnum_Type_WhenEnumIsFlags_ReturnsCustomEnumDefinition()
  {
    // Arrange
    var type = typeof(TestEnum2);
    var expected = new CustomEnumDefinition
    {
      ID = 0,
      Name = "TestEnum2",
      StorageType = CustomEnumStorageType.Int,
      Values = ["Value1", "Value2", "Value3"],
      ValueAsFlags = true
    };

    // Act
    var result = CustomEnumDefinition.FromEnum(type);

    // Assert
    DotTiledAssert.AssertCustomEnumDefinitionEqual(expected, result);
  }

  [Fact]
  public void FromEnum_T_WhenTypeIsEnum_ReturnsCustomEnumDefinition()
  {
    // Arrange
    var expected = new CustomEnumDefinition
    {
      ID = 0,
      Name = "TestEnum1",
      StorageType = CustomEnumStorageType.Int,
      Values = ["Value1", "Value2", "Value3"],
      ValueAsFlags = false
    };

    // Act
    var result = CustomEnumDefinition.FromEnum<TestEnum1>();

    // Assert
    DotTiledAssert.AssertCustomEnumDefinitionEqual(expected, result);
  }

  [Fact]
  public void FromEnum_T_WhenEnumIsFlags_ReturnsCustomEnumDefinition()
  {
    // Arrange
    var expected = new CustomEnumDefinition
    {
      ID = 0,
      Name = "TestEnum2",
      StorageType = CustomEnumStorageType.Int,
      Values = ["Value1", "Value2", "Value3"],
      ValueAsFlags = true
    };

    // Act
    var result = CustomEnumDefinition.FromEnum<TestEnum2>();

    // Assert
    DotTiledAssert.AssertCustomEnumDefinitionEqual(expected, result);
  }
}
