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
    public class Hole:ICollider
    {
        public Texture2D sHole;

        public Vector2 pos;
        Vector2 origin;
        public Rectangle rHole;
        public Hole()
        {
            
        }

        public void Init()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                sHole = srvMain.LoadT2D("hole_large_end_alt");
                pos = new Vector2(srvMain.GetBounds().X / 2, srvMain.GetBounds().Y / 2);
                origin = new Vector2(sHole.Width / 2, sHole.Height / 2);
            }
            rHole = new Rectangle((int)pos.X - (int)origin.X, (int)pos.Y - (int)origin.Y, (int)sHole.Width, (int)sHole.Height);
        }

        public void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sHole, pos, null, Color.White, 0, origin, 1.0f, SpriteEffects.None, 0);
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
