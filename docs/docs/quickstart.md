# Quick Start

Install DotTiled from NuGet:

```bash
dotnet add package DotTiled
```

Use the `DotTiled` namespace (if you want).

```csharp
using DotTiled;
```

Or fully qualify all `DotTiled` types e.g. `DotTiled.Loader`.

## Loading a map from the file system

This will create a loader that will load files from the underlying file system using <xref:DotTiled.Serialization.FileSystemResourceReader>. It will also be configured to use an in-memory cache to avoid loading the same tileset or template multiple times using <xref:DotTiled.Serialization.DefaultResourceCache>.

```csharp
var loader = Loader.Default();
var map = loader.LoadMap("path/to/map.tmx");
```

## Loading a map from a different source

If you want to load resources (maps, tilesets, templates) from a different source than the underlying file system, you can override the <xref:DotTiled.Serialization.FileSystemResourceReader> that is being used with your own implementation of <xref:DotTiled.Serialization.IResourceReader>.

```csharp	
var loader = Loader.DefaultWith(
  resourceReader: new MyCustomResourceReader());
var map = loader.LoadMap("path/to/map.tmx");
```

## Caching resources

Similarly, you can override the <xref:DotTiled.Serialization.DefaultResourceCache> that is being used with your own implementation of <xref:DotTiled.Serialization.IResourceCache>.

```csharp
var loader = Loader.DefaultWith(
  resourceReader: new MyCustomResourceReader(),
  resourceCache: new MyCustomResourceCache());
var map = loader.LoadMap("path/to/map.tmx");
```

## Custom types

If you have custom types in your map, you can provide any `IEnumerable<ICustomTypeDefinition>` to the loader. This will allow the loader to deserialize the custom types in your map.

```csharp
var monsterSpawnerDef = new CustomClassDefinition { ... };
var chestDef = new CustomClassDefinition
{
  Name = "Chest",
  UseAs = CustomClassUseAs.All,
  Members = [
    new IntProperty { Name = "coins", Value = 0 },
    new BoolProperty { Name = "locked", Value = true }
  ]
};

var loader = Loader.DefaultWith(
  customTypeDefinitions: [monsterSpawnerDef, chestDef]);
var map = loader.LoadMap("path/to/map.tmx");

var chest = map.GetProperty<CustomClassProperty>("chest").Value;
var coinsToSpawn = chest.GetProperty<IntProperty>("coins").Value;
```

## Examples

See the [DotTiled.Examples](https://github.com/dcronqvist/DotTiled/tree/master/src/DotTiled.Examples) directory for more in-depth examples of how to use DotTiled. The [DotTiled.Example.Raylib](https://github.com/dcronqvist/DotTiled/tree/master/src/DotTiled.Examples/DotTiled.Example.Raylib) example is a good starting point for using DotTiled with Raylib or any similar library/framework. It demonstrates how to load and render a Tiled map, handle player movement, and perform collision detection.