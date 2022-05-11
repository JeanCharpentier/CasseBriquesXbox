using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    public class GameServices
    {
        private Vector2 bounds;
        public Game theGame;

        public Paddle paddle;
        public Ball ball;

        public Vector2 GetBounds()
        {
            bounds = new Vector2(this.theGame.GraphicsDevice.Viewport.Width, this.theGame.GraphicsDevice.Viewport.Height);
            return bounds;
        }

        public void SetGame(Game pGame)
        {
            theGame = pGame;
        }

        public void SetPaddle(Paddle pPaddle)
        {
            paddle = pPaddle;
        }

        public void SetBall(Ball pBall)
        {
            ball = pBall;
        }

        public float Lerp(float a, float b, float factor)
        {
            return (a * (1 - factor)) + (b * factor);
        }

        public bool IsColliding(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }
    }
}
 