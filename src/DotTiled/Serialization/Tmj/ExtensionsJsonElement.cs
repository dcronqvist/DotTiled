using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace DotTiled.Serialization.Tmj;

internal static class ExtensionsJsonElement
{
  internal static T GetRequiredProperty<T>(this JsonElement element, string propertyName)
  {
    if (!element.TryGetProperty(propertyName, out var property))
      throw new JsonException($"Missing required property '{propertyName}'.");

    return property.GetValueAs<T>();
  }

  internal static Optional<T> GetOptionalProperty<T>(this JsonElement element, string propertyName)
  {
    if (!element.TryGetProperty(propertyName, out var property))
      return Optional.Empty;

    if (property.ValueKind == JsonValueKind.Null)
      return Optional.Empty;

    return property.GetValueAs<T>();
  }

  internal static T GetValueAs<T>(this JsonElement element)
  {
    bool isNullable = Nullable.GetUnderlyingType(typeof(T)) != null;

    if (isNullable && element.ValueKind == JsonValueKind.Null)
      return default!;

    var realType = isNullable ? Nullable.GetUnderlyingType(typeof(T))! : typeof(T);

    string val = realType switch
    {
      Type t when t == typeof(string) => element.GetString()!,
      Type t when t == typeof(int) => element.GetInt32().ToString(CultureInfo.InvariantCulture),
      Type t when t == typeof(uint) => element.GetUInt32().ToString(CultureInfo.InvariantCulture),
      Type t when t == typeof(float) => element.GetSingle().ToString(CultureInfo.InvariantCulture),
      Type t when t == typeof(bool) => element.GetBoolean().ToString(CultureInfo.InvariantCulture),
      _ => throw new JsonException($"Unsupported type '{typeof(T)}'.")
    };

    return (T)Convert.ChangeType(val, realType, CultureInfo.InvariantCulture);
  }

  internal static T GetRequiredPropertyParseable<T>(this JsonElement element, string propertyName) where T : IParsable<T>
  {
    if (!element.TryGetProperty(propertyName, out var property))
      throw new JsonException($"Missing required property '{propertyName}'.");

    return T.Parse(property.GetString()!, CultureInfo.InvariantCulture);
  }

  internal static T GetRequiredPropertyParseable<T>(this JsonElement element, string propertyName, Func<string, T> parser)
  {
    if (!element.TryGetProperty(propertyName, out var property))
      throw new JsonException($"Missing required property '{propertyName}'.");

    return parser(property.GetString()!);
  }

  internal static Optional<T> GetOptionalPropertyParseable<T>(this JsonElement element, string propertyName) where T : IParsable<T>
  {
    if (!element.TryGetProperty(propertyName, out var property))
      return Optional.Empty;

    return T.Parse(property.GetString()!, CultureInfo.InvariantCulture);
  }

  internal static Optional<T> GetOptionalPropertyParseable<T>(this JsonElement element, string propertyName, Func<string, Optional<T>> parser)
  {
    if (!element.TryGetProperty(propertyName, out var property))
      return Optional.Empty;

    return parser(property.GetString()!);
  }

  internal static T GetRequiredPropertyCustom<T>(this JsonElement element, string propertyName, Func<JsonElement, T> parser)
  {
    if (!element.TryGetProperty(propertyName, out var property))
      throw new JsonException($"Missing required property '{propertyName}'.");

    return parser(property);
  }

  internal static Optional<T> GetOptionalPropertyCustom<T>(this JsonElement element, string propertyName, Func<JsonElement, T> parser)
  {
    if (!element.TryGetProperty(propertyName, out var property))
      return Optional.Empty;

    return parser(property);
  }

  internal static List<T> GetValueAsList<T>(this JsonElement element, Func<JsonElement, T> parser)
  {
    var list = new List<T>();

    foreach (var item in element.EnumerateArray())
      list.Add(parser(item));

    return list;
  }
}
