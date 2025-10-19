// using System.Numerics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Serilog;

namespace Knight;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Player
    private string[] playerSheetPaths = ["character/16x32 Idle", "character/16x32 Walk Cycle", "character/16x32 Run Cycle-Sheet"];
    private string mapPath = "map/map";
    private Player _player;
    private TiledMapManager _map;

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

            _map = new TiledMapManager(Content, GraphicsDevice, mapPath);

            _player = new Player(Content, playerSheetPaths, new Vector2(100, 100));

            _camera = new Camera(GraphicsDevice);

        }catch (Exception ex) {
            Log.Information("File Game1! Load Content Error: "+ ex.Message +"\t" + ex.Data.ToString());
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
            Log.Information(    "File Game1! Update Error : " + ex.Message + "\t" + ex.Data.ToString());
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        // GraphicsDevice.Clear(Color.CornflowerBlue);
        try
        {
            GraphicsDevice.Clear(Color.White);

            // _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            // Zoom + Draw
            _spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
            
            _map.Draw(_spriteBatch, gameTime, _camera.Transform);
            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);           
        } catch (Exception ex){


            Log.Information("File Game 1! Draw Error : " + ex.Message +"\t" + ex.Data.ToString());
        }
        // TODO: Add your drawing code here

        
    }
}
