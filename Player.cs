using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using System;
using System;
using System.Collections.Generic;
using System.Data;
using Serilog;
using System.IO;
// using System.Collections.Specialized;
// using System.ComponentModel;
// using System.Reflection.PortableExecutable;

namespace Knight
{
    public class Player
    {
        private List<Texture2D> _texture;

        // private float speed = 2.5f;
        // private double timer;
        private string direction = "S";
        private string[] directions = ["S", "SW", "SE", "W", "E", "NW", "NE", "N"];

        private Dictionary <string, Rectangle[]> playerIdleFrames, playerWalkFrames, playerRunFrames;
        // private bool attack, moving, idle;
        private int frameNums;
        private float _speed = 120f;
        private int _frameWidth = 16;
        private int _frameHeight = 32;
        private int _frameIndex = 0;
        private double _timer = 0;
        private double _interval = 0.15;

        private Microsoft.Xna.Framework.Vector2 _position;
        public Microsoft.Xna.Framework.Vector2 Position => _position;

        public Player(ContentManager content, string[] spritesheetPaths, Microsoft.Xna.Framework.Vector2 _position)
        {
            // _texture = content.Load<Texture2D>(spritesheetPath);
            try
            {
                _texture = new List<Texture2D>();
                foreach (string sheetPath in spritesheetPaths) _texture.Add(content.Load<Texture2D>(sheetPath));
                this._position = _position;
                InitFrames();
                
            } catch (Exception ex)
            {
                Log.Warning(" Log Warn : " + ex.Message + "\t" + ex.Data.ToString());
            }
        }

        private void InitFrames()
        {
            int frameNums = (int)(_texture[0].Width / _frameWidth), frameDirections = (int)(_texture[0].Height / _frameHeight);
            Log.Information("\t  FrameDirections: " + frameDirections.ToString());
            // Player Idle Frames
            for (int i = 0; i < frameDirections; i++)
            {
                Rectangle[] tmpRects = new Rectangle[4];

                for (int j = 0; j < frameNums; j++) tmpRects[j] = new Rectangle(j * _frameWidth, _frameHeight * 1, _frameWidth, _frameHeight);

                playerIdleFrames.Add(directions[i], tmpRects);
            }

            Log.Information("Player Dict : " + playerIdleFrames.ToString() + "\t  FrameNums: " + frameNums.ToString());
        }
        
        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            Vector2 move = Vector2.Zero;
            if (ks.IsKeyDown(Keys.W)) { move.Y -= 1; this.direction = "W"; }  // Up
            if (ks.IsKeyDown(Keys.S)) { move.Y += 1; this.direction = "S"; }  // Down
            if (ks.IsKeyDown(Keys.A)) { move.X -= 1; this.direction = "W"; }  // Left
            if (ks.IsKeyDown(Keys.D)) { move.X += 1; this.direction = "E"; }  // Right
            if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.A) && ks.GetPressedKeyCount() > 1)
            {
                move.Y -= 1;
                move.X -= 1;
                this.direction = "NW";
            }
            if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.D) && ks.GetPressedKeyCount() > 1)
            {
                move.Y -= 1;
                move.X += 1;
                this.direction = "NE";
            }
            if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.A) && ks.GetPressedKeyCount() > 1)
            {
                move.Y += 1;
                move.X -= 1;
                this.direction = "SW";
            }
            if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.A) && ks.GetPressedKeyCount() > 1)
            {
                move.Y += 1;
                move.X += 1;
                this.direction = "SE";
            }

            if (move != Vector2.Zero)
            {
                move.Normalize();
                _position += move * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                _timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer > _interval)
                {
                    _frameIndex = (_frameIndex + 1) % 4;
                    _timer = 0;
                }
            }
            else
            {
                _frameIndex = 0; // idle giữa 3 frame
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture[0], _position, playerIdleFrames[direction][_frameIndex], Color.White);
        }
    }
}