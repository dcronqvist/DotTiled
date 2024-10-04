using System.Reflection;
using DotTiled.Serialization;

namespace DotTiled.Example;

public class Program
{
  private static void Main(string[] args)
  {
    Quick(args[0]);
    Manual();
  }

  // QUICK START
  // Automatic and easy way to load tilemaps.
  private static void Quick(string basePath)
  {
    var tilemapPath = Path.Combine(basePath, "tilemap.tmx");

    var loader = Loader.Default();
    var map = loader.LoadMap(tilemapPath);

    // You can do stuff with it like...
    Console.WriteLine($"Tile width and height: {map.TileWidth}x{map.TileHeight}");
    TileLayer layer0 = (TileLayer)map.Layers[0]; // Get a layer
    Console.WriteLine($"Tile in layer 0 at 0, 0: {layer0.Data.Value.GlobalTileIDs.Value[0]}");
  }

  // MANUAL
  // Manually load a map, if you need to load from a custom source
  private static void Manual()
  {
    using Stream? tilemapStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DotTiled.Example.Console.tilemap.tmx")
      ?? throw new FileLoadException($"DotTiled.Example.Console.tilemap.tmx not found in assembly.");
    string tileMapString = new StreamReader(tilemapStream).ReadToEnd();
    using var mapReader = new MapReader(tileMapString, ResolveTileset, ResolveTemplate, ResolveCustomType);
    var map = mapReader.ReadMap();

    // Now do some other stuff with it...
    StringProperty hello = map.GetProperty<StringProperty>("hello");
    Console.WriteLine($"Layer 1 name: {map.Layers[0].Name}");
    Console.WriteLine($"Property 'hello': {hello.Value}");

    // Now with tileset
    Tileset tileset = map.Tilesets[0];
    Console.WriteLine($"Tileset 0 source: {tileset.Source.Value}");
    Console.WriteLine($"Tileset 0 image 0 source: {tileset.Image.Value.Source.Value}");
  }

  // This function is responsible for loading all tilesets required by a tilemap, if you
  // want to use a custom source.
  private static Tileset ResolveTileset(string source)
  {
    // Read a file from assembly
    // You can use any other source for files, eg. compressed archive, or even file from internet.
    using Stream? tilesetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DotTiled.Example.Console.{source}")
      ?? throw new FileLoadException($"{source} not found in assembly.");
    string tilesetString = new StreamReader(tilesetStream).ReadToEnd();

    using TilesetReader tilesetReader = new TilesetReader(tilesetString, ResolveTileset, ResolveTemplate, ResolveCustomType); // Parse loaded tileset.

    return tilesetReader.ReadTileset(); // Return loaded tileset
  }

  // This is pretty similar to above, but instead it loads templates, not tilesets.
  private static Template ResolveTemplate(string source)
  {
    using Stream? templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"DotTiled.Example.Console.{source}")
      ?? throw new FileLoadException($"{source} not found in assembly.");
    string templateString = new StreamReader(templateStream).ReadToEnd();

    using TemplateReader templateReader = new TemplateReader(templateString, ResolveTileset, ResolveTemplate, ResolveCustomType);
    return templateReader.ReadTemplate();
  }

  private static ICustomTypeDefinition ResolveCustomType(string name)
  {
    ICustomTypeDefinition[] allDefinedTypes =
    [
      new CustomClassDefinition() { Name = "a" },
    ];
    return allDefinedTypes.FirstOrDefault(type => type.Name == name) ?? throw new InvalidOperationException();
  }
}
