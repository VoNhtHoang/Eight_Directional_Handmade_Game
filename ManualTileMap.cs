using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework;

using Serilog;

using System.Collections.Generic;
using System;

using MonoGame.Extended.Serialization.Json;
using MonoGame.Extended.Collisions.Layers;
using MonoGame.Extended;
using System.Security.Cryptography.X509Certificates;

namespace Knight
{
    public class ManualTileMap
    {
        public TiledMap _map { get; private set; }

        public List<MonoGame.Extended.RectangleF> _collisionRects { get; private set; }

        public List<Microsoft.Xna.Framework.Vector2> _playerDesiredCorners;

        public int _mapWidth { get; private set; }
        public int _mapHeight { get; private set; }

        // private List<(TiledMapTileLayer layer, Action<SpriteBatch> )> _drawList;

        // private readonly string[] _depthLayers = { "Tree", "obj-ground", "wall_cliff" };
        public ManualTileMap(ContentManager content, GraphicsDevice _graphicsDevice, string mapPath)
        {
            _map = content.Load<TiledMap>(mapPath);

            _mapWidth = _map.WidthInPixels;
            _mapHeight = _map.HeightInPixels;

            // _drawList = new();
            // foreach (var layer in _map.TileLayers)
            // {
            //     float depth = GetDrawDepth(layer);
            //     _drawList.Add((layer, _spriteBatch => DrawLayer(_map, _spriteBatch, layer, depth)));
            // }
            _collisionRects = new();
            LoadCollisionLayer();
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

            Log.Information("File Tiled Map Manager ---- collision Rects count" + _collisionRects.Count.ToString());
            
        }

        public float GetDrawDepth(TiledMapTileLayer layer)
        {
            return Convert.ToSingle(layer.Properties["drawDepth"].ToString());
        }

        public List<TiledMapTileLayer> getTileLayers()
        {
            List<TiledMapTileLayer> results = new();
            foreach (var layer in _map.TileLayers) results.Add(layer);
            return results;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {

        }

        public void Draw(TiledMap map, SpriteBatch _spriteBatch, TiledMapTileLayer layer, float drawDepth)
        {
            Log.Information("Manual Tile Map File  -  layer: " + layer.Name);
            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    var tile = layer.GetTile((ushort)x, (ushort)y);
                    
                    if (tile.GlobalIdentifier == 0)
                        continue;

                    var tileset = map.GetTilesetByTileGlobalIdentifier(tile.GlobalIdentifier);

                    var region = tileset.GetTileRegion(tile.GlobalIdentifier - map.GetTilesetFirstGlobalIdentifier(tileset));
                    // Rectangle region = new Rectangle(tile.X * map.TileWidth, tile.Y * map.TileHeight, map.TileWidth, map.TileHeight);
                    // var pos = new Vector2(x * map.TileWidth, y * map.TileHeight);

                    Vector2 pos = new Vector2(
                        x * map.TileWidth,
                        y * map.TileHeight
                    );

                    _spriteBatch.Draw(
                        tileset.Texture,
                        pos,
                        region,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        drawDepth
                    );
                }
            }
        }
        
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
                }
            }
            // Log.Information("Map - Come to ResolveColl");
            return velocity;
        }
    }
}