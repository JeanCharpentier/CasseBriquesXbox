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
    public class Paddle : Entity,ICollider
    {
        private static Paddle paddleInstance;

        public Rectangle rPaddle; // Rectangle de collision

        public Vector2 spd;
        public Vector2 vel;

        const int maxVel = 15;
        const float factVel = 1.5f;

        private Paddle(Texture2D pTexture):base(pTexture)
        {
            ServicesLocator.AddService<ICollider>(this);
        }

        public void Init()
        {
            pos = new Vector2(this.bounds.X / 2, this.bounds.Y - 100);
            spd = new Vector2(1.5f, 1.5f);
            vel = new Vector2(0, 0);
        }

        public void Load()
        { 
            /*IMain srvMain = ServicesLocator.GetService<IMain>();
            if(srvMain != null)
            {
                sPaddle = srvMain.LoadT2D("block_narrow");
            }*/

            origin = new Vector2(sprite.Width/2, 0);

            rPaddle = new Rectangle((int)pos.X-(int)origin.X, (int)pos.Y, sprite.Width, sprite.Height);
        }

        public static Paddle GetInstance()
        {
            if(paddleInstance == null)
            {
                IMain srvMain = ServicesLocator.GetService<IMain>();
                if (srvMain != null)
                {
                    Texture2D sPaddle;
                    sPaddle = srvMain.LoadT2D("block_narrow");
                    paddleInstance = new Paddle(sPaddle);
                }
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
        public Vector2 GetPosition()
        {
            return pos;
        }
        public Rectangle GetCollRect()
        {
            return rPaddle;
        }
        public bool ManageLife()
        {
            throw new NotImplementedException();
        }
    }
}
