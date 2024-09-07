namespace DotTiled.Tests;

public static partial class DotTiledAssert
{
  internal static void AssertCustomClassDefinitionEqual(CustomClassDefinition expected, CustomClassDefinition actual)
  {
    AssertEqual(expected.ID, actual.ID, nameof(CustomClassDefinition.ID));
    AssertEqual(expected.Name, actual.Name, nameof(CustomClassDefinition.Name));
    AssertEqual(expected.Color, actual.Color, nameof(CustomClassDefinition.Color));
    AssertEqual(expected.DrawFill, actual.DrawFill, nameof(CustomClassDefinition.DrawFill));
    AssertEqual(expected.UseAs, actual.UseAs, nameof(CustomClassDefinition.UseAs));
    AssertProperties(expected.Members, actual.Members);
  }

  internal static void AssertCustomEnumDefinitionEqual(CustomEnumDefinition expected, CustomEnumDefinition actual)
  {
    AssertEqual(expected.ID, actual.ID, nameof(CustomEnumDefinition.ID));
    AssertEqual(expected.Name, actual.Name, nameof(CustomEnumDefinition.Name));
    AssertEqual(expected.StorageType, actual.StorageType, nameof(CustomEnumDefinition.StorageType));
    AssertEqual(expected.ValueAsFlags, actual.ValueAsFlags, nameof(CustomEnumDefinition.ValueAsFlags));
    AssertListOrdered(expected.Values, actual.Values, nameof(CustomEnumDefinition.Values));
  }
}
