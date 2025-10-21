
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Content;
// using MonoGame.Extended.Tiled;
// using MonoGame.Extended.Tiled.Renderers;
// using Microsoft.Xna.Framework;

// namespace Knight
// {
//     public class TileMap
//     {
//         public TiledMap _map { get; private set;}
//         private TiledMapRenderer _renderer;
//         public Vector2 MapSize { get; private set; }

//         public TileMap(ContentManager contentManager, GraphicsDevice graphicsDevice, string mapFile)
//         {
//             _map = contentManager.Load<TiledMap>(mapFile);
//             _renderer = new TiledMapRenderer(graphicsDevice, _map);

//              MapSize = new Vector2(
//                 _map.WidthInPixels,
//                 _map.HeightInPixels
//             );
//         }

//         public void Update(GameTime gameTime) => _renderer.Update(gameTime);
//         public void Draw(Matrix viewMatrix) => _renderer.Draw(viewMatrix);
//     }
// }


using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

using Serilog;

using System.Collections.Generic;
using System;
using MonoGame.Extended.Serialization.Json;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended.Timers;
using System.Numerics;

// using System.Drawing;

namespace Knight
{
    public class TiledMapManager
    {
        public MonoGame.Extended.Tiled.TiledMap _map { get; private set; }
        public TiledMapRenderer _renderer;

        public List<MonoGame.Extended.RectangleF> _collisionRects { get; private set; }

        public List<Microsoft.Xna.Framework.Vector2> _playerDesiredCorners;

        public int _mapWidth { get; private set; }
        public int _mapHeight { get; private set; }
        private readonly string[] _depthLayers = { "Tree", "obj-ground", "wall_cliff" };

        public TiledMapManager(ContentManager content, GraphicsDevice graphicsDevice, string mapPath)
        {
            _map = content.Load<TiledMap>(mapPath);
            _renderer = new TiledMapRenderer(graphicsDevice, _map);
            // MapSize
            _mapWidth = _map.WidthInPixels;
            _mapHeight = _map.HeightInPixels;

            // Collision Tile Layer if enable
            _playerDesiredCorners = new List<Microsoft.Xna.Framework.Vector2>();

            // Collision with Objects 
            _collisionRects = new List<MonoGame.Extended.RectangleF>();
            LoadCollisionLayer();
        }

        public TiledMapTileLayer _getGroundLayer()
        {
            return _map.GetLayer<TiledMapTileLayer>("ground");
        }

        public List<TiledMapTileLayer> getDepthLayers()
        {
            List<TiledMapTileLayer> depthLayers = new List<TiledMapTileLayer>();
            foreach (string layerName in _depthLayers)
            {
                if (_map.GetLayer<TiledMapTileLayer>(layerName) == null) continue;
                depthLayers.Add(_map.GetLayer<TiledMapTileLayer>(layerName));
            }

            return depthLayers;
        }

        public TiledMapTileLayer _getCollisionTileLayer()
        {
            TiledMapTileLayer layer = _map.GetLayer<TiledMapTileLayer>("collision");
            return layer;
        }

        private void LoadCollisionLayer()
        {
            TiledMapObjectLayer layer = _map.GetLayer<TiledMapObjectLayer>("collision");

            if (layer == null) {
                Log.Information("File Tiled Map Manager ---- Layer is NULL");
                return; 
            }

            foreach (var obj in layer.Objects)
            {
                if (obj is TiledMapObject { IsVisible: true })
                    _collisionRects.Add(new MonoGame.Extended.RectangleF(obj.Position.X, obj.Position.Y, obj.Size.Width, obj.Size.Height));
            }
            Log.Information("File Tiled Map Manager ---- collision Rects count"+ _collisionRects.Count.ToString());
        }

        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix? cameraTransform = null)
        {
            // for (int i = 0; i < _collisionRects.Count; i++)
            // {
            //     MonoGame.Extended.RectangleF rect = _collisionRects[i];
            //     spriteBatch.DrawRectangle(rect, Color.Red * 1f);
            // }

            // Log.Warning("Tiled Map Manager - Draw Rect! \t" + _collisionRects.Count.ToString());

            // Zoom & Draw
            if (cameraTransform.HasValue)
                _renderer.Draw(cameraTransform.Value);
            else
                _renderer.Draw();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, TiledMapTileLayer layer, Matrix? cameraTransform = null)
        {
            for (int i = 0; i < _collisionRects.Count; i++)
            {
                MonoGame.Extended.RectangleF rect = _collisionRects[i];
                spriteBatch.DrawRectangle(rect, Color.Red * 1f);
            }
            // Log.Warning("Tiled Map Manager - Draw Rect! \t" + _collisionRects.Count.ToString());

            // Zoom & Draw
            if (cameraTransform.HasValue)
                _renderer.Draw(layer, cameraTransform.Value);
            else
                _renderer.Draw(layer);
        }
        void DrawRectangle(SpriteBatch spriteBatch, MonoGame.Extended.RectangleF rect, Color color)
        {
            // Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            // pixel.SetData(new[] { Color.White });
            // spriteBatch.Draw(pixel, rect, color);

            spriteBatch.DrawRectangle(rect, color);
        }

        // public bool CheckCollision(MonoGame.Extended.RectangleF rect)
        // {
        //     foreach (var c in _collisionRects)
        //     {
        //         if (rect.Intersects(c))
        //             return true;
        //     }
        //     return false;
        // }

        public Microsoft.Xna.Framework.Vector2 ResolveCollision(RectangleF playerRect, Microsoft.Xna.Framework.Vector2 velocity)
        {
            var nextRect = new RectangleF(playerRect.Position + velocity, playerRect.Size);

            foreach (var rect in _collisionRects)
            {
                if (nextRect.Intersects(rect))
                {
                    // Tách trục X
                    var xRect = new RectangleF(playerRect.Position + new Microsoft.Xna.Framework.Vector2(velocity.X, 0), playerRect.Size);
                    if (!xRect.Intersects(rect))
                        velocity.Y = 0;
                    else
                        velocity.X = 0;
                    // var futureRect = new MonoGame.Extended.RectangleF(playerRect.Position + new Vector2(velocity.X, 0), playerRect.Size);
                    // if (!futureRect.Intersects(rect)) velocity.X = 0;
                    // futureRect = new MonoGame.Extended.RectangleF(playerRect.Position + new Vector2(0, velocity.Y), playerRect.Size);
                    // if (!futureRect.Intersects(rect)) velocity.Y = 0;
                }
            }
            Log.Information("Map - Come to ResolveColl");
            return velocity;
        }
        
        public Microsoft.Xna.Framework.Vector2 ResolveCollision(Player player, Microsoft.Xna.Framework.Vector2 velocity)
        {
            TiledMapTileLayer layer = this._getCollisionTileLayer();
            if (layer == null) return velocity;

            _playerDesiredCorners = new List<Microsoft.Xna.Framework.Vector2>();
            _playerDesiredCorners.Add(new Microsoft.Xna.Framework.Vector2(player.Bounds.X + velocity.X, player.Position.Y + velocity.Y));
            _playerDesiredCorners.Add(new Microsoft.Xna.Framework.Vector2(player.Bounds.X + player.Bounds.Width + velocity.X, player.Bounds.Y + velocity.Y));
            _playerDesiredCorners.Add(new Microsoft.Xna.Framework.Vector2(player.Bounds.X + velocity.X, player.Bounds.Y + velocity.Y + player.Bounds.Height));
            _playerDesiredCorners.Add(new Microsoft.Xna.Framework.Vector2(player.Bounds.X + player.Bounds.Width + velocity.X, player.Bounds.Y + velocity.Y + player.Bounds.Height));

            foreach (var c in _playerDesiredCorners)
            {
                ushort x = (ushort)(c.X / _map.TileWidth);
                ushort y = (ushort)(c.Y / _map.TileHeight);

                if (layer.TryGetTile(x, y, out var tile))
                {
                    var tmp = layer.GetTile(x, y);
                    if (tmp.GlobalIdentifier != 0) return velocity;
                }
            }

            // Log.Information("Collision - Why it came here!");
            velocity.X = 0;
            velocity.Y = 0;
            return velocity;
        }
    }
}


