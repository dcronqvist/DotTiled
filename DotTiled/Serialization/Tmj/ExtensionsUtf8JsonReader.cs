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

  internal delegate void UseReader(ref Utf8JsonReader reader);

  internal class RequiredProperty(string propertyName, UseReader useReader) : JsonProperty(propertyName)
  {
    internal UseReader UseReader { get; } = useReader;
  }

  internal class OptionalProperty(string propertyName, UseReader useReader, bool allowNull = true) : JsonProperty(propertyName)
  {
    internal UseReader UseReader { get; } = useReader;
    internal bool AllowNull { get; } = allowNull;
  }
}

internal static class ExtensionsUtf8JsonReader
{
  internal static T Progress<T>(ref this Utf8JsonReader reader, T value)
  {
    reader.Read();
    return value;
  }

  internal static void MoveToContent(this ref Utf8JsonReader reader)
  {
    while (reader.Read() && reader.TokenType == JsonTokenType.Comment ||
           reader.TokenType == JsonTokenType.None)
      ;
  }

  internal delegate void ProcessProperty(ref Utf8JsonReader reader);

  private static void ProcessJsonObject(this ref Utf8JsonReader reader, (string PropertyName, ProcessProperty Processor)[] processors)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
      throw new JsonException("Expected start of object.");

    reader.Read();

    while (reader.TokenType != JsonTokenType.EndObject)
    {
      if (reader.TokenType != JsonTokenType.PropertyName)
        throw new JsonException("Expected property name.");

      var propertyName = reader.GetString();
      reader.Read();

      if (!processors.Any(x => x.PropertyName == propertyName))
      {
        var depthBefore = reader.CurrentDepth;

        while (reader.TokenType != JsonTokenType.PropertyName || reader.CurrentDepth > depthBefore)
          reader.Read();

        continue;
      }

      var processor = processors.First(x => x.PropertyName == propertyName).Processor;
      processor(ref reader);
    }

    if (reader.TokenType != JsonTokenType.EndObject)
      throw new JsonException("Expected end of object.");

    reader.Read();
  }

  internal static void ProcessJsonObject(this ref Utf8JsonReader reader, Tmj.JsonProperty[] properties, string objectTypeName)
  {
    List<string> processedProperties = [];

    ProcessJsonObject(ref reader, properties.Select<Tmj.JsonProperty, (string, ProcessProperty)>(x => (x.PropertyName, (ref Utf8JsonReader reader) =>
    {
      if (processedProperties.Contains(x.PropertyName))
        throw new JsonException($"Property '{x.PropertyName}' was already processed.");

      processedProperties.Add(x.PropertyName);

      if (x is Tmj.RequiredProperty req)
      {
        if (reader.TokenType == JsonTokenType.Null)
          throw new JsonException($"Required property '{req.PropertyName}' cannot be null when reading {objectTypeName}.");

        req.UseReader(ref reader);
      }
      else if (x is Tmj.OptionalProperty opt)
      {
        if (reader.TokenType == JsonTokenType.Null && !opt.AllowNull)
          throw new JsonException($"Value cannot be null for optional property '{opt.PropertyName}' when reading {objectTypeName}.");
        else if (reader.TokenType == JsonTokenType.Null && opt.AllowNull)
          return;

        opt.UseReader(ref reader);
      }
    }
    )).ToArray());

    foreach (var property in properties)
    {
      if (property is Tmj.RequiredProperty && !processedProperties.Contains(property.PropertyName))
        throw new JsonException($"Required property '{property.PropertyName}' was not found when reading {objectTypeName}.");
    }
  }

  internal delegate void UseReader(ref Utf8JsonReader reader);

  internal static void ProcessJsonArray(this ref Utf8JsonReader reader, UseReader useReader)
  {
    if (reader.TokenType != JsonTokenType.StartArray)
      throw new JsonException("Expected start of array.");

    reader.Read();

    while (reader.TokenType != JsonTokenType.EndArray)
    {
      useReader(ref reader);
    }

    if (reader.TokenType != JsonTokenType.EndArray)
      throw new JsonException("Expected end of array.");

    reader.Read();
  }
}
