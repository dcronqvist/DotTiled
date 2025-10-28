using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotTiled.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DotTiled.Example.MonoGame;

/// <summary>
/// A beginner-friendly example of using DotTiled with MonoGame.
/// Loads a Tiled map, renders its layers, and allows basic player movement with collision.
/// </summary>
public class Game1 : Game
{
  private readonly GraphicsDeviceManager _graphics;
  private SpriteBatch _spriteBatch;

  // Camera for following the player
  private Camera2D _camera;

  // DotTiled map and tileset textures
  private Map _map;
  private Dictionary<string, Texture2D> _tilesetTextures;

  // Layers for collisions and points of interest
  private ObjectLayer _collisionLayer;
  private ObjectLayer _pointsOfInterestLayer;

  // Player state
  private Vector2 _playerPosition;
  private Texture2D _playerTexture;

  // Used for drawing rectangles (player)
  private Texture2D _whitePixel;

  public Game1()
  {
    _graphics = new GraphicsDeviceManager(this)
    {
      PreferredBackBufferWidth = 1280,
      PreferredBackBufferHeight = 720
    };
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
  }

  /// <summary>
  /// MonoGame initialization.
  /// </summary>
  protected override void Initialize() => base.Initialize();

  /// <summary>
  /// Load map, textures, and initialize player/camera.
  /// </summary>
  protected override void LoadContent()
  {
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    // Used for drawing rectangles (player)
    _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
    _whitePixel.SetData(new[] { Color.White });

    // Load the Tiled map using DotTiled
    var loader = Loader.Default();
    _map = loader.LoadMap(Path.Combine(Content.RootDirectory, "world.tmx"));

    // Load all tileset textures referenced by the map
    _tilesetTextures = LoadTilesetTextures(_map);

    // Extract layers for collisions and points of interest
    _collisionLayer = _map.Layers.OfType<ObjectLayer>().Single(l => l.Name == "Collisions");
    _pointsOfInterestLayer = (ObjectLayer)_map.Layers.Single(l => l.Name == "PointsOfInterest");

    // Get the player's spawn point from the PointsOfInterest layer
    var playerSpawn = _pointsOfInterestLayer.Objects.Single(obj => obj.Name == "PlayerSpawn");
    _playerPosition = new Vector2(playerSpawn.X, playerSpawn.Y);

    // Set up the camera to follow the player
    _camera = new Camera2D(GraphicsDevice.Viewport);

    // Optionally, create a simple player texture (blue rectangle)
    _playerTexture = new Texture2D(GraphicsDevice, 1, 1);
    _playerTexture.SetData(new[] { Color.Blue });
  }

  /// <summary>
  /// Loads all tileset textures referenced by the map.
  /// </summary>
  private Dictionary<string, Texture2D> LoadTilesetTextures(Map map)
  {
    // Remove ".png" for MonoGame Content Pipeline compatibility
    return map.Tilesets.ToDictionary(
        tileset => tileset.Image.Value.Source.Value,
        tileset => Content.Load<Texture2D>(Path.GetDirectoryName(tileset.Image.Value.Source.Value) + "/" + Path.GetFileNameWithoutExtension(tileset.Image.Value.Source.Value))
    );
  }

  /// <summary>
  /// Handles player input and updates game logic.
  /// </summary>
  protected override void Update(GameTime gameTime)
  {
    // Exit on Escape
    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();

    _camera.UpdateCamera(GraphicsDevice.Viewport);

    // Handle player movement input
    var move = HandlePlayerInput(gameTime);

    // Define the player's collision rectangle
    var playerRect = new Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 12, 12);

    // Collision detection with rectangles in the collision layer
    foreach (var obj in _collisionLayer.Objects.OfType<RectangleObject>())
    {
      var objRect = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);

      // Horizontal collision
      var movePlayerHRect = new Rectangle(playerRect.X + (int)move.X, playerRect.Y, playerRect.Width, playerRect.Height);
      if (move.X != 0 && movePlayerHRect.Intersects(objRect))
        move.X = 0;

      // Vertical collision
      var movePlayerVRect = new Rectangle(playerRect.X, playerRect.Y + (int)move.Y, playerRect.Width, playerRect.Height);
      if (move.Y != 0 && movePlayerVRect.Intersects(objRect))
        move.Y = 0;
    }

    // Update player position
    _playerPosition += move;

    // Smoothly update the camera to follow the player
    var newCameraTarget = new Vector2(_playerPosition.X, _playerPosition.Y);
    _camera.Position += (newCameraTarget - _camera.Position) * 15f * (float)gameTime.ElapsedGameTime.TotalSeconds;

    base.Update(gameTime);
  }

  /// <summary>
  /// Handles WASD input for player movement.
  /// </summary>
  private static Vector2 HandlePlayerInput(GameTime gameTime)
  {
    var move = Vector2.Zero;
    var speed = 150f * (float)gameTime.ElapsedGameTime.TotalSeconds;
    var state = Keyboard.GetState();

    if (state.IsKeyDown(Keys.W)) move.Y -= speed;
    if (state.IsKeyDown(Keys.S)) move.Y += speed;
    if (state.IsKeyDown(Keys.A)) move.X -= speed;
    if (state.IsKeyDown(Keys.D)) move.X += speed;

    return move;
  }

  /// <summary>
  /// Draws the map, player, and handles layer ordering.
  /// </summary>
  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(Color.Black);

    _spriteBatch.Begin(transformMatrix: _camera.Transform);

    // Get all visual tile layers from the "Visuals" group
    var visualLayers = _map.Layers.OfType<Group>().Single(l => l.Name == "Visuals").Layers.OfType<TileLayer>();

    // Render layers below the player
    RenderLayers(_map, visualLayers, _tilesetTextures, ["Ground", "Ponds", "Paths", "HouseWalls", "HouseDoors", "FencesBushes"]);

    // Draw the player as a blue rectangle (centered on tile)
    var playerVisualRect = new Rectangle((int)_playerPosition.X, (int)_playerPosition.Y - 12, 12, 24);
    _spriteBatch.Draw(_playerTexture, playerVisualRect, Color.White);

    // Render layers above the player
    RenderLayers(_map, visualLayers, _tilesetTextures, ["HouseRoofs"]);

    _spriteBatch.End();

    base.Draw(gameTime);
  }

  /// <summary>
  /// Renders specific named layers from the map.
  /// </summary>
  private void RenderLayers(Map map, IEnumerable<TileLayer> visualLayers, Dictionary<string, Texture2D> tilesetTextures, string[] layerNames)
  {
    foreach (var layerName in layerNames)
    {
      var layer = visualLayers.Single(l => l.Name == layerName);
      RenderLayer(map, layer, tilesetTextures);
    }
  }

  /// <summary>
  /// Renders a single tile layer.
  /// </summary>
  private void RenderLayer(Map map, TileLayer layer, Dictionary<string, Texture2D> tilesetTextures)
  {
    for (var y = 0; y < layer.Height; y++)
    {
      for (var x = 0; x < layer.Width; x++)
      {
        var tileGID = layer.GetGlobalTileIDAtCoord(x, y);
        if (tileGID == 0) continue;

        var tileset = map.ResolveTilesetForGlobalTileID(tileGID, out var localTileID);
        var sourceRect = tileset.GetSourceRectangleForLocalTileID(localTileID);

        var destination = new Vector2(x * tileset.TileWidth, y * tileset.TileHeight);
        var sourceRectangle = new Rectangle(sourceRect.X + 1, sourceRect.Y + 1, sourceRect.Width - 2, sourceRect.Height - 2);

        _spriteBatch.Draw(
            tilesetTextures[tileset.Image.Value.Source.Value],
            destination,
            sourceRectangle,
            Color.White,
            0f,
            Vector2.Zero,
            Vector2.One * (1f / (14f / 16f)), // Shrink by a tiny amount to avoid seams (not a perfect solution)
            SpriteEffects.None,
            0f
        );
      }
    }
  }
}
