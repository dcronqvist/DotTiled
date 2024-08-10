namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertProperties(Dictionary<string, IProperty>? expected, Dictionary<string, IProperty>? actual)
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

  private static void AssertProperty(IProperty expected, IProperty actual)
  {
    Assert.Equal(expected.Type, actual.Type);
    Assert.Equal(expected.Name, actual.Name);
    AssertProperties((dynamic)actual, (dynamic)expected);
  }

  private static void AssertProperty(StringProperty expected, StringProperty actual)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(IntProperty expected, IntProperty actual)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(FloatProperty expected, FloatProperty actual)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(BoolProperty expected, BoolProperty actual)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(ColorProperty expected, ColorProperty actual)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(FileProperty expected, FileProperty actual)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(ObjectProperty expected, ObjectProperty actual)
  {
    Assert.Equal(expected.Value, actual.Value);
  }

  private static void AssertProperty(ClassProperty expected, ClassProperty actual)
  {
    Assert.Equal(expected.PropertyType, actual.PropertyType);
    AssertProperties(expected.Properties, actual.Properties);
  }
}
