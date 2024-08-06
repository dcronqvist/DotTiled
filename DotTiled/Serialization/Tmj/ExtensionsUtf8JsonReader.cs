using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace DotTiled;

internal partial class Tmj
{
  internal abstract class JsonProperty(string propertyName)
  {
    internal string PropertyName { get; } = propertyName;
  }

  internal class RequiredProperty<T>(string propertyName, Action<T> withValue) : JsonProperty(propertyName)
  {
    internal Action<T> WithValue { get; } = withValue;
  }

  internal class OptionalProperty<T>(string propertyName, Action<T?> withValue, bool allowNull = false) : JsonProperty(propertyName)
  {
    internal Action<T?> WithValue { get; } = withValue;
    internal bool AllowNull { get; } = allowNull;
  }
}

internal static class ExtensionsUtf8JsonReader
{
  private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
  {
    while (toCheck != typeof(object))
    {
      var cur = toCheck!.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
      if (generic == cur)
        return true;

      toCheck = toCheck.BaseType!;
    }

    return false;
  }

  internal static void Require<T>(this ref Utf8JsonReader reader, ProcessProperty process)
  {
    if (reader.TokenType == JsonTokenType.Null)
      throw new JsonException("Value is required.");

    process(ref reader);
  }

  internal static void MoveToContent(this ref Utf8JsonReader reader)
  {
    while (reader.Read() && reader.TokenType == JsonTokenType.Comment ||
           reader.TokenType == JsonTokenType.None)
      ;
  }

  internal delegate void ProcessProperty(ref Utf8JsonReader reader);

  internal static void ProcessJsonObject(this Utf8JsonReader reader, (string PropertyName, ProcessProperty Processor)[] processors)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
      throw new JsonException("Expected start of object.");

    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject)
        return;

      if (reader.TokenType != JsonTokenType.PropertyName)
        throw new JsonException("Expected property name.");

      var propertyName = reader.GetString();
      reader.Read();

      if (!processors.Any(x => x.PropertyName == propertyName))
      {
        reader.Skip();
        continue;
      }

      var processor = processors.First(x => x.PropertyName == propertyName).Processor;
      processor(ref reader);
    }

    throw new JsonException("Expected end of object.");
  }

  delegate T UseReader<T>(ref Utf8JsonReader reader);

  internal static void ProcessJsonObject(this Utf8JsonReader reader, Tmj.JsonProperty[] properties)
  {
    List<string> processedProperties = [];

    bool CheckType<T>(ref Utf8JsonReader reader, Tmj.JsonProperty prop, UseReader<T?> useReader)
    {
      return CheckRequire<T>(ref reader, prop, (ref Utf8JsonReader r) => useReader(ref r)!) || CheckOptional<T>(ref reader, prop, useReader);
    }

    bool CheckRequire<T>(ref Utf8JsonReader reader, Tmj.JsonProperty prop, UseReader<T> useReader)
    {
      if (prop is Tmj.RequiredProperty<T> requiredProp)
      {
        reader.Require<string>((ref Utf8JsonReader r) =>
        {
          requiredProp.WithValue(useReader(ref r));
        });
        return true;
      }
      return false;
    }

    bool CheckOptional<T>(ref Utf8JsonReader reader, Tmj.JsonProperty prop, UseReader<T?> useReader)
    {
      if (prop is Tmj.OptionalProperty<T> optionalProp)
      {
        if (reader.TokenType == JsonTokenType.Null && !optionalProp.AllowNull)
          throw new JsonException("Value cannot be null for optional property.");
        else if (reader.TokenType == JsonTokenType.Null && optionalProp.AllowNull)
          optionalProp.WithValue(default);
        else
          optionalProp.WithValue(useReader(ref reader));
        return true;
      }
      return false;
    }

    ProcessJsonObject(reader, properties.Select<Tmj.JsonProperty, (string, ProcessProperty)>(x => (x.PropertyName.ToLowerInvariant(), (ref Utf8JsonReader reader) =>
    {
      var lowerInvariant = x.PropertyName.ToLowerInvariant();

      if (processedProperties.Contains(lowerInvariant))
        throw new JsonException($"Property '{lowerInvariant}' was already processed.");

      processedProperties.Add(lowerInvariant);

      if (CheckType<string>(ref reader, x, (ref Utf8JsonReader r) => r.GetString()!))
        return;
      if (CheckType<int>(ref reader, x, (ref Utf8JsonReader r) => r.GetInt32()))
        return;
      if (CheckType<uint>(ref reader, x, (ref Utf8JsonReader r) => r.GetUInt32()))
        return;
      if (CheckType<float>(ref reader, x, (ref Utf8JsonReader r) => r.GetSingle()))
        return;

      throw new NotSupportedException($"Unsupported property type '{x.GetType().GenericTypeArguments.First()}'.");
    }
    )).ToArray());

    foreach (var property in properties)
    {
      if (IsSubclassOfRawGeneric(typeof(Tmj.RequiredProperty<>), property.GetType()) && !processedProperties.Contains(property.PropertyName.ToLowerInvariant()))
        throw new JsonException($"Required property '{property.PropertyName}' was not found.");
    }
  }
}
