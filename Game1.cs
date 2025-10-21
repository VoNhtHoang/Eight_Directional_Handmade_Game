// using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;

using MonoGame.Extended.Tiled.Renderers;
//
using Serilog;

//
using System.Linq;
using MonoGame.Extended.Collisions.Layers;


namespace Knight;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Player
    private string[] playerSheetPaths = ["character/16x32 Idle", "character/16x32 Walk Cycle", "character/16x32 Run Cycle-Sheet"];
    private string mapPath = "map/map";
    private Player _player;

    private ManualTileMap _map;
    // private TiledMapManager _map;
    // private List<TiledMapTileLayer> _map_layers;
    // private TiledMapTileLayer _groundLayer;
    // private List<TiledMapTileLayer> _depthLayers;

    private List<(float y, Action<SpriteBatch> )> _drawList;
    private Camera _camera;

    // private CollisionManager _collisionManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        try
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Y Sorting
            _drawList = new();
            // ManualTileMap 
            _map = new ManualTileMap(Content, GraphicsDevice, mapPath);
            _player = new Player(Content, playerSheetPaths, new Vector2(100, 100));
            _camera = new Camera(GraphicsDevice);


            foreach (var layer in _map._map.TileLayers)
            {
                Log.Information("Game1 File - Layer load: " + layer.Name);
                float depth = _map.GetDrawDepth(layer);
                _drawList.Add((depth, spriteBatch => _map.Draw(_map._map, spriteBatch, layer, depth)));
            }

            _drawList.Add((0.3f, spriteBatch => _player.Draw(spriteBatch, 0.3f)));

            
            // TiledMapRenderer 
            // _map = new TiledMapManager(Content, GraphicsDevice, mapPath);
            // _map_layers = new();
            // _map_layers = _map._getAllTileLayer();
            // _map_layers = _map_layers.OrderBy(layer => Convert.ToSingle(layer.Properties["drawDepth"])).ToList();

            // Y Sorting
            // _groundLayer = _map._getGroundLayer();
            // _depthLayers = _map.getDepthLayers();

        }
        catch (Exception ex)
        {
            Log.Information("File Game1! Load Content Error: " + ex.Message + "\t" + ex.Data.ToString());
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        try
        {
            _map.Update(gameTime);

            _player.Update(gameTime, _map);

            _camera.Follow(_player.Bounds.Center, gameTime, _map._mapWidth, _map._mapHeight);

            base.Update(gameTime);
        }
        catch (Exception ex)
        {
            Log.Information("File Game1! Update Error : " + ex.Message + "\t" + ex.Data.ToString());
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        // GraphicsDevice.Clear(Color.CornflowerBlue);
        try
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(
                SpriteSortMode.FrontToBack,       // hoặc FrontToBack lớn hơn sẽ vẽ sau
                BlendState.AlphaBlend,
                SamplerState.PointClamp
            // DepthStencilState.None,   // DepthStencilState.Default, nếu có dùng GPU để buffer test
            // RasterizerState.CullNone,
            // null,
            // transformMatrix: _camera.Transform
            );
            
            // ManuallTileMap
            foreach (var (_, draw) in _drawList.OrderBy(item => item.y))
            {
                draw(_spriteBatch);
            }


            // TileMapManager
            // foreach (TiledMapTileLayer layer in _map_layers)
            //     if (Convert.ToSingle(layer.Properties["drawDepth"]) < 0.3f)
            //         _map.Draw(_spriteBatch, gameTime, layer, _camera.Transform);

            // _player.Draw(_spriteBatch, 0.3f);

            // foreach (TiledMapTileLayer layer in _map_layers)
            //     if (Convert.ToSingle(layer.Properties["drawDepth"]) > 0.3f)
            //     {
            //         Log.Information("Game1 File - " + layer.Name.ToString() + "\t Depth: " + Convert.ToSingle(layer.Properties["drawDepth"]));
            //         _map.Draw(_spriteBatch, gameTime, layer, _camera.Transform);
            //     }

            // GraphicsDevice.DepthStencilState = DepthStencilState.None;    

            _spriteBatch.End();
            base.Draw(gameTime);
        }
        catch (Exception ex)
        {
            Log.Information("File Game 1! Draw Error : " + ex.Message + "\t" + ex.Data.ToString());
        }

    }

    
}
