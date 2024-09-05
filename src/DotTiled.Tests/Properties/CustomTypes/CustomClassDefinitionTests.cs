namespace DotTiled.Tests;

public class CustomClassDefinitionTests
{
  [Fact]
  public void FromClassType_WhenTypeIsNotCustomClass_ThrowsArgumentException()
  {
    // Arrange
    var type = typeof(string);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => CustomClassDefinition.FromClassType(type));
  }

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

  private static IEnumerable<(Type, CustomClassDefinition)> GetCustomClassDefinitionTestData()
  {
    yield return (typeof(TestClass1), ExpectedTestClass1Definition);
    yield return (typeof(TestClass2WithNestedClass), ExpectedTestClass2WithNestedClassDefinition);
    yield return (typeof(TestClass3WithOverridenNestedClass), ExpectedTestClass3WithOverridenNestedClassDefinition);
  }

  private static void AssertCustomClassDefinitionEqual(CustomClassDefinition expected, CustomClassDefinition actual)
  {
    DotTiledAssert.AssertEqual(expected.ID, actual.ID, nameof(CustomClassDefinition.ID));
    DotTiledAssert.AssertEqual(expected.Name, actual.Name, nameof(CustomClassDefinition.Name));
    DotTiledAssert.AssertEqual(expected.Color, actual.Color, nameof(CustomClassDefinition.Color));
    DotTiledAssert.AssertEqual(expected.DrawFill, actual.DrawFill, nameof(CustomClassDefinition.DrawFill));
    DotTiledAssert.AssertEqual(expected.UseAs, actual.UseAs, nameof(CustomClassDefinition.UseAs));
    DotTiledAssert.AssertProperties(expected.Members, actual.Members);
  }

  public static IEnumerable<object[]> CustomClassDefinitionTestData =>
    GetCustomClassDefinitionTestData().Select(data => new object[] { data.Item1, data.Item2 });
  [Theory]
  [MemberData(nameof(CustomClassDefinitionTestData))]
  public void FromClassType_WhenTypeIsCustomClass_ReturnsCustomClassDefinition(Type type, CustomClassDefinition expected)
  {
    // Arrange & Act
    var result = CustomClassDefinition.FromClassType(type);

    // Assert
    AssertCustomClassDefinitionEqual(expected, result);
  }
}
