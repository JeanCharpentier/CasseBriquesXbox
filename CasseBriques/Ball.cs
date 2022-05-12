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

        public Vector2 size;

        private Vector2 pos;
        private Vector2 spd;
        public Vector2 vel;

        // Read Only : public int x { get; private set; }


        public Rectangle rBall;

        private Vector2 bounds;

        public Ball()
        {
        }

        public void Init()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                this.bounds = srvMain.GetBounds();
            }
            else
            {
                this.bounds = new Vector2(800, 600);
            }
            this.pos = new Vector2((this.bounds.X / 2)-20, this.bounds.Y - 200);
            this.spd = new Vector2(2, 2);
            this.vel = new Vector2(0, 0);
        }
        public void Load()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                this.sBall = srvMain.LoadT2D("ball_blue_small");
            }
            
            this.size = new Vector2(sBall.Width / 2, sBall.Height / 2);

            rBall = new Rectangle((int)pos.X, (int)pos.Y, sBall.Width, sBall.Height);
        }

        public void Update()
        {
            ICollider srvCollider = ServicesLocator.GetService<ICollider>();
            if (this.pos.X >= this.bounds.X || this.pos.X <= 0)
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

            if (GameServices.IsColliding(rBall,srvCollider.GetCollRect()))
            {
                Trace.WriteLine("Collide!");
                this.spd.Y *= -1;
                if(rBall.Y > srvCollider.GetPosition().Y) // Replacer la balle au dessus de la raquette
                {
                    //this.pos.Y = this.gs.paddle.pos.Y - 10;
                }
            }
        }

        public void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sBall, pos, null, Color.White, 0, size, 1.0f, SpriteEffects.None, 0);
            }
        }

        public void ResetPos()
        {

        }
    }
}
