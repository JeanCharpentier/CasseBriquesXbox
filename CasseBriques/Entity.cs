﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class Entity
    {
        public Texture2D sprite;

        public Vector2 pos;
        public Vector2 origin;
        public Vector2 bounds;
        public Entity(Texture2D pSprite)
        {
            IMain srvScreen = ServicesLocator.GetService<IMain>();
            if (srvScreen != null)
            {
                bounds = srvScreen.GetBounds();
            }
            else
            {
                bounds = new Vector2(1280, 720);
            }
            sprite = pSprite;
        }
        public virtual void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sprite, pos, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    } 
}
