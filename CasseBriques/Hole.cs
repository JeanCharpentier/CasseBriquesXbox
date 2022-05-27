﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class Hole:Entity,ICollider
    {
        public Rectangle rHole;
        public Hole(Texture2D pTexture):base(pTexture)
        {
            
        }

        public void Init()
        {
            pos = new Vector2(bounds.X / 2, 200-(sprite.Height/4));
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            rHole = new Rectangle((int)pos.X - (int)origin.X, (int)pos.Y - (int)origin.Y, (int)sprite.Width, (int)sprite.Height);
        }

        public Rectangle GetCollRect()
        {
            return rHole;
        }

        public Vector2 GetPosition()
        {
            return pos;
        }

        public bool ManageLife()
        {
            throw new NotImplementedException();
        }
    }
}
