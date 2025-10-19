// using Microsoft.Xna.Framework;
// using MonoGame.Extended;

// namespace Knight
// {
//     public class CollisionManager
//     {
//         private readonly TiledMapManager _mapManager;

//         public CollisionManager(TiledMapManager mapManager)
//         {
//             _mapManager = mapManager;
//         }

//         public Vector2 ResolveCollision(RectangleF playerRect, Vector2 velocity)
//         {
//             var nextRect = new RectangleF(playerRect.Position + velocity, playerRect.Size);

//             foreach (var rect in _mapManager._collisionRects)
//             {
//                 if (nextRect.Intersects(rect))
//                 {
//                     // Tách trục X
//                     var xRect = new RectangleF(playerRect.Position + new Vector2(velocity.X, 0), playerRect.Size);
//                     if (!xRect.Intersects(rect))
//                         velocity.Y = 0;
//                     else
//                         velocity.X = 0;
//                 }
//             }

//             return velocity;
//         }
//     }
// }
