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

        public Vector2 origin;

        private Vector2 pos;
        public Vector2 oldPos;
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
            
            origin = new Vector2(0,0);

            rBall = new Rectangle((int)pos.X, (int)pos.Y, sBall.Width, sBall.Height);
        }

        public void Update()
        {
            oldPos = pos;

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
        }

        public void UpdateColl(ICollider pCollider) // Test des collisions
        {
            if (CustomFunctions.IsColliding(rBall, pCollider.GetCollRect()))
            {
                pos = oldPos;
                spd.Y *= -1;


                if(pCollider is Brick)
                {
                    IManager srvBricks = ServicesLocator.GetService<IManager>();
                    srvBricks.DeleteObject(pCollider);
                    //(ICollider)pCollider => Brique passer au briqueManager
                }

            }
        }

        public void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sBall, pos, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
