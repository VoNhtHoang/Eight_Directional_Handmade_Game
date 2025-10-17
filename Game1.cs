using System;
using System.Data;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Knight {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;    
        private SpriteBatch _spriteBatch;

        private TileMap _map;

        private string _mapPath;
        private TiledMapRenderer _mapRenderer;
        private Player _player;
        private Camera2D _camera;

        public Game1() {
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
        protected override void LoadContent() {
            try
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);

                // _map = Content.Load<TiledMap>("map/map.tmx");
                _map = new TileMap(Content, GraphicsDevice, _mapPath);

                // Texture2D playerTexture = Content.Load<Texture2D>("player");
                _player = new Player(Content, new Vector2(200, 200));

                // Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice = game.GraphicsDevice;
                // Microsoft.Xna.Framework.Graphics.Viewport viewport = graphicsDevice.Viewport;


                // Sử dụng thông tin trong viewport theo cách bạn cần
                _camera = new Camera2D(GraphicsDevice);
            } catch(Exception ex)
            {
                Console.WriteLine("Lỗi LoadContent: " + ex.Message);
                Exit();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);

            _camera.LookAt(_player.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _map.Update(gameTime);
            _map.Draw(_camera.GetViewMatrix());

            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            _player.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
