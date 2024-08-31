using System.Globalization;

namespace DotTiled.Tests;

public class HasPropertiesBaseTests
{
  private sealed class TestHasProperties(IList<IProperty> props) : HasPropertiesBase
  {
    public override IList<IProperty> GetProperties() => props;
  }

  private sealed class MapTo
  {
    public bool MapToBool { get; set; } = false;
    public Color MapToColor { get; set; } = Color.Parse("#00000000", CultureInfo.InvariantCulture);
    public float MapToFloat { get; set; } = 0.0f;
    public string MapToFile { get; set; } = "";
    public int MapToInt { get; set; } = 0;
    public int MapToObject { get; set; } = 0;
    public string MapToString { get; set; } = "";
  }

  [Fact]
  public void MapClassPropertyTo_PropertyNotFound_ThrowsKeyNotFoundException()
  {
    // Arrange
    List<IProperty> props = [
      new ClassProperty {
        Name = "ClassInObject",
        PropertyType = "MapTo",
        Value = [
          new StringProperty { Name = "PropertyThatDoesNotExistInMapTo", Value = "Test" }
        ],
      }
    ];
    var hasProperties = new TestHasProperties(props);

    // Act & Assert
    _ = Assert.Throws<KeyNotFoundException>(() => hasProperties.GetProperty<ClassProperty>("ClassInObject").MapPropertiesTo<MapTo>());
  }

  [Fact]
  public void MapClassPropertyTo_PropertyIsNotClassProperty_ThrowsInvalidCastException()
  {
    // Arrange
    List<IProperty> props = [
      new StringProperty { Name = "ClassInObject", Value = "Test" }
    ];
    var hasProperties = new TestHasProperties(props);

    // Act & Assert
    _ = Assert.Throws<InvalidCastException>(() => hasProperties.GetProperty<ClassProperty>("ClassInObject").MapPropertiesTo<MapTo>());
  }

  [Fact]
  public void MapClassPropertyTo_AllBasicValidProperties_ReturnsMappedProperty()
  {
    // Arrange
    List<IProperty> props = [
      new ClassProperty {
        Name = "ClassInObject",
        PropertyType = "MapTo",
        Value = [
          new BoolProperty { Name = "MapToBool", Value = true },
          new ColorProperty { Name = "MapToColor", Value = Color.Parse("#FF0000FF", CultureInfo.InvariantCulture) },
          new FloatProperty { Name = "MapToFloat", Value = 1.0f },
          new StringProperty { Name = "MapToFile", Value = "Test" },
          new IntProperty { Name = "MapToInt", Value = 1 },
          new IntProperty { Name = "MapToObject", Value = 1 },
          new StringProperty { Name = "MapToString", Value = "Test" },
        ],
      }
    ];
    var hasProperties = new TestHasProperties(props);

    // Act
    var mappedProperty = hasProperties.GetProperty<ClassProperty>("ClassInObject").MapPropertiesTo<MapTo>();

    // Assert
    Assert.True(mappedProperty.MapToBool);
    Assert.Equal(Color.Parse("#FF0000FF", CultureInfo.InvariantCulture), mappedProperty.MapToColor);
    Assert.Equal(1.0f, mappedProperty.MapToFloat);
    Assert.Equal("Test", mappedProperty.MapToFile);
    Assert.Equal(1, mappedProperty.MapToInt);
    Assert.Equal(1, mappedProperty.MapToObject);
    Assert.Equal("Test", mappedProperty.MapToString);
  }

  private sealed class NestedMapTo
  {
    public string NestedMapToString { get; set; } = "";
    public MapTo MapToInNested { get; set; } = new MapTo();
  }

  [Fact]
  public void MapClassPropertyTo_NestedMapTo_ReturnsMappedProperty()
  {
    // Arrange
    List<IProperty> props = [
      new ClassProperty {
        Name = "ClassInObject",
        PropertyType = "NestedMapTo",
        Value = [
          new StringProperty { Name = "NestedMapToString", Value = "Test" },
          new ClassProperty {
            Name = "MapToInNested",
            PropertyType = "MapTo",
            Value = [
              new BoolProperty { Name = "MapToBool", Value = true },
              new ColorProperty { Name = "MapToColor", Value = Color.Parse("#FF0000FF", CultureInfo.InvariantCulture) },
              new FloatProperty { Name = "MapToFloat", Value = 1.0f },
              new StringProperty { Name = "MapToFile", Value = "Test" },
              new IntProperty { Name = "MapToInt", Value = 1 },
              new IntProperty { Name = "MapToObject", Value = 1 },
              new StringProperty { Name = "MapToString", Value = "Test" },
            ],
          },
        ],
      }
    ];
    var hasProperties = new TestHasProperties(props);

    // Act
    var mappedProperty = hasProperties.GetProperty<ClassProperty>("ClassInObject").MapPropertiesTo<NestedMapTo>();

    // Assert
    Assert.Equal("Test", mappedProperty.NestedMapToString);
    Assert.True(mappedProperty.MapToInNested.MapToBool);
    Assert.Equal(Color.Parse("#FF0000FF", CultureInfo.InvariantCulture), mappedProperty.MapToInNested.MapToColor);
    Assert.Equal(1.0f, mappedProperty.MapToInNested.MapToFloat);
    Assert.Equal("Test", mappedProperty.MapToInNested.MapToFile);
    Assert.Equal(1, mappedProperty.MapToInNested.MapToInt);
    Assert.Equal(1, mappedProperty.MapToInNested.MapToObject);
    Assert.Equal("Test", mappedProperty.MapToInNested.MapToString);
  }

  private enum TestEnum
  {
    TestValue1,
    TestValue2,
    TestValue3
  }

  private sealed class EnumMapTo
  {
    public TestEnum EnumMapToEnum { get; set; } = TestEnum.TestValue1;
  }

  [Fact]
  public void MapClassPropertyTo_EnumProperty_ReturnsMappedProperty()
  {
    // Arrange
    List<IProperty> props = [
      new ClassProperty {
        Name = "ClassInObject",
        PropertyType = "EnumMapTo",
        Value = [
          new EnumProperty { Name = "EnumMapToEnum", PropertyType = "TestEnum", Value = new HashSet<string> { "TestValue1" } },
        ],
      }
    ];
    var hasProperties = new TestHasProperties(props);

    // Act
    var mappedProperty = hasProperties.GetProperty<ClassProperty>("ClassInObject").MapPropertiesTo<EnumMapTo>();

    // Assert
    Assert.Equal(TestEnum.TestValue1, mappedProperty.EnumMapToEnum);
  }

  private enum TestEnumWithFlags
  {
    TestValue1 = 1,
    TestValue2 = 2,
    TestValue3 = 4
  }

  private sealed class EnumWithFlagsMapTo
  {
    public TestEnumWithFlags EnumWithFlagsMapToEnum { get; set; } = TestEnumWithFlags.TestValue1;
  }

  [Fact]
  public void MapClassPropertyTo_EnumWithFlagsProperty_ReturnsMappedProperty()
  {
    // Arrange
    List<IProperty> props = [
      new ClassProperty {
        Name = "ClassInObject",
        PropertyType = "EnumWithFlagsMapTo",
        Value = [
          new EnumProperty { Name = "EnumWithFlagsMapToEnum", PropertyType = "TestEnumWithFlags", Value = new HashSet<string> { "TestValue1", "TestValue2" } },
        ],
      }
    ];
    var hasProperties = new TestHasProperties(props);

    // Act
    var mappedProperty = hasProperties.GetProperty<ClassProperty>("ClassInObject").MapPropertiesTo<EnumWithFlagsMapTo>();

    // Assert
    Assert.Equal(TestEnumWithFlags.TestValue1 | TestEnumWithFlags.TestValue2, mappedProperty.EnumWithFlagsMapToEnum);
  }

  [Fact]
  public void MapPropertiesTo_WithProperties_ReturnsMappedProperty()
  {
    // Arrange
    List<IProperty> props = [
      new BoolProperty { Name = "MapToBool", Value = true },
      new ColorProperty { Name = "MapToColor", Value = Color.Parse("#FF0000FF", CultureInfo.InvariantCulture) },
      new FloatProperty { Name = "MapToFloat", Value = 1.0f },
      new StringProperty { Name = "MapToFile", Value = "Test" },
      new IntProperty { Name = "MapToInt", Value = 1 },
      new IntProperty { Name = "MapToObject", Value = 1 },
      new StringProperty { Name = "MapToString", Value = "Test" },
    ];
    var hasProperties = new TestHasProperties(props);

    // Act
    var mappedProperty = hasProperties.MapPropertiesTo<MapTo>();

    // Assert
    Assert.True(mappedProperty.MapToBool);
    Assert.Equal(Color.Parse("#FF0000FF", CultureInfo.InvariantCulture), mappedProperty.MapToColor);
    Assert.Equal(1.0f, mappedProperty.MapToFloat);
    Assert.Equal("Test", mappedProperty.MapToFile);
    Assert.Equal(1, mappedProperty.MapToInt);
    Assert.Equal(1, mappedProperty.MapToObject);
    Assert.Equal("Test", mappedProperty.MapToString);
  }
}
