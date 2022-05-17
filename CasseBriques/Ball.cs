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
        private int speed;
        public Vector2 vel;
        private float angle;

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
            angle = MathF.PI*(7.0f/4.0f);
            speed = 6;
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

            if(angle >= MathF.PI * 2) // Clamp l'angle à moins de 2*PI
            {
                angle = angle - (MathF.PI * 2);
            }


            //Trace.WriteLine("Angle : " + angle);
            if (this.pos.X >= this.bounds.X-sBall.Width || this.pos.X <= 0)
            {
                angle = MathF.PI - angle;
                //pos.X = oldPos.X - (sBall.Width/2);
            }
            if (this.pos.Y >= this.bounds.Y-sBall.Height || this.pos.Y <= 0)
            {
                angle = MathF.PI*(3.0f/2.0f) + angle;
                //pos.Y = oldPos.Y-(sBall.Height/2);
            }

            Vector2 v = new Vector2(speed * MathF.Cos(angle), speed * MathF.Sin(angle));
            pos += v;

            this.rBall.X = (int)this.pos.X; // Mouvements Rectangle de collision
            this.rBall.Y = (int)this.pos.Y;

            oldPos = pos; // Garde l'ancienne position pour affiner les rebonds
        }

        public void UpdateColl(ICollider pCollider) // Test des collisions
        {
            if (CustomFunctions.IsColliding(rBall, pCollider.GetCollRect()))
            {
                angle = angle + CustomFunctions.RndFloat(0.01f, 0.06f); // Un peu de random dans le rebondiou !
                if (pCollider is Brick)
                {
                    IManager srvBricks = ServicesLocator.GetService<IManager>();
                    srvBricks.DeleteObject(pCollider);
                    //(ICollider)pCollider => Brique passer au briqueManager
                    if (angle >= MathF.PI * 1.5f)
                    {
                        angle = (MathF.PI / 2) + angle;
                    }
                    else
                    {
                        angle = MathF.PI + angle;
                    }
                }

                if(pCollider is Paddle)
                {
                    pos.Y = oldPos.Y - (sBall.Height/4); //Repositionne la balle pour éviter les overlap buggués

                    //Trace.WriteLine("Angle : " + angle);
                    if(rBall.X <= pCollider.GetPosition().X) // Centre de la raquette
                    {
                        Trace.WriteLine("Touche à gauche !");
                        if(angle >= MathF.PI * 1.5f)
                        {
                            angle = MathF.PI + angle; // Renvoit à 180°
                        }else
                        {
                            angle = (MathF.PI/2) + angle;
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Touche à droite !");
                        if (angle >= MathF.PI * 1.5f)
                        {
                            angle = (MathF.PI / 2) + angle;
                        }
                        else
                        {
                            angle = MathF.PI + angle;
                        }
                    }
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
