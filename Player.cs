using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Numerics;

namespace Knight {
    public class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private float speed = 2.5f;

        private double timer;

        private int frameIndex;

        private Dictionary<string, List<Rectangle>> playerFrames;

        private bool attack, moving, idle;

        public Vector2 Position => position;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 startPos, String playerSpriteSheetFile) {
            texture = content.Load<Texture2D>(playerSpriteSheetFile);
            position = startPos;

            playerFrames = new Dictionary<string, List<Rectangle>>();


            // Giả sử mỗi frame 64x64, row 1 = idle, row 2 = move

            // North
            List<Rectangle> tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
                tmpFrames.Add(new Rectangle(idle * 16, 4 * 32, 16, 32));
            
            playerFrames.Add("N", tmpFrames);

            // South
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
                tmpFrames.Add(new Rectangle(idle * 16, 0 * 32, 16, 32));
            playerFrames.Add("S", tmpFrames);

            // West <-
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
                tmpFrames.Add(new Rectangle(idle * 16, 2 * 32, 16, 32));
            playerFrames.Add("W", tmpFrames);

            // East ->
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
            {
                Rectangle tmpRect = new Rectangle(idle * 16, 2 * 32, 16, 32);
                tmpFrames.Add(tmpRect);
            }
            playerFrames.Add("W", tmpFrames);
            
            // North West 
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(idle * 16, 3 * 32, 16, 32));
            }
            playerFrames.Add("NW", tmpFrames);

            // North East
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(idle * 16, 3 * 32, 16, 32));
            }
            playerFrames.Add("NE", tmpFrames);

            // South West
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(idle * 16, 1 * 32, 16, 32));
            }
            playerFrames.Add("SW", tmpFrames);

            // South East
            tmpFrames = new List<Rectangle>();
            for (int j = 0; j < texture.Width / 16; j++)
            {
                tmpFrames.Add(new Rectangle(idle * 16, 1 * 32, 16, 32));
            }
            playerFrames.Add("SE", tmpFrames);
        }
    }
}
