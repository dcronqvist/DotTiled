using System.Globalization;
using System.Linq;
using DotTiled.Serialization;
using Godot;

namespace DotTiled.Example.Godot;
public partial class MapParser : Node2D
{
  public override void _Ready()
  {
    // Load map
    var mapString = FileAccess.Open("res://tilemap.tmx", FileAccess.ModeFlags.Read).GetAsText(); //Get file from Godot filesystem
    using var mapReader = new MapReader(mapString, ResolveTileset, ResolveTemplate, ResolveCustomType);
    var map = mapReader.ReadMap();

    TileLayer layer0 = (TileLayer)map.Layers[0];

    for (int y = 0; y < layer0.Height; y++)
    {
      for (int x = 0; x < layer0.Width; x++)
      {
        uint tile = layer0.Data.Value.GlobalTileIDs.Value[(y * layer0.Width) + x];
        if (tile == 0) continue; // If block is 0, i.e. air, then continue

        // Load actual block from Godot resources
        Node2D block = (Node2D)GD.Load<PackedScene>($"res://blocks/{tile}.tscn").Instantiate();

        // Calculate where block should be
        Vector2I scale = (Vector2I)block.GetNode<Sprite2D>(tile.ToString(CultureInfo.CurrentCulture)).Scale;
        int blockX = (block.GetNode<Sprite2D>(tile.ToString(CultureInfo.CurrentCulture)).Texture.GetWidth() * scale.X / 2) +
                     (x * block.GetNode<Sprite2D>(tile.ToString(CultureInfo.CurrentCulture)).Texture.GetWidth() * scale.X);
        int blockY = (block.GetNode<Sprite2D>(tile.ToString(CultureInfo.CurrentCulture)).Texture.GetHeight() * scale.Y / 2) +
                     (y * block.GetNode<Sprite2D>(tile.ToString(CultureInfo.CurrentCulture)).Texture.GetHeight() * scale.Y);
        block.Position = new Vector2(blockX, blockY);

        // Add block to current scene
        AddChild(block);
        GD.Print($"{blockX}, {blockY}: {tile}");
      }
    }
  }

  private Tileset ResolveTileset(string source)
  {
      string tilesetString = FileAccess.Open($"res://{source}", FileAccess.ModeFlags.Read).GetAsText();
      using TilesetReader tilesetReader =
        new TilesetReader(tilesetString, ResolveTileset, ResolveTemplate, ResolveCustomType);
      return tilesetReader.ReadTileset();
  }

  private Template ResolveTemplate(string source)
  {
    string templateString = FileAccess.Open($"res://{source}", FileAccess.ModeFlags.Read).GetAsText();
    using TemplateReader templateReader =
      new TemplateReader(templateString, ResolveTileset, ResolveTemplate, ResolveCustomType);
    return templateReader.ReadTemplate();
  }

  private static Optional<ICustomTypeDefinition> ResolveCustomType(string name)
  {
    ICustomTypeDefinition[] allDefinedTypes =
      [
        new CustomClassDefinition() { Name = "a" },
      ];
    return allDefinedTypes.FirstOrDefault(ctd => ctd.Name == name) is ICustomTypeDefinition ctd ? new Optional<ICustomTypeDefinition>(ctd) : Optional<ICustomTypeDefinition>.Empty;
  }
}
