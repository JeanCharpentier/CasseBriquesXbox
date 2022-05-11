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

        public Vector2 GetBounds()
        {
            bounds = new Vector2(this.theGame.GraphicsDevice.Viewport.Width, this.theGame.GraphicsDevice.Viewport.Height);
            return bounds;
        }

        public void SetGame(Game pGame)
        {
            theGame = pGame;
        }
    }
}
 