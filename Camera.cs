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
