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
    public class Paddle : ICollider
    {
        private static Paddle paddleInstance;

        private Vector2 bounds;

        public Texture2D sPaddle;
        public Rectangle rPaddle; // Rectangle de collision

        public Vector2 pos;
        public Vector2 spd;
        public Vector2 vel;
        public Vector2 origin;

        const int maxSpd = 15;
        const int maxVel = 15;
        const float factVel = 1.5f;


        private Paddle()
        {
            ServicesLocator.AddService<ICollider>(this);
        }

        public void Init()
        {
            IMain srvScreen = ServicesLocator.GetService<IMain>();
            if (srvScreen != null)
            {
                this.bounds = srvScreen.GetBounds();
            }
            else
            {
                this.bounds = new Vector2(800, 600);
            }
            this.pos = new Vector2(this.bounds.X / 2, this.bounds.Y - 100);
            this.spd = new Vector2(1.5f, 1.5f);
            this.vel = new Vector2(0, 0);
        }

        public void Load()
        { 
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if(srvMain != null)
            {
                sPaddle = srvMain.LoadT2D("block_narrow");
            }

            origin = new Vector2(sPaddle.Width/2, 0);

            rPaddle = new Rectangle((int)pos.X-(int)origin.X, (int)pos.Y, sPaddle.Width, sPaddle.Height);
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
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (this.vel.X >= -maxVel)
                {
                    this.vel.X -= factVel;
                }

            }else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (this.vel.X <= maxVel)
                {
                    this.vel.X += factVel;
                }
            }
            if ((GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Released) || (Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Left)))
            {
                this.vel.X = CF.Lerp(this.vel.X, 0.0f, 0.07f);
            }

            if (pos.X+vel.X >= 0 && pos.X+vel.X <= this.bounds.X)
            {
                pos.X += vel.X;
            }

            this.rPaddle.X = (int)this.pos.X - (int)origin.X; // Mouvements Rectangle de collision
            this.rPaddle.Y = (int)this.pos.Y;
        }

        public void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sPaddle, pos, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
            }
        }

        public Vector2 GetPosition()
        {
            return pos;
        }

        public Rectangle GetCollRect()
        {
            return rPaddle;
        }

        public bool DeleteMe()
        {
            throw new NotImplementedException();
        }

        public int GetLife()
        {
            throw new NotImplementedException();
        }

        public bool SetLife(int pLife)
        {
            throw new NotImplementedException();
        }
    }
}
