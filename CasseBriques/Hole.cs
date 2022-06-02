using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace CasseBriques
{
    public class Hole:Entity,ICollider,IHole
    {
        public Rectangle rHole;
        public Texture2D closedSprite;

        public bool isOpen;
        public Hole(Texture2D pTexture):base(pTexture)
        {
            ServicesLocator.AddService<IHole>(this);
            pos = new Vector2(bounds.X / 2, 200 - (sprite.Height / 4));
            rHole = new Rectangle((int)pos.X-(int)origin.X, (int)pos.Y-(int)origin.Y, (int)sprite.Width, (int)sprite.Height);
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if(srvMain != null)
            {
                closedSprite = srvMain.LoadT2D("hole_large_end_alt_locked");
            }
            isOpen = false;
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

        public override void Draw()
        {
            if (isOpen)
            {
                base.Draw();
            }else
            {
                IMain srvMain = ServicesLocator.GetService<IMain>();
                if (srvMain != null)
                {
                    srvMain.GetSpriteBatch().Draw(closedSprite, pos, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
                }
            }
            
        }

        public bool GetState()
        {
            return isOpen;
        }

        public void SetState(bool pState)
        {
            isOpen = pState;
        }
    }
}
