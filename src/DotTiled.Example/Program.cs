using DotTiled.Serialization;

namespace DotTiled.Example;

class Program
{
  static void Main(string[] args)
  {
    Quick();
    Manual();
  }

  // QUICK START
  // Automatic and easy way to load tilemaps.
  static void Quick()
  {
    var loader = Loader.Default();
    var map = loader.LoadMap("tilemap.tmx");

    // You can do stuff with it like...
    Console.WriteLine($"Tile width and height: {map.TileWidth}x{map.TileHeight}");
    TileLayer layer0 = (TileLayer)map.Layers[0]; // Get a layer
    Console.WriteLine($"Tile in layer 0 at 0, 0: {layer0.Data.Value.GlobalTileIDs.Value[0]}");
  }

  // MANUAL
  // Manually load a map, if you need to load from a custom source
  static void Manual()
  {
    using var mapFileReader = new StreamReader("tilemap.tmx");
    var mapString = mapFileReader.ReadToEnd();
    using var mapReader = new MapReader(mapString, ResolveTileset, ResolveTemplate, ResolveCustomType);
    var map = mapReader.ReadMap();

    // Now do some other stuff with it...
    StringProperty hello = (StringProperty)map.Properties.FirstOrDefault(property => property.Name == "hello");
    Console.WriteLine($"Layer 1 name: {map.Layers[0].Name}");
    Console.WriteLine($"Property 'hello': {hello.Value}");

    // Now with tileset
    Tileset tileset = map.Tilesets[0];
    Console.WriteLine($"Tileset 0 source: {tileset.Source.Value}");
    Console.WriteLine($"Tileset 0 image 0 source: {tileset.Image.Value.Source.Value}");
  }

  /* This function is responsible for loading all tilesets required by a tilemap, if you
   want to use a custom source. */
  static Tileset ResolveTileset(string source)
  {
    string tilesetPath = Path.Combine(Directory.GetCurrentDirectory(), source); // Resolve path to a tileset.
    using var tilesetFileReader = new StreamReader(tilesetPath); // Read tileset file itself.
    var tilesetString = tilesetFileReader.ReadToEnd();           // You can replace this with any custom function
                                                                 // to load data from any source, eg. .zip file.
    using var tilesetReader = new TilesetReader(tilesetString, ResolveTileset, ResolveTemplate, ResolveCustomType); // Parse loaded tileset.

    return tilesetReader.ReadTileset(); // Return loaded tileset
  }

  // This is pretty similar to above, but instead it loads templates, not tilesets.
  static Template ResolveTemplate(string source)
  {
    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), source);
    using var templateFileReader = new StreamReader(templatePath);
    var templateString = templateFileReader.ReadToEnd();
    using var templateReader = new TemplateReader(templateString, ResolveTileset, ResolveTemplate, ResolveCustomType);
    return templateReader.ReadTemplate();
  }

  static ICustomTypeDefinition ResolveCustomType(string name)
  {
    CustomClassDefinition[] allDefinedTypes =
      [
        new CustomClassDefinition() { Name = "a" },
      ];
    return allDefinedTypes.FirstOrDefault(type => type.Name == name);
  }

}
