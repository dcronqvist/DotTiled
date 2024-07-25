namespace DotTiled.Tests;

public partial class TmxSerializerPropertiesTests
{
  public static void AssertProperties(Dictionary<string, IProperty>? actual, Dictionary<string, IProperty>? expected)
  {
    if (expected is null)
    {
      Assert.Null(actual);
      return;
    }

    Assert.NotNull(actual);
    Assert.Equal(expected.Count, actual.Count);
    foreach (var kvp in expected)
    {
      Assert.Contains(kvp.Key, actual.Keys);
      AssertProperty((dynamic)kvp.Value, (dynamic)actual[kvp.Key]);
    }
  }

  private static void AssertProperty(IProperty actual, IProperty expected)
  {
    Assert.Equal(expected.Type, actual.Type);
    Assert.Equal(expected.Name, actual.Name);
    AssertProperties((dynamic)actual, (dynamic)expected);
  }

  private static void AssertProperty(StringProperty actual, StringProperty expected)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(IntProperty actual, IntProperty expected)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(FloatProperty actual, FloatProperty expected)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(BoolProperty actual, BoolProperty expected)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(ColorProperty actual, ColorProperty expected)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(FileProperty actual, FileProperty expected)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(ObjectProperty actual, ObjectProperty expected)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(ClassProperty actual, ClassProperty expected)
  {
    Assert.Equal(expected.PropertyType, actual.PropertyType);
    AssertProperties(actual.Properties, expected.Properties);
  }
}
