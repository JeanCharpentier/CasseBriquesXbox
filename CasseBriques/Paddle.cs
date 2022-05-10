using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Windows.Media.ContentRestrictions;

namespace CasseBriques
{
    public class Paddle
    {
        private static Paddle paddleInstance;

        public Texture2D sPaddle;

        public Vector2 pos;
        public Vector2 spd;
        public Vector2 vel;

        private Paddle()
        {

        }

        public void Init(GraphicsDevice pWindow)
        {
            pos = new Vector2(pWindow.Viewport.Width/2, pWindow.Viewport.Height - 100);
            spd = new Vector2(2, 2);
            vel = new Vector2(0, 0);
        }

        public void Load(Game pGame)
        {
            sPaddle = pGame.Content.Load<Texture2D>("block_narrow");
        }

        public static Paddle GetInstance()
        {
            if(paddleInstance == null)
            {
                paddleInstance = new Paddle();
            }

            return paddleInstance;
        }

        public void Update()
        {
            if(pos.X+vel.X >= 0 && pos.X+vel.X <= 1200)
            {
                pos.X += vel.X;
            }
            
        }

        public void Draw(SpriteBatch pBatch)
        {
            pBatch.Draw(sPaddle, pos, null, Color.White, 0, new Vector2(sPaddle.Width / 2, sPaddle.Height / 2), 1.0f, SpriteEffects.None, 0);
        }
    }
}
