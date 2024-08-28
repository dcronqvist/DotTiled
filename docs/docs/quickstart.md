# Quick Start

Install DotTiled from NuGet:

```bash
dotnet add package DotTiled
```

Load a map from file system:

```csharp
string mapPath = "path/to/map.tmx";
string mapDirectory = Path.GetDirectoryName(mapPath);

Tileset ResolveTileset(string source)
{
  string tilesetPath = Path.Combine(mapDirectory, source);
  using var tilesetFileReader = new StreamReader(tilesetPath);
  var tilesetString = tilesetReader.ReadToEnd();
  using var tilesetReader = new TilesetReader(tilesetString, ResolveTileset, ResolveTemplate, ResolveCustomType);
  return tilesetReader.ReadTileset();
}

Template ResolveTemplate(string source)
{
  string templatePath = Path.Combine(mapDirectory, source);
  using var templateFileReader = new StreamReader(templatePath);
  var templateString = templateReader.ReadToEnd();
  using var templateReader = new TemplateReader(templateString, ResolveTileset, ResolveTemplate, ResolveCustomType);
  return templateReader.ReadTemplate();
}

ICustomTypeDefinition ResolveCustomType(string name)
{
  var allDefinedTypes = [ ... ];
  return allDefinedTypes.FirstOrDefault(type => type.Name == name);
}

using var mapFileReader = new StreamReader(mapPath);
var mapString = mapFileReader.ReadToEnd();
using var mapReader = new MapReader(mapString, ResolveTileset, ResolveTemplate, ResolveCustomType);

var map = mapReader.ReadMap();
```

If the above looks intimidating, don't worry! DotTiled is designed to be flexible and allow you to load maps from any source, such as a database or a custom file format. The above example is just one way to load a map from a file system. Please look at [Loading Maps](essentials/loading-maps.md) for more information on how to load maps from different sources.