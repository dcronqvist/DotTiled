namespace DotTiled.Tests;

public class CustomClassDefinitionTests
{
  private sealed class TestClass1
  {
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 42;
  }

  private static CustomClassDefinition ExpectedTestClass1Definition => new CustomClassDefinition
  {
    Name = "TestClass1",
    UseAs = CustomClassUseAs.All,
    Members = new List<IProperty>
    {
      new StringProperty { Name = "Name", Value = "John Doe" },
      new IntProperty { Name = "Age", Value = 42 }
    }
  };

  private sealed class TestClass2WithNestedClass
  {
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 42;
    public TestClass1 Nested { get; set; } = new TestClass1();
  }

  private static CustomClassDefinition ExpectedTestClass2WithNestedClassDefinition => new CustomClassDefinition
  {
    Name = "TestClass2WithNestedClass",
    UseAs = CustomClassUseAs.All,
    Members = [
      new StringProperty { Name = "Name", Value = "John Doe" },
      new IntProperty { Name = "Age", Value = 42 },
      new ClassProperty
      {
        Name = "Nested",
        PropertyType = "TestClass1",
        Value = []
      }
    ]
  };

  private sealed class TestClass3WithOverridenNestedClass
  {
    public string Name { get; set; } = "John Doe";
    public int Age { get; set; } = 42;
    public TestClass1 Nested { get; set; } = new TestClass1
    {
      Name = "Jane Doe"
    };
  }

  private static CustomClassDefinition ExpectedTestClass3WithOverridenNestedClassDefinition => new CustomClassDefinition
  {
    Name = "TestClass3WithOverridenNestedClass",
    UseAs = CustomClassUseAs.All,
    Members = [
      new StringProperty { Name = "Name", Value = "John Doe" },
      new IntProperty { Name = "Age", Value = 42 },
      new ClassProperty
      {
        Name = "Nested",
        PropertyType = "TestClass1",
        Value = [
          new StringProperty { Name = "Name", Value = "Jane Doe" },
        ]
      }
    ]
  };

  private enum TestEnum1
  {
    Value1,
    Value2
  }

  [Flags]
  private enum TestFlags1
  {
    Value1 = 0b001,
    Value2 = 0b010,
    Value3 = 0b100
  }

  private sealed class TestClass4WithEnums
  {
    public TestEnum1 Enum { get; set; } = TestEnum1.Value2;
    public TestFlags1 Flags { get; set; } = TestFlags1.Value1 | TestFlags1.Value2;
  }

  private static CustomClassDefinition ExpectedTestClass4WithEnumsDefinition => new CustomClassDefinition
  {
    Name = "TestClass4WithEnums",
    UseAs = CustomClassUseAs.All,
    Members = [
      new EnumProperty { Name = "Enum", PropertyType = "TestEnum1", Value = new HashSet<string> { "Value2" } },
      new EnumProperty { Name = "Flags", PropertyType = "TestFlags1", Value = new HashSet<string> { "Value1", "Value2" } }
    ]
  };

  private static IEnumerable<(Type, CustomClassDefinition)> GetCustomClassDefinitionTestData()
  {
    yield return (typeof(TestClass1), ExpectedTestClass1Definition);
    yield return (typeof(TestClass2WithNestedClass), ExpectedTestClass2WithNestedClassDefinition);
    yield return (typeof(TestClass3WithOverridenNestedClass), ExpectedTestClass3WithOverridenNestedClassDefinition);
    yield return (typeof(TestClass4WithEnums), ExpectedTestClass4WithEnumsDefinition);
  }

  public static IEnumerable<object[]> CustomClassDefinitionTestData =>
    GetCustomClassDefinitionTestData().Select(data => new object[] { data.Item1, data.Item2 });
  [Theory]
  [MemberData(nameof(CustomClassDefinitionTestData))]
  public void FromClass_Type_WhenTypeIsCustomClass_ReturnsCustomClassDefinition(Type type, CustomClassDefinition expected)
  {
    // Arrange & Act
    var result = CustomClassDefinition.FromClass(type);

    // Assert
    DotTiledAssert.AssertCustomClassDefinitionEqual(expected, result);
  }

  [Fact]
  public void FromClass_Type_WhenTypeIsNull_ThrowsArgumentNullException()
  {
    // Arrange
    Type type = null;

    // Act & Assert
    Assert.Throws<ArgumentNullException>(() => CustomClassDefinition.FromClass(type));
  }

  [Fact]
  public void FromClass_Type_WhenTypeIsString_ThrowsArgumentException()
  {
    // Arrange
    Type type = typeof(string);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => CustomClassDefinition.FromClass(type));
  }

  [Fact]
  public void FromClass_Type_WhenTypeIsNotClass_ThrowsArgumentException()
  {
    // Arrange
    Type type = typeof(int);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => CustomClassDefinition.FromClass(type));
  }
}
