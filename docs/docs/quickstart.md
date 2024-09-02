# Quick Start

Install DotTiled from NuGet:

```bash
dotnet add package DotTiled
```

## Loading a map from the file system

This will create a loader that will load files from the underlying file system using <xref:DotTiled.Serialization.FileSystemResourceReader>. It will also be configured to use an in-memory cache to avoid loading the same tileset or template multiple times using <xref:DotTiled.Serialization.DefaultResourceCache>.

```csharp
using DotTiled;

var loader = Loader.Default();
var map = loader.LoadMap("path/to/map.tmx");
```

## Loading a map from a different source

If you want to load resources (maps, tilesets, templates) from a different source than the underlying file system, you can override the <xref:DotTiled.Serialization.FileSystemResourceReader> that is being used with your own implementation of <xref:DotTiled.Serialization.IResourceReader>.

```csharp	
using DotTiled;

var loader = Loader.DefaultWith(
  resourceReader: new MyCustomResourceReader());
var map = loader.LoadMap("path/to/map.tmx");
```

## Caching resources

Similarly, you can override the <xref:DotTiled.Serialization.DefaultResourceCache> that is being used with your own implementation of <xref:DotTiled.Serialization.IResourceCache>.

```csharp
using DotTiled;

var loader = Loader.DefaultWith(
  resourceReader: new MyCustomResourceReader(),
  resourceCache: new MyCustomResourceCache());
var map = loader.LoadMap("path/to/map.tmx");
```

## Custom types

If you have custom types in your map, you can provide any `IEnumerable<ICustomTypeDefinition>` to the loader. This will allow the loader to deserialize the custom types in your map.

```csharp
using DotTiled;

var monsterSpawnerDef = new CustomClassDefinition { ... };
var chestDef = new CustomClassDefinition { ... };

var loader = Loader.DefaultWith(
  customTypeDefinitions: [monsterSpawnerDef, chestDef]);
var map = loader.LoadMap("path/to/map.tmx");
```