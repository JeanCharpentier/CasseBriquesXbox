﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;

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

        private bool isMoving;

        // Read Only : public int x { get; private set; }


        public Rectangle rBall;

        private Vector2 bounds;

        public Ball()
        {
            isMoving = false;
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
            this.spd = new Vector2(6, 6);
            
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
            
            origin = new Vector2(sBall.Width/2, sBall.Height/2);

            rBall = new Rectangle((int)pos.X, (int)pos.Y, sBall.Width, sBall.Height);
        }

        public void Update()
        {
            if (isMoving)
            {
                if (this.pos.X >= this.bounds.X - sBall.Width || this.pos.X <= 0)
                {
                    spd.X *= -1;
                }
                if (this.pos.Y >= this.bounds.Y - sBall.Height || this.pos.Y <= 0)
                {
                    spd.Y *= -1;
                }

                Vector2 v = new Vector2(speed * MathF.Cos(angle), speed * MathF.Sin(angle));
                pos += spd;

                rBall.X = (int)pos.X - (int)origin.X; // Mouvements Rectangle de collision
                rBall.Y = (int)pos.Y - (int)origin.Y;

                oldPos = pos; // Garde l'ancienne position pour affiner les rebonds
            }else if(!isMoving && (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A)))
            {
                isMoving = true;
            }else if (!isMoving)
            {
                ICollider srvPaddle = ServicesLocator.GetService<ICollider>();
                if(srvPaddle != null && srvPaddle is Paddle)
                {
                    pos.X = srvPaddle.GetPosition().X;
                    pos.Y = srvPaddle.GetPosition().Y - (sBall.Height/2);
                }
            }
        }

        public void UpdateColl(ICollider pCollider) // Test des collisions
        {
            if (CF.IsColliding(rBall, pCollider.GetCollRect()))
            {
                if (pCollider is Brick)
                {
                    pCollider.ManageLife();
                    if (pos.X <= pCollider.GetPosition().X || pos.X >= pCollider.GetPosition().X + pCollider.GetCollRect().Width)
                    {
                        spd.X *= -1;
                        
                    }else
                    {
                        spd.Y *= -1;
                    }
                    
                }

                if(pCollider is Paddle)
                {
                    Vector2 paddleLoc = pCollider.GetPosition();
                    Rectangle paddleRect = pCollider.GetCollRect();

                    float ballDist = CF.Dist2(pos, paddleLoc);
                    int deg = 10;

                    if(spd.X > 0) // Vient de la gauche
                    {
                        deg = 10;
                    }else // Vient de la droite
                    {
                        deg = -10;
                    }

                    spd.Y *= -1;
                    spd.X = deg * (ballDist / 100);

                    if (pos.X >= paddleLoc.X + (paddleRect.Width/2))
                    {
                        pos.X += sBall.Width / 2;
                        spd.X *= -1;
                    }else if(pos.X <= paddleLoc.X - (paddleRect.Width / 2))
                    {
                        pos.X -= sBall.Width / 2;
                        spd.X *= -1;
                    }
                }

                if(pCollider is Hole)
                {
                    Debug.WriteLine("Fin de NIVEAU !! GG !");
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
