using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Serilog;

using System.IO;
using System.Security.Cryptography.X509Certificates;
using Serilog.Sinks.File;


namespace Knight
{
    public interface InterfacePlayerState
    {
        void Enter(Player player);
        void Update(Player player, GameTime gameTime);
        void Exit(Player player);
    }

    public class PlayerIdleState : InterfacePlayerState
    {
        public void Enter(Player player) {
            player._playerState = Player.State.Idle;
            player._interval = 0.3f; // 15fps (tự nhiên, kiểu idle)
            player.Veclocity = Microsoft.Xna.Framework.Vector2.Zero; 
        }

        private bool check_walking_state(KeyboardState ks)
        {
            if (ks.GetPressedKeyCount() >= 1 &&
            (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.D)))
                return true;
            return false;
        }

        public void Update(Player player, GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (check_walking_state(ks)) player.ChangeState(new PlayerWalkState());

            // Microsoft.Xna.Framework.Vector2 move = Microsoft.Xna.Framework.Vector2.Zero;
            // move.Normalize();
            // player._position += move * player._speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            player._timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (player._timer > player._interval)
            {
                player._frameIndex = (player._frameIndex + 1) % 4;
                player._timer -= player._interval;
            }
        }

        public void Exit(Player player) { }
    }
    
    public class PlayerWalkState : InterfacePlayerState
    {
        public void Enter(Player player) {
            player._playerState = Player.State.Walk;
            player._interval = 0.2f;  
        }
        public void Update(Player player, GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.GetPressedKeyCount() < 1)
            {
                player.ChangeState(new PlayerIdleState());
                return;
            }
            // Log.Information("Key Down - - - - : " + ks.ToString());
            Microsoft.Xna.Framework.Vector2 move = Microsoft.Xna.Framework.Vector2.Zero;
            if (ks.IsKeyDown(Keys.W)) { move.Y -= 1; player.direction = "N"; }  // Up
            if (ks.IsKeyDown(Keys.S)) { move.Y += 1; player.direction = "S"; }  // Down
            if (ks.IsKeyDown(Keys.A)) { move.X -= 1; player.direction = "W"; }  // Left
            if (ks.IsKeyDown(Keys.D)) { move.X += 1; player.direction = "E"; }  // Right
            if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.A) && ks.GetPressedKeyCount() > 1)
            {
                move.Y -= 1;
                move.X -= 1;
                player.direction = "NW";
            }
            if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.D) && ks.GetPressedKeyCount() > 1)
            {
                move.Y -= 1;
                move.X += 1;
                player.direction = "NE";
            }
            if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.A) && ks.GetPressedKeyCount() > 1)
            {
                move.Y += 1;
                move.X -= 1;
                player.direction = "SW";
            }
            if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.D) && ks.GetPressedKeyCount() > 1)
            {
                move.Y += 1;
                move.X += 1;
                player.direction = "SE";
            }

            if (move != Microsoft.Xna.Framework.Vector2.Zero)
            {
                move.Normalize();
                player._position += move * player._speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                player._timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (player._timer > player._interval)
                {
                    player._frameIndex = (player._frameIndex + 1) % 4;
                    player._timer -= player._interval;
                }
            }
        }
        public void Exit(Player player) { player._playerState = Player.State.Idle;}
    }
}