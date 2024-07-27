# 📚 DotTiled

DotTiled is a simple, lightweight, and easy-to-use library for loading, saving, and managing Tiled maps and tilesets in your .NET projects. After [TiledCS](https://github.com/TheBoneJarmer/TiledCS) unfortunately became unmaintained (since 2022), I aimed to create a new library that could fill its shoes. DotTiled is the result of that effort.

## Quickstart

### Loading a map with external tilesets

```csharp
using DotTiled;

TmxSerializer tmxSerializer;

// Tiled can store tilesets in external files or in a map itself
// When they are stored externally, you need to provide a way 
// to resolve them given their source path
Func<string, Tileset> externalTilesetResolver = (string path) => 
{
  // Load the tileset from however you want
  var tilesetXml = fileSystem.ReadAllText(path);
  var xmlStringReader = new StringReader(tilesetXml);  
  var xmlReader = XmlReader.Create(xmlStringReader);
  var tmxSerializer = tmxSerializer.DeserializeTileset(xmlReader);
};

```