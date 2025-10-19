// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;

// namespace Knight {
//     public class Camera {
//         private Viewport viewport;
//         private Matrix transform;
//         private Vector2 position;
//         private Vector2 mapSize;

//         public Matrix Transform => transform;

//         public Camera(Viewport viewport, Vector2 mapSize) {
//             this.viewport = viewport;
//             this.mapSize = mapSize;
//         }

//         public void Follow(Vector2 target)
//         {
//             position = target - new Vector2(viewport.Width / 2, viewport.Height / 2);

//             // Giới hạn trong bản đồ
//             position.X = MathHelper.Clamp(position.X, 0, mapSize.X - viewport.Width);
//             position.Y = MathHelper.Clamp(position.Y, 0, mapSize.Y - viewport.Height);

//             transform = Matrix.CreateTranslation(new Vector3(-position, 0));
//         }
        
//     }
// }

// using Microsoft.Xna.Framework;
// using MonoGame.Extended;
// using Microsoft.Xna.Framework.Graphics;


// namespace Knight
// {    
//     public class Camera2D
//     {
//         private OrthographicCamera _camera;

//         public Camera2D(GraphicsDevice device)
//         {
//             _camera = new OrthographicCamera(device);
//         }

//         public void LookAt(Vector2 position)
//         {
//             _camera.LookAt(position);
//         }

//         public Matrix GetViewMatrix() => _camera.GetViewMatrix();
//     }
// }

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Knight
{
    public class Camera
    {
        private GraphicsDevice _graphics;
        private Matrix _transform;
        private Vector2 _position;
        private float _zoom;
        private float _rotation;
        private float _followSpeed = 2.9f;

        public Matrix Transform => _transform;
        public float Zoom => _zoom;
        public Vector2 Position => _position;

        public Camera(GraphicsDevice graphics)
        {
            _graphics = graphics;
            _zoom = 2.5f; // zoom
            _rotation = 0f;
            _position = Vector2.Zero;
        }

        public void Follow(Vector2 target,GameTime gameTime, int mapWidth, int mapHeight)
        {
            _position = target;

            // LERP help smooth camera movement
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position = Vector2.Lerp(_position, target, _followSpeed * delta);

            // Tạo ma trận transform
            var viewport = _graphics.Viewport;
            var viewWidth = viewport.Width;
            var viewHeight = viewport.Height;

            _transform =
                Matrix.CreateTranslation(new Vector3( -_position.X, -_position.Y - 16, 0)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom) *
                Matrix.CreateTranslation(new Vector3(viewWidth / 2f, viewHeight / 2f, 0));

            // Giữ camera không vượt map
            var halfWidth = (viewWidth / 2f) / _zoom;
            var halfHeight = (viewHeight / 2f) / _zoom;

            _position.X = MathHelper.Clamp(_position.X, halfWidth, mapWidth - halfWidth);
            _position.Y = MathHelper.Clamp(_position.Y, halfHeight, mapHeight - halfHeight);
        }

        public void ZoomIn() => _zoom = MathHelper.Min(_zoom + 0.1f, 3f);
        public void ZoomOut() => _zoom = MathHelper.Max(_zoom - 0.1f, 0.5f);
    }
}
