using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using System;
using System;
using System.Collections.Generic;
// using System.Data;

using Serilog;

using System.IO;
// using System.Numerics;
// using System.Dynamic;
// using System.Drawing;
// using System.Collections.Specialized;
// using System.ComponentModel;
// using System.Reflection.PortableExecutable;

namespace Knight
{
    public class Player
    {
        public List<Texture2D> _texture { get; private set; }
        public string direction = "S";
        private string[] directions = ["S", "SW", "SE", "W", "E", "NW", "NE", "N"];

        private Dictionary<string, Microsoft.Xna.Framework.Rectangle[]> playerIdleFrames, playerWalkFrames, playerRunFrames;
        private InterfacePlayerState _state;

        // private bool moving = false, idle = true, walk =false;
        public enum State
        {
            Idle,
            Walk,
            Run
        }

        public State _playerState;

        public float _speed { get; private set;}
        private int _frameWidth = 16;
        private int _frameHeight = 32;
        public int _frameIndex = 0;
        public double _timer = 0;
        public double _interval = 0.15f;

        // pos & veclo & collision
        public Microsoft.Xna.Framework.Vector2 _position;
        public Microsoft.Xna.Framework.Vector2 Position => _position;
        public Microsoft.Xna.Framework.Vector2 _veclocity;
        public MonoGame.Extended.RectangleF Bounds => new MonoGame.Extended.RectangleF(_position.X, _position.Y + 24, _frameWidth, _frameHeight - 24);

        // public CollisionManager _collisionManager { get; private set; }
        public Player(ContentManager content, string[] spritesheetPaths, Microsoft.Xna.Framework.Vector2 _position)
        {
            // _texture = content.Load<Texture2D>(spritesheetPath);
            try
            {
                _texture = new List<Texture2D>();
                foreach (string sheetPath in spritesheetPaths) _texture.Add(content.Load<Texture2D>(sheetPath));
                this._position = _position;

                // Init
                playerIdleFrames = new Dictionary<string, Microsoft.Xna.Framework.Rectangle[]>();
                playerRunFrames = new Dictionary<string, Microsoft.Xna.Framework.Rectangle[]>();
                playerWalkFrames = new Dictionary<string, Microsoft.Xna.Framework.Rectangle[]>();

                InitFrames();

                _speed = 120f;
                _playerState = State.Idle;

                _state = new PlayerIdleState();
                _state.Enter(this);


            }
            catch (Exception ex)
            {
                Log.Warning(" Log Warn : " + ex.Message + "\t" + ex.Data.ToString());
            }
        }

        // public void setCollisionManager(CollisionManager _collisionManager)
        // {
        //     this._collisionManager = _collisionManager;
        // }

        private void InitFrames()
        {
            // Player Idle Frames
            int frameNums = (int)(_texture[0].Width / _frameWidth), frameDirections = (int)(_texture[0].Height / _frameHeight);
            for (int i = 0; i < frameDirections; i++)
            {
                Microsoft.Xna.Framework.Rectangle[] tmpRects = new Microsoft.Xna.Framework.Rectangle[4];
                for (int j = 0; j < frameNums; j++) tmpRects[j] = new Microsoft.Xna.Framework.Rectangle(j * _frameWidth, _frameHeight * i, _frameWidth, _frameHeight);

                playerIdleFrames.Add(directions[i], tmpRects);
            }

            Log.Information("Player Dict : " + playerIdleFrames.ToString() + "\t  FrameNums: " + frameNums.ToString());


            // Player Walk Frames
            frameNums = (int)(_texture[1].Width / _frameWidth);
            frameDirections = (int)(_texture[1].Height / _frameHeight);
            for (int i = 0; i < frameDirections; i++)
            {
                Microsoft.Xna.Framework.Rectangle[] tmpRects = new Microsoft.Xna.Framework.Rectangle[4];
                for (int j = 0; j < frameNums; j++) tmpRects[j] = new Microsoft.Xna.Framework.Rectangle(j * _frameWidth, _frameHeight * i, _frameWidth, _frameHeight);

                playerWalkFrames.Add(directions[i], tmpRects);
            }
            Log.Information("Player Walk Dict Rect : " + playerWalkFrames.ToString() + "\t  FrameNums: " + frameNums.ToString());
        }
        
        public void ChangeState(InterfacePlayerState newState)
        {
            _state.Exit(this);

            _state = newState;
            
            _state.Enter(this);
        }
        
        public void Update(GameTime gameTime, ManualTileMap _map) //TiledMapManager
        {
            _state.Update(this, gameTime, _map);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_playerState == State.Idle)
            {
                spriteBatch.Draw(_texture[0], _position, playerIdleFrames[direction][_frameIndex], Color.White);
                // Log.Information("Using Idle Frames");
            }
            else if (_playerState == State.Walk)
            {
                spriteBatch.Draw(_texture[1], _position, playerWalkFrames[direction][_frameIndex], Color.White);
                // Log.Information("Using Walking State");
            }
        }

        public void Draw(SpriteBatch spriteBatch, float playerDepth)
        {
            Texture2D tex = _playerState == State.Walk ? _texture[1] : _texture[0];
            Rectangle src = _playerState == State.Walk ? playerWalkFrames[direction][_frameIndex]
                                               : playerIdleFrames[direction][_frameIndex];

            Vector2 origin = new(src.Width / 2f, src.Height / 2f);
            spriteBatch.Draw(
                tex,
                _position,
                sourceRectangle: src,
                Color.White,
                rotation: 0f,            // hoặc MathHelper.ToRadians(90f)
                origin: Vector2.Zero,
                1.0f,                    // scale an toàn
                SpriteEffects.None,
                layerDepth: playerDepth
            );

            // Log.Information($"SortMode={SpriteSortMode.FrontToBack}, Depth={playerDepth}");
            // if (_playerState == State.Idle)
            // {
            //     Microsoft.Xna.Framework.Rectangle rect = playerIdleFrames[direction][_frameIndex];

            //     Microsoft.Xna.Framework.Vector2 origin = new Microsoft.Xna.Framework.Vector2(rect.Width / 2f, rect.Height / 2f);

            //     spriteBatch.Draw(_texture[0], _position, rect, Color.White, rotation: MathHelper.ToRadians(0), origin, 1.0f, SpriteEffects.None, playerDepth);
            //     // Log.Information("Using Idle Frames");
            // }
            // else if (_playerState == State.Walk)
            // {
            //     Microsoft.Xna.Framework.Rectangle rect = playerWalkFrames[direction][_frameIndex];

            //     Microsoft.Xna.Framework.Vector2 origin = new Microsoft.Xna.Framework.Vector2(rect.Width / 2f, rect.Height / 2f);

            //     spriteBatch.Draw(_texture[1], _position, rect, Color.White, rotation: MathHelper.ToRadians(0), origin, 1.0f, SpriteEffects.None, playerDepth);
            //     // Log.Information("Using Walking State");
            // }

        }
    }
}