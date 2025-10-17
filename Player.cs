using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
// using System.Numerics;
// using System;
// using System.Drawing;
// using System.Numerics;

namespace Knight {
    public class Player
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float speed = 2.5f;

        private double timer;

        private string direction;

        private Dictionary<string, List<Rectangle>> playerFrames;

        private bool attack, moving, idle;

        private float _speed = 120f;
        private int _frameWidth = 16;
        private int _frameHeight = 32;
        private int _frameIndex = 0;
        private double _timer = 0;
        private double _interval = 0.15;
        public Vector2 Position => _position;


        public Player(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 startPos)
        {
            _texture = content.Load<Texture2D>("character/16x32 Idle-Sheet.png");
            _position = startPos;

            playerFrames = new Dictionary<string, List<Rectangle>>();

            // North
            InitFrames(_texture);
        }

        public Player(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 startPos, string playerSpriteSheetFile)
        {
            _texture = content.Load<Texture2D>(playerSpriteSheetFile);
            _position = startPos;

            playerFrames = new Dictionary<string, List<Rectangle>>();

            InitFrames(_texture);
        }

        private void InitFrames(Texture2D _texture)
        {
            List<Rectangle> tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
                tmpFrames.Add(new Rectangle(j * 16, 4 * 32, 16, 32));

            playerFrames.Add("N", tmpFrames);

            // South
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
                tmpFrames.Add(new Rectangle(j * 16, 0 * 32, 16, 32));
            playerFrames.Add("S", tmpFrames);

            // West <-
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
                tmpFrames.Add(new Rectangle(j * 16, 2 * 32, 16, 32));
            playerFrames.Add("W", tmpFrames);

            // East ->
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
            {
                Rectangle tmpRect = new Rectangle(j * 16, 2 * 32, 16, 32);
                tmpFrames.Add(tmpRect);
            }
            playerFrames.Add("W", tmpFrames);

            // North West 
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(j * 16, 3 * 32, 16, 32));
            }
            playerFrames.Add("NW", tmpFrames);

            // North East
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(j * 16, 3 * 32, 16, 32));
            }
            playerFrames.Add("NE", tmpFrames);

            // South West
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(j * 16, 1 * 32, 16, 32));
            }
            playerFrames.Add("SW", tmpFrames);

            // South East
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < _texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(j * 16, 1 * 32, 16, 32));
            }
            playerFrames.Add("SE", tmpFrames);
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
            spriteBatch.Draw(_texture, _position, playerFrames[direction][_frameIndex], Color.White);
        }
    }
}
