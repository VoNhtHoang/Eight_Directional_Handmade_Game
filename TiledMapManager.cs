// using Microsoft.Xna.Framework.Content;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGame.Extended.Tiled;
// using MonoGame.Extended.Tiled.Renderers;
// using Microsoft.Xna.Framework;

// namespace MyGame {
//     public class TileMap {
//         private TiledMap tiledMap;
//         private TiledMapRenderer renderer;
//         public Vector2 MapSize { get; private set; }

//         public TileMap(ContentManager content, string mapFile) {
//             tiledMap = content.Load<TiledMap>(mapFile);
//             renderer = new TiledMapRenderer(content.ServiceProvider.GetService<GraphicsDevice>(), tiledMap);

//             MapSize = new Vector2(
//                 tiledMap.WidthInPixels,
//                 tiledMap.HeightInPixels
//             );
//         }

//         public void Draw(SpriteBatch spriteBatch) {
//             renderer.Draw();
//         }
//     }
// }

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

using Serilog;

using System.Collections.Generic;
using System.Drawing;

namespace Knight
{
    public class TiledMapManager
    {
        public MonoGame.Extended.Tiled.TiledMap _map { get; private set; }
        public TiledMapRenderer _renderer;

        public List<RectangleF> collisionRects { get; private set; }
        public TiledMapManager(ContentManager content, GraphicsDevice graphicsDevice, string mapPath)
        {
            _map = content.Load<TiledMap>(mapPath);
            _renderer = new TiledMapRenderer(graphicsDevice, _map);
        }

        private void LoadCollisionLayer()
        {
            var layer = _map.GetLayer<TiledMapObjectLayer>("obstacles");
            if (layer == null) return;

            foreach (var obj in layer.Objects)
            {
                if (obj is TiledMapObject { IsVisible: true })
                    collisionRects.Add(new RectangleF(obj.Position.X, obj.Position.Y, obj.Size.Width, obj.Size.Height));
            }
        }

        public void Draw(GameTime gameTime)
        {
            _renderer.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
        }

        public bool CheckCollision(RectangleF rect)
        {
            // foreach (var c in collisionRects)
            // {
            //     if (rect.Intersects(c))
            //         return true;
            // }
            // return false;
            return false;
        }

    }
}


