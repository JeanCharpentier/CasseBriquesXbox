using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CasseBriques
{
    public class ImageLoader:IImageLoader
    {
        private Game _game;
        public ImageLoader(Game pGame)
        {
            ServicesLocator.AddService<IImageLoader>(this);
            _game = pGame;
            _game.Content.RootDirectory = "Content";
        }

        public Texture2D LoadT2D(string pTex)
        {
            return _game.Content.Load<Texture2D>(pTex);
        }
    }
}
