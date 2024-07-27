# 📚 DotTiled

<img src="https://www.mapeditor.org/img/tiled-logo-white.png" align="right" width="20%"/>

DotTiled is a simple and easy-to-use library for loading, saving, and managing [Tiled maps and tilesets](https://mapeditor.org) in your .NET projects. After [TiledCS](https://github.com/TheBoneJarmer/TiledCS) unfortunately became unmaintained (since 2022), I aimed to create a new library that could fill its shoes. DotTiled is the result of that effort.

Other similar libraries exist, and you may want to consider them as well:
| Library | Active | Formats | Docs | License | Benchmark* Rank |
| --- | --- | --- | --- | --- | --- |
| **DotTiled** | ✅ | `.tmx`, `.tsx` | Usage, API, XML docs | MIT | 1 |
| [TiledLib](https://github.com/Ragath/TiledLib.Net) | ✅ | | | | |
| [TiledCSPlus](https://github.com/nolemretaWxd/TiledCSPlus) | ✅ | | | | |
| [TiledSharp](https://github.com/marshallward/TiledSharp) | ❌ | | | | |
| [TiledCS](https://github.com/TheBoneJarmer/TiledCS) | ❌ | | | | |
| [TiledNet](https://github.com/napen123/Tiled.Net) | ❌ | | | | |

## Quickstart

Here are a few examples to get you started with DotTiled.

### Loading a `.tmx` map 

```csharp
var tmxSerializer = new TmxSerializer();

// A map may or may not contain tilesets that are stored in external
// files. To deserialize a map, you must provide a way to resolve such
// tilesets.
Func<string, Tileset> externalTilesetResolver = (string path) => 
{
  string tilesetXml = fileSystem.ReadAllText(path);
  return tmxSerializer.DeserializeTileset(tilesetXml);
};

string mapXml = fileSystem.ReadAllText("path/to/map.tmx");
Map map = tmxSerializer.DeserializeMap(mapXml, externalTilesetResolver);
```

### Loading a `.tsx` tileset

```csharp
var tmxSerializer = new TmxSerializer();

string tilesetXml = fileSystem.ReadAllText("path/to/tileset.tsx");
Tileset tileset = tmxSerializer.DeserializeTileset(tilesetXml);
```