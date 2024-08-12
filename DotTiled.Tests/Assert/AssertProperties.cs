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
    AssertEqual(expected.Count, actual.Count, "Properties.Count");
    foreach (var kvp in expected)
    {
      Assert.Contains(kvp.Key, actual.Keys);
      AssertProperty((dynamic)kvp.Value, (dynamic)actual[kvp.Key]);
    }
  }

  private static void AssertProperty(IProperty expected, IProperty actual)
  {
    AssertEqual(expected.Type, actual.Type, "Property.Type");
    AssertEqual(expected.Name, actual.Name, "Property.Name");
    AssertProperties((dynamic)actual, (dynamic)expected);
  }

  private static void AssertProperty(StringProperty expected, StringProperty actual)
  {
    AssertEqual(expected.Value, actual.Value, "StringProperty.Value");
  }

  private static void AssertProperty(IntProperty expected, IntProperty actual)
  {
    AssertEqual(expected.Value, actual.Value, "IntProperty.Value");
  }

  private static void AssertProperty(FloatProperty expected, FloatProperty actual)
  {
    AssertEqual(expected.Value, actual.Value, "FloatProperty.Value");
  }

  private static void AssertProperty(BoolProperty expected, BoolProperty actual)
  {
    AssertEqual(expected.Value, actual.Value, "BoolProperty.Value");
  }

  private static void AssertProperty(ColorProperty expected, ColorProperty actual)
  {
    AssertEqual(expected.Value, actual.Value, "ColorProperty.Value");
  }

  private static void AssertProperty(FileProperty expected, FileProperty actual)
  {
    AssertEqual(expected.Value, actual.Value, "FileProperty.Value");
  }

  private static void AssertProperty(ObjectProperty expected, ObjectProperty actual)
  {
    AssertEqual(expected.Value, actual.Value, "ObjectProperty.Value");
  }

  private static void AssertProperty(ClassProperty expected, ClassProperty actual)
  {
    AssertEqual(expected.PropertyType, actual.PropertyType, "ClassProperty.PropertyType");
    AssertProperties(expected.Properties, actual.Properties);
  }
}
