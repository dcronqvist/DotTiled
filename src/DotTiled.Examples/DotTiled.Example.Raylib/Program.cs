using System.Numerics;
using DotTiled.Serialization;
using Raylib_cs;

using RayColor = Raylib_cs.Color;

namespace DotTiled.Example
{
  public class Program
  {
    public static void Main(string[] _)
    {
      // Initialize the Raylib window
      Raylib.InitWindow(1280, 720, "DotTiled Example with Raylib");
      Raylib.SetConfigFlags(ConfigFlags.VSyncHint);

      // Load the Tiled map
      var loader = Loader.Default();
      var map = loader.LoadMap("assets/world.tmx");

      // Load tileset textures
      var tilesetTextures = LoadTilesetTextures(map);

      // Extract layers from the map
      var visualLayers = map.Layers.OfType<Group>().Single(l => l.Name == "Visuals").Layers.OfType<TileLayer>();
      var collisionLayer = map.Layers.OfType<ObjectLayer>().Single(l => l.Name == "Collisions");
      var pointsOfInterest = (ObjectLayer)map.Layers.Single(layer => layer.Name == "PointsOfInterest");

      // Get the player's spawn point
      var playerSpawnPoint = pointsOfInterest.Objects.Single(obj => obj.Name == "PlayerSpawn");
      var playerPosition = new Vector2(playerSpawnPoint.X, playerSpawnPoint.Y);

      // Set up the camera
      var camera = new Camera2D
      {
        Target = playerPosition,
        Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
        Rotation = 0.0f,
        Zoom = 1.0f
      };

      // Main game loop
      while (!Raylib.WindowShouldClose())
      {
        // Update game logic
        Update(ref playerPosition, collisionLayer, ref camera);

        // Render the game
        Render(map, visualLayers, tilesetTextures, playerPosition, camera);
      }

      // Clean up resources
      Raylib.CloseWindow();
    }

    /// <summary>
    /// Loads tileset textures from the map.
    /// </summary>
    private static Dictionary<string, Texture2D> LoadTilesetTextures(Map map)
    {
      return map.Tilesets.ToDictionary(
          tileset => tileset.Image.Value.Source.Value,
          tileset => Raylib.LoadTexture(Path.Combine("assets", tileset.Image.Value.Source.Value))
      );
    }

    /// <summary>
    /// Updates the player's position and camera.
    /// </summary>
    private static void Update(ref Vector2 playerPosition, ObjectLayer collisionLayer, ref Camera2D camera)
    {
      // Define the player's rectangle
      var playerRect = new Rectangle(playerPosition.X, playerPosition.Y, 12, 12);

      // Handle player movement
      var move = HandlePlayerInput();

      // Check for collisions
      foreach (var obj in collisionLayer.Objects.OfType<RectangleObject>())
      {
        var objRect = new Rectangle(obj.X, obj.Y, obj.Width, obj.Height);

        // Horizontal collision
        var movePlayerHRect = new Rectangle(playerRect.X + move.X, playerRect.Y, playerRect.Width, playerRect.Height);
        if (Raylib.CheckCollisionRecs(movePlayerHRect, objRect))
        {
          move.X = 0;
        }

        // Vertical collision
        var movePlayerVRect = new Rectangle(playerRect.X, playerRect.Y + move.Y, playerRect.Width, playerRect.Height);
        if (Raylib.CheckCollisionRecs(movePlayerVRect, objRect))
        {
          move.Y = 0;
        }
      }

      // Update player position
      playerPosition += move;

      // Smoothly update the camera target
      var newCameraTarget = new Vector2(playerPosition.X, playerPosition.Y);
      camera.Target += (newCameraTarget - camera.Target) * 15f * Raylib.GetFrameTime();
    }

    /// <summary>
    /// Handles player input for movement.
    /// </summary>
    private static Vector2 HandlePlayerInput()
    {
      var move = Vector2.Zero;
      var playerSpeed = 150 * Raylib.GetFrameTime();

      if (Raylib.IsKeyDown(KeyboardKey.W)) move.Y -= playerSpeed;
      if (Raylib.IsKeyDown(KeyboardKey.S)) move.Y += playerSpeed;
      if (Raylib.IsKeyDown(KeyboardKey.A)) move.X -= playerSpeed;
      if (Raylib.IsKeyDown(KeyboardKey.D)) move.X += playerSpeed;

      return move;
    }

    /// <summary>
    /// Renders the game, including layers and the player.
    /// </summary>
    private static void Render(Map map, IEnumerable<TileLayer> visualLayers, Dictionary<string, Texture2D> tilesetTextures, Vector2 playerPosition, Camera2D camera)
    {
      Raylib.BeginDrawing();
      Raylib.ClearBackground(RayColor.Blank);
      Raylib.BeginMode2D(camera);

      // Render layers below the player
      RenderLayers(map, visualLayers, tilesetTextures, ["Ground", "Ponds", "Paths", "HouseWalls", "HouseDoors", "FencesBushes"]);

      // Draw the player
      var playerVisualRect = new Rectangle(playerPosition.X, playerPosition.Y - 12, 12, 24);
      Raylib.DrawRectangleRec(playerVisualRect, RayColor.Blue);

      // Render layers above the player
      RenderLayers(map, visualLayers, tilesetTextures, ["HouseRoofs"]);

      Raylib.EndMode2D();
      Raylib.EndDrawing();
    }

    /// <summary>
    /// Renders specific layers from the map.
    /// </summary>
    private static void RenderLayers(Map map, IEnumerable<TileLayer> visualLayers, Dictionary<string, Texture2D> tilesetTextures, string[] layerNames)
    {
      foreach (var layerName in layerNames)
      {
        var layer = visualLayers.OfType<TileLayer>().Single(l => l.Name == layerName);
        RenderLayer(map, layer, tilesetTextures);
      }
    }

    /// <summary>
    /// Renders a single layer from the map.
    /// </summary>
    private static void RenderLayer(Map map, TileLayer layer, Dictionary<string, Texture2D> tilesetTextures)
    {
      for (var y = 0; y < layer.Height; y++)
      {
        for (var x = 0; x < layer.Width; x++)
        {
          var tileGID = layer.GetGlobalTileIDAtCoord(x, y);
          if (tileGID == 0) continue;

          var tileset = map.ResolveTilesetForGlobalTileID(tileGID, out var localTileID);
          var sourceRect = tileset.GetSourceRectangleForLocalTileID(localTileID);

          // Source rec is shrunk by tiny amount to avoid ugly seams between tiles
          // when the camera is at certain subpixel positions
          var raylibSourceRect = ShrinkRectangle(new Rectangle(sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height), 0.01f);

          var destinationRect = new Rectangle(x * tileset.TileWidth, y * tileset.TileHeight, tileset.TileWidth, tileset.TileHeight);

          Raylib.DrawTexturePro(
              tilesetTextures[tileset.Image.Value.Source.Value],
              raylibSourceRect,
              destinationRect,
              Vector2.Zero,
              0,
              RayColor.White
          );
        }
      }
    }

    /// <summary>
    /// Shrinks a rectangle by a specified amount.
    /// </summary>
    private static Rectangle ShrinkRectangle(Rectangle rect, float amount)
    {
      return new Rectangle(
          rect.X + amount,
          rect.Y + amount,
          rect.Width - (2 * amount),
          rect.Height - (2 * amount)
      );
    }
  }
}
