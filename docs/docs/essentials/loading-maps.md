# Loading maps

Loading maps with DotTiled is straightforward and easy. The <xref:DotTiled.Model.Map> class is a representation of a Tiled map, mimicking the structure of a Tiled map file. Map files can either be in the [`.tmx`/XML](https://doc.mapeditor.org/en/stable/reference/tmx-map-format/) or [`.tmj`/json](https://doc.mapeditor.org/en/stable/reference/json-map-format/) format. DotTiled supports **both** formats fully.

> [!NOTE]
> Using the `.tmj` file format will result in <xref:DotTiled.Model.ImageLayer.Image> not having the same amount of information as for the `.tmx` format. This is due to the fact that the `.tmj` format does not include the full information that the `.tmx` format does. This is not a problem with DotTiled, but rather a limitation of the `.tmj` format.

## External resolution

Tiled maps may consist of several external files, such as tilesets or object templates. In Tiled map files, they are typically referenced by their path relative to the map file. It would be annoying to have to first load all these external resources before loading a map (which is how some other similar libraries work), so loading a map with DotTiled is designed in a way that you only have to provide a function that resolves these external resources. This way, DotTiled will figure out which external resources are needed and will invoke the corresponding resolver function to load them.

Loading a map, tileset, or template will require you to specify **three** resolver functions. We'll go through each of them below.

### `Func<string, Tileset>` - Tileset resolver

This function is used to resolve external tilesets by their source path. The function should return a <xref:DotTiled.Model.Tileset> instance given the source path of the tileset. If you just want to load tilesets from the file system, you can use something like this:

```csharp
Tileset ResolveTileset(string source)
{
  using var tilesetFileReader = new StreamReader(source);
  var tilesetString = tilesetReader.ReadToEnd();
  using var tilesetReader = new TilesetReader(tilesetString, ResolveTileset, ResolveTemplate, ResolveCustomType);
  return tilesetReader.ReadTileset();
}
```

But, DotTiled is designed this way so you can retrieve your external resources from anywhere, such as a database or a custom file format, by implementing your own resolver function however you like. If you have some other means of accessing resources, you can use that instead of the file system.

```csharp
Tileset ResolveTileset(string source)
{
  var tilesetString = ContentManager.GetString($"tilesets/{source}");
  using var tilesetReader = new TilesetReader(tilesetString, ResolveTileset, ResolveTemplate, ResolveCustomType);
  return tilesetReader.ReadTileset();
}
```

### `Func<string, Template>` - Template resolver

This function is used to resolve external object templates by their source path. The function should return a <xref:DotTiled.Model.Template> instance given the source path of the template. If you just want to load templates from the file system, you can use something very similar to the tileset resolver by replacing <xref:DotTiled.Serialization.TilesetReader> with <xref:DotTiled.Serialization.TemplateReader>.

### `Func<string, CustomType>` - Custom type resolver

This function is used to resolve custom types that are defined in Tiled maps. Please refer to the [custom properties](custom-properties.md) documentation for more information on custom types. The function should return a <xref:DotTiled.Model.ICustomTypeDefinition> instance given the custom type's name.

## Putting it all together

The following classes are the readers that you will need to use to read the map, tileset, and template: <xref:DotTiled.Serialization.MapReader>, <xref:DotTiled.Serialization.TilesetReader>, and <xref:DotTiled.Serialization.TemplateReader>.

Here is an example of how you can load a map with DotTiled:

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