using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private Paddle(string pTexString):base(pTexString)
        {
            ServicesLocator.AddService<ICollider>(this);
            pos = new Vector2(this.bounds.X / 2, this.bounds.Y - 100);
            origin = new Vector2(sprite.Width / 2, sprite.Height /2);
            spd = new Vector2(1.5f, 1.5f);
            rPaddle = new Rectangle((int)pos.X - (int)origin.X, (int)pos.Y - (int)origin.Y, sprite.Width, sprite.Height);
        }
        public static Paddle GetInstance()
        {
            if(paddleInstance == null)
            {
                paddleInstance = new Paddle("block_narrow");
                /*IImageLoader srvImg = ServicesLocator.GetService<IImageLoader>();
                if (srvImg != null)
                {
                    Texture2D sPaddle;
                    //sPaddle = srvImg.LoadT2D("block_narrow");
                    paddleInstance = new Paddle("block_narrow");
                }*/
            }
            return paddleInstance;
        }

        public void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (vel.X >= -maxVel)
                {
                    vel.X -= factVel;
                }

            }else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (vel.X <= maxVel)
                {
                    vel.X += factVel;
                }
            }
            if ((GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Released) || (Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Left)))
            {
                vel.X = CF.Lerp(vel.X, 0.0f, 0.07f);
            }

            if (pos.X+vel.X >= 0 && pos.X+vel.X <= bounds.X)
            {
                pos.X += vel.X;
            }

            rPaddle.X = (int)pos.X - (int)origin.X; // Mouvements Rectangle de collision
            rPaddle.Y = (int)pos.Y - (int)origin.Y;

            // Orientation au démarrage
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                IVisee srvVisee = ServicesLocator.GetService<IVisee>();
                if(srvVisee != null)
                {
                    srvVisee.Rotate(0.1f);
                }
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.E))
            {
                IVisee srvVisee = ServicesLocator.GetService<IVisee>();
                if (srvVisee != null)
                {
                    srvVisee.Rotate(-0.1f);
                }
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
        public bool ManageLife()
        {
            throw new NotImplementedException();
        }
    }
}
