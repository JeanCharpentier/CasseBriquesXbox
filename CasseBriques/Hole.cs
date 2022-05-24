using Microsoft.Xna.Framework;
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
            pos = new Vector2(bounds.X / 2, bounds.Y / 2);
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            rHole = new Rectangle((int)pos.X - (int)origin.X, (int)pos.Y - (int)origin.Y, (int)sprite.Width, (int)sprite.Height);
        }

        public void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sprite, pos, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
            }
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
