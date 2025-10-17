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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework;

namespace Knight
{
    public class TileMap
    {
        public TiledMap _map { get; private set;}
        private TiledMapRenderer _renderer;
        public Vector2 MapSize { get; private set; }

        public TileMap(ContentManager contentManager, GraphicsDevice graphicsDevice, string mapFile)
        {
            _map = contentManager.Load<TiledMap>(mapFile);
            _renderer = new TiledMapRenderer(graphicsDevice, _map);

             MapSize = new Vector2(
                _map.WidthInPixels,
                _map.HeightInPixels
            );
        }

        public void Update(GameTime gameTime) => _renderer.Update(gameTime);
        public void Draw(Matrix viewMatrix) => _renderer.Draw(viewMatrix);
    }
}