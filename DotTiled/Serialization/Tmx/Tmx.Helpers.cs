using System;

namespace DotTiled;

internal partial class Tmx
{
  private static class Helpers
  {
    public static void SetAtMostOnce<T>(ref T? field, T value, string fieldName)
    {
      if (field is not null)
        throw new InvalidOperationException($"{fieldName} already set");

      field = value;
    }

    public static void SetAtMostOnceUsingCounter<T>(ref T? field, T value, string fieldName, ref int counter)
    {
      if (counter > 0)
        throw new InvalidOperationException($"{fieldName} already set");

      field = value;
      counter++;
    }
  }
}
