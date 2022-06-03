using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CasseBriques
{
    public class Ball:Entity
    {
        public Vector2 oldPos;
        private Vector2 spd;
        private Vector2 base_spd;

        private Laser _laser;

        private float elapsed;
        private float spawnrate;


        private bool isMoving;

        public Rectangle rBall;

        public Ball(string pTexString):base(pTexString)
        {
            isMoving = false;
            base_spd = new Vector2(2, -8);
            IImageLoader srvImg = ServicesLocator.GetService<IImageLoader>();
            if(srvImg != null)
            {
                sprite = srvImg.LoadT2D(pTexString);
                _laser = new Laser("laser");
            }
            pos = new Vector2((bounds.X / 2) - 20, bounds.Y - 200);
            spd = base_spd;
            rBall = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            spawnrate = 50f;
            /*IMain srvMain = ServicesLocator.GetService<IMain>();
            if(srvMain != null)
            {
                _laser = new Laser(srvMain.LoadT2D("laser"));
            }*/
            
        }
        public void Update(GameTime gameTime)
        {
            if (isMoving)
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsed >= spawnrate)
                {
                    IParticle srvPart = ServicesLocator.GetService<IParticle>();
                    if(srvPart != null)
                    {
                        srvPart.CreateParticle(pos);
                    }
                    elapsed = 0;
                }
                
                if (pos.X >= bounds.X - (sprite.Width / 2 )|| pos.X <= sprite.Width/2)
                {
                    spd.X *= -1;
                }
                if (pos.Y <= sprite.Height / 2)
                {
                    spd.Y *= -1;
                }
                if(pos.Y >= bounds.Y - (sprite.Height / 2))
                {
                    isMoving = false;
                    spd = base_spd;
                }
                pos += spd;

                rBall.X = (int)pos.X - (int)origin.X; // Mouvements Rectangle de collision
                rBall.Y = (int)pos.Y - (int)origin.Y;

            }else if(!isMoving && (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space)))
            {
                spd = _laser.GetAngle();
                isMoving = true;
            }else if (!isMoving)
            {
                _laser.Update();
                ICollider srvPaddle = ServicesLocator.GetService<ICollider>();
                if(srvPaddle != null && srvPaddle is Paddle)
                {
                    pos.X = srvPaddle.GetPosition().X;
                    pos.Y = srvPaddle.GetPosition().Y - (sprite.Height);
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
                        Rectangle brickRect = pCollider.GetCollRect();

                        if(CF.CollideLeft(rBall, brickRect, spd) || CF.CollideRight(rBall, brickRect, spd)) // Modifier la fonction pour sortir un INT pour "Switch Case"
                        {
                            spd.X *= -1;
                        }
                        if (CF.CollideTop(rBall, brickRect, spd) || CF.CollideBottom(rBall, brickRect, spd))
                        {
                            spd.Y *= -1;
                        }
                        pCollider.ManageLife(); // Gère la vie et la mort de la brique
                    }

                    if (pCollider is Paddle)
                    {
                        Vector2 paddleLoc = pCollider.GetPosition();
                        Rectangle paddleRect = pCollider.GetCollRect();

                        float ballDist = CF.Dist2(pos, paddleLoc);
                        int deg;

                        if (CF.CollideTop(rBall, paddleRect, spd))
                        {
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
                        }
                        if(CF.CollideLeft(rBall, paddleRect, spd) || CF.CollideRight(rBall, paddleRect,spd))
                        {
                            spd *= -1;
                        }
                    }

                    if (pCollider is Hole)
                    {
                        IHole srvHole = ServicesLocator.GetService<IHole>();
                        if(srvHole.GetState())
                        {
                            isMoving = false;
                            spd = base_spd;
                            ICollider srvPaddle = ServicesLocator.GetService<ICollider>();
                            if (srvPaddle != null && srvPaddle is Paddle)
                            {
                                pos.X = srvPaddle.GetPosition().X;
                                pos.Y = srvPaddle.GetPosition().Y - (sprite.Height / 2);
                                rBall.X = (int)pos.X - (int)origin.X; // Mouvements Rectangle de collision
                                rBall.Y = (int)pos.Y - (int)origin.Y;
                            }

                            ILevel srvLevel = ServicesLocator.GetService<ILevel>();
                            if (srvLevel != null)
                            {
                                srvLevel.SetCurrentLevel(0);
                            }
                            srvHole.SetState(false);
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            if (!isMoving)
            {
                _laser.Draw();
            }
            base.Draw();
        }
    }
}
