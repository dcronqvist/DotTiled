# Loading maps

Loading maps with DotTiled is straightforward and easy. The <xref:DotTiled.Model.Map> class is a representation of a Tiled map, mimicking the structure of a Tiled map file. Map files can either be in the [`.tmx`/XML](https://doc.mapeditor.org/en/stable/reference/tmx-map-format/) or [`.tmj`/json](https://doc.mapeditor.org/en/stable/reference/json-map-format/) format. DotTiled supports **both** formats fully.

> [!TIP]
> Using the `.tmj` file format will result in <xref:DotTiled.Model.ImageLayer.Image> not having the same amount of information as for `.tmx` format. This is due to the fact that the `.tmj` format does not include the full information that the `.tmx` format does. This is not a problem with DotTiled, but rather a limitation of the `.tmj` format.

## External resolution

Tiled maps may consist of several external files, such as tilesets and object templates. In Tiled map files, these are typically referenced by their path relative to the map file. It would be annoying to have to first load all these external resources before loading a map, so loading a map with DotTiled is designed in a way that you only have to provide a function that resolves these external resources. This way, DotTiled will figure out which external resources are needed and will invoke the corresponding resolver function to load them.

Loading a map, tileset, or template will require you to specify **three** resolver functions

### `Func<string, Tileset>` - Tileset resolver

This function is used to resolve external tilesets by their source path. The function should return a <xref:DotTiled.Model.Tileset> instance given the source path of the tileset. If you just want to load tilesets from the file system, you can use something like this:

```csharp
Tileset ResolveTileset(string source)
{
    
}
```

- `Func<string, Template>` - Template resolver, which resolves a template by its source path
- `Func<string, CustomType>` - Custom type resolver, which resolves a custom type by its name

The first two resolver functions are used to resolve external tilesets and object templates. The third resolver function is used to resolve custom types that are defined in Tiled maps. Please refer to the [custom properties](custom-properties.md) documentation for more information on custom types.


-------------
 
### [OLD]

To be completely agnostic about how you may store/retrieve your tilesets, DotTiled will invoke a supplied `Func<string, Tileset>` where you can resolve the tileset by its source path. This is useful for scenarios where you may want to load tilesets in custom ways, such as from a database or a custom file format.

```csharp
Tileset ResolveTileset(string source)
{
  var xmlStringReader = CustomContentManager.ReadString(source);
  var xmlReader = XmlReader.Create(xmlStringReader);
  var tilesetReader = new TmxTilesetReader(xmlReader, ResolveTileset, ResolveTemplate, ResolveCustomType);
}