using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    public class Ball
    {
        private Texture2D sBall;

        private GameServices gs;

        public Vector2 size;

        private Vector2 pos;
        private Vector2 spd;
        public Vector2 vel;
        public Rectangle rBall;

        private Vector2 bounds;

        public Ball(GameServices pGS)
        {
            this.gs = pGS;
        }

        public void Init()
        {
            this.pos = new Vector2((this.gs.theGame.GraphicsDevice.Viewport.Width / 2)-20, this.gs.theGame.GraphicsDevice.Viewport.Height - 200);
            this.spd = new Vector2(2, 2);
            this.vel = new Vector2(0, 0);
            this.bounds = this.gs.GetBounds();
        }
        public void Load()
        {
            this.sBall = this.gs.theGame.Content.Load<Texture2D>("ball_blue_small");
            this.size = new Vector2(sBall.Width / 2, sBall.Height / 2);

            rBall = new Rectangle((int)pos.X, (int)pos.Y, sBall.Width, sBall.Height);
        }

        public void Update()
        {
            if(this.pos.X >= this.bounds.X || this.pos.X <= 0)
            {
                this.spd.X *= -1;
            }
            if (this.pos.Y >= this.bounds.Y || this.pos.Y <= 0)
            {
                this.spd.Y *= -1;
            }
            this.pos += this.spd;

            this.rBall.X = (int)this.pos.X; // Mouvements Rectangle de collision
            this.rBall.Y = (int)this.pos.Y;

            if (this.gs.IsColliding(rBall,this.gs.paddle.rPaddle))
            {
                Trace.WriteLine("Collide!");
                this.spd.Y *= -1;
                if(rBall.Y > this.gs.paddle.pos.Y)
                {
                    this.pos.Y = this.gs.paddle.pos.Y - 10;
                }
                //this.gs.theGame.Exit();
            }
        }

        public void Draw(SpriteBatch pBatch)
        {
            pBatch.Draw(sBall, pos, null, Color.White, 0, size, 1.0f, SpriteEffects.None, 0);
        }

        public void ResetPos()
        {

        }
    }
}
