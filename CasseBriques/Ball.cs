using System;
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
    public class Ball:Entity
    {
        //private Texture2D sBall;

        public Vector2 oldPos;
        private Vector2 spd;
        private int speed;
        public Vector2 vel;
        private float angle;

        private bool isMoving;

        public Rectangle rBall;

        public Ball(Texture2D pTexture):base(pTexture)
        {
            isMoving = false;
        }

        public void Init()
        {
            pos = new Vector2((bounds.X / 2)-20, bounds.Y - 200);
            spd = new Vector2(6, 6);
            
            vel = new Vector2(0, 0);
            angle = MathF.PI*(7.0f/4.0f);
            speed = 6;
        }
        public void Load()
        {            
            origin = new Vector2(sprite.Width/2, sprite.Height/2);

            rBall = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
        }

        public void Update()
        {
            if (isMoving)
            {
                if (pos.X >= bounds.X - sprite.Width || pos.X <= 0)
                {
                    spd.X *= -1;
                }
                if (pos.Y >= bounds.Y - sprite.Height || pos.Y <= 0)
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
                    pos.Y = srvPaddle.GetPosition().Y - (sprite.Height/2);
                }
            }
        }

        public void UpdateColl(ICollider pCollider) // Test des collisions
        {
            if(isMoving)
            {
                if (CF.IsColliding(rBall, pCollider.GetCollRect()))
                {
                    if (pCollider is Brick)
                    {
                        pCollider.ManageLife();
                        if (pos.X <= pCollider.GetPosition().X)
                        {
                            spd.X *= -1;
                            pos.X -= sprite.Width / 4;
                        }
                        else if (pos.X >= pCollider.GetPosition().X + pCollider.GetCollRect().Width)
                        {
                            spd.X *= -1;
                            pos.X += sprite.Width / 4;
                        }
                        else if (pos.Y <= pCollider.GetPosition().Y)
                        {
                            spd.Y *= -1;
                            pos.Y -= sprite.Height / 4;
                        }
                        else if (pos.Y >= pCollider.GetPosition().Y + pCollider.GetCollRect().Height)
                        {
                            spd.Y *= -1;
                            pos.Y += sprite.Height / 4;
                        }
                    }

                    if (pCollider is Paddle)
                    {
                        Vector2 paddleLoc = pCollider.GetPosition();
                        Rectangle paddleRect = pCollider.GetCollRect();

                        float ballDist = CF.Dist2(pos, paddleLoc);
                        int deg = 10;

                        if (spd.X > 0) // Vient de la gauche
                        {
                            deg = 10;
                        }
                        else // Vient de la droite
                        {
                            deg = -10;
                        }

                        spd.Y *= -1;
                        spd.X = deg * (ballDist / 100);

                        if (pos.X >= paddleLoc.X + (paddleRect.Width / 2))
                        {
                            pos.X += sprite.Width / 2;
                            spd.X *= -1;
                        }
                        else if (pos.X <= paddleLoc.X - (paddleRect.Width / 2))
                        {
                            pos.X -= sprite.Width / 2;
                            spd.X *= -1;
                        }
                    }

                    if (pCollider is Hole)
                    {
                        isMoving = false;
                        ICollider srvPaddle = ServicesLocator.GetService<ICollider>();
                        if (srvPaddle != null && srvPaddle is Paddle)
                        {
                            pos.X = srvPaddle.GetPosition().X;
                            pos.Y = srvPaddle.GetPosition().Y - (sprite.Height / 2);
                            rBall.X = (int)pos.X - (int)origin.X; // Mouvements Rectangle de collision
                            rBall.Y = (int)pos.Y - (int)origin.Y;
                        }


                        Debug.WriteLine("Fin de NIVEAU !! GG !");
                        IMain srvMain = ServicesLocator.GetService<IMain>();
                        if (srvMain != null)
                        {
                            int level;
                            level = srvMain.GetCurrentLevel();
                            srvMain.SetCurrentLevel(level+1);
                            IManager srvManager = ServicesLocator.GetService<IManager>();
                            if (srvManager != null && srvManager is BricksManager)
                            {
                                srvManager.LoadFromJson(level.ToString());
                            }
                        }
                    }
                }
            }
            
        }

        /*public void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sprite, pos, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
            }
        }*/
    }
}
