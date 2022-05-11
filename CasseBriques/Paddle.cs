using System;
using System.Diagnostics;
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

        public GameServices gs;

        private Vector2 bounds;

        public Texture2D sPaddle;
        public Rectangle rPaddle; // Rectangle de collision

        public Vector2 pos;
        public Vector2 spd;
        public Vector2 vel;
        public Vector2 size;

        private Paddle(GameServices pGS)
        {
            this.gs = pGS;
        }

        public void Init()
        {
            this.pos = new Vector2(this.gs.theGame.GraphicsDevice.Viewport.Width/2, this.gs.theGame.GraphicsDevice.Viewport.Height - 100);
            this.spd = new Vector2(2, 2);
            this.vel = new Vector2(0, 0);
            this.bounds = new Vector2(this.gs.theGame.GraphicsDevice.Viewport.Width, this.gs.theGame.GraphicsDevice.Viewport.Height);
        }

        public void Load()
        {
            sPaddle = this.gs.theGame.Content.Load<Texture2D>("block_narrow");
            size = new Vector2(sPaddle.Width / 2, sPaddle.Height / 2);

            rPaddle = new Rectangle((int)pos.X, (int)pos.Y, sPaddle.Width, sPaddle.Height);
        }

        public static Paddle GetInstance(GameServices pGS)
        {
            if(paddleInstance == null)
            {
                paddleInstance = new Paddle(pGS);
            }

            return paddleInstance;
        }

        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (this.vel.X >= -20)
                {
                    this.vel.X -= 2;
                }

            }else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (this.vel.X <= 20)
                {
                    this.vel.X += 2;
                }
            }
            if ((GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Released) || (Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Left)))
            {
                this.vel.X = this.gs.Lerp(this.vel.X, 0.0f, 0.07f);
            }

            if (pos.X+vel.X >= 0 && pos.X+vel.X <= this.bounds.X)
            {
                pos.X += vel.X;
            }

            this.rPaddle.X = (int)this.pos.X; // Mouvements Rectangle de collision
            this.rPaddle.Y = (int)this.pos.Y;
        }

        public void Draw(SpriteBatch pBatch)
        {
            pBatch.Draw(sPaddle, pos, null, Color.White, 0, size, 1.0f, SpriteEffects.None, 0);
        }
    }
}
