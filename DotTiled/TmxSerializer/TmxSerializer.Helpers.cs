using System;

namespace DotTiled;

public partial class TmxSerializer
{
  private static class Helpers
  {
    public static void SetAtMostOnce<T>(ref T? field, T value, string fieldName)
    {
      if (field is not null)
        throw new InvalidOperationException($"{fieldName} already set");

      field = value;
    }
  }
}
