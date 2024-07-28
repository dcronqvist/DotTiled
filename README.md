# 📚 DotTiled

<img src="https://www.mapeditor.org/img/tiled-logo-white.png" align="right" width="20%"/>

DotTiled is a simple and easy-to-use library for loading, saving, and managing [Tiled maps and tilesets](https://mapeditor.org) in your .NET projects. After [TiledCS](https://github.com/TheBoneJarmer/TiledCS) unfortunately became unmaintained (since 2022), I aimed to create a new library that could fill its shoes. DotTiled is the result of that effort.

- [Feature coverage](#feature-coverage)
- [Alternative libraries and comparison + benchmarks](#alternative-libraries-and-comparison)
- [Quickstart](#quickstart)
  - [Installing DotTiled](#installing-dottiled)
  - [Constructing a `TmxSerializer`](#constructing-a-tmxserializer)
  - [Loading a `.tmx` map](#loading-a-tmx-map)
  - [Loading a `.tsx` tileset](#loading-a-tsx-tileset)
  - [Constructing a `TmjSerializer`](#constructing-a-tmjserializer)
  - [Loading a `.tmj` map](#loading-a-tmj-map)
  - [Loading a `.tsj` tileset](#loading-a-tsj-tileset)

# Feature coverage

**TBD**: Add a table displaying the features that DotTiled supports, and which features are not yet supported, and perhaps why they are not supported (yet or ever).

# Alternative libraries and comparison

Other similar libraries exist, and you may want to consider them for your project as well:

| Library | Actively maintained | Supported formats | Feature coverage | Tiled version compatibility | Docs | License | Benchmark rank* |
| --- | --- | --- | --- | --- | --- | --- | --- |
| **DotTiled** | ✅ | `.tmx` `.tsx` `.tmj` `.tsj` `.tx` | | | Usage, API, XML docs | | |
| [TiledLib](https://github.com/Ragath/TiledLib.Net) |✅| | | | | | |
| [TiledCSPlus](https://github.com/nolemretaWxd/TiledCSPlus) |✅| | | | | | |
| [TiledSharp](https://github.com/marshallward/TiledSharp) |❌| | | | | | |
| [TiledCS](https://github.com/TheBoneJarmer/TiledCS) |❌| | | | | | |
| [TiledNet](https://github.com/napen123/Tiled.Net) |❌| | | | | | |

> [!NOTE]
> *Benchmark rank is based on the libraries' speed and memory usage when loading different types of maps and tilesets. Further explanations and details can be found in the below collapsible section.

<details>
<summary>
Comparison and benchmark details
</summary>

**TODO: Add table displaying feature availability**

**TODO: Add table displaying benchmark results**

</details>

[MonoGame](https://www.monogame.net) users may also want to consider using [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended) for loading Tiled maps and tilesets. Like MonoGame.Extended, DotTiled also provides a way to properly import Tiled maps and tilesets with the MonoGame content pipeline (with the DotTiled.MonoGame.Pipeline NuGet). However, unlike MonoGame.Extended, DotTiled does *not* include any kind of rendering capabilities, and it is up to you as a developer to implement any kind of rendering for your maps when using DotTiled. The feature coverage by MonoGame.Extended is less than that of DotTiled, so you may want to consider using DotTiled if you need access to more Tiled features and flexibility.

# Quickstart

### Installing DotTiled

DotTiled is available as a NuGet package. You can install it by using the NuGet Package Manager UI in Visual Studio, or equivalent, or using the following command for the .NET CLI:

```pwsh
dotnet add package DotTiled
```


### Constructing a `TmxSerializer`

There are few details to be aware of for your `TmxSerializer`:

```csharp
// A map may or may not contain tilesets that are stored in external
// files. To deserialize a map, you must provide a way to resolve such
// tilesets.
Func<TmxSerializer, string, Tileset> tilesetResolver = 
  (TmxSerializer serializer, string path) => 
  {
    string tilesetXml = fileSystem.ReadAllText(path);
    return serializer.DeserializeTileset(tilesetXml);
  };

// A map may or may not contain objects that reference template files.
// To deserialize a map, you must provide a way to resolve such templates.
Func<TmxSerializer, string, Template> templateResolver = 
  (TmxSerializer serializer, string path) => 
  {
    string templateXml = fileSystem.ReadAllText(path);
    return serializer.DeserializeTemplate(templateXml);
  };

var tmxSerializer = new TmxSerializer(tilesetResolver, templateResolver);
```

### Loading a `.tmx` map 

The `TmxSerializer` has several overloads for `DeserializeMap` that allow you to load a map from a number of different sources.

```csharp
string mapXml = fileSystem.ReadAllText("path/to/map.tmx");
Map mapFromRaw = tmxSerializer.DeserializeMap(mapXml); // From raw XML string in memory

using var reader = fileSystem.OpenXmlReader("path/to/map.tmx");
Map mapFromReader = tmxSerializer.DeserializeMap(reader); // From XML reader
```

### Loading a `.tsx` tileset

Similar to maps, the `TmxSerializer` has several overloads for `DeserializeTileset` that allow you to load a tileset from a number of different sources.

```csharp
string tilesetXml = fileSystem.ReadAllText("path/to/tileset.tsx");
Tileset tileset = tmxSerializer.DeserializeTileset(tilesetXml); // From raw XML string in memory

using var reader = fileSystem.OpenXmlReader("path/to/tileset.tsx");
Tileset tileset = tmxSerializer.DeserializeTileset(reader); // From XML reader
```