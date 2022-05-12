using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasseBriques
{
    public class Briques : ICollider
    {
        public Texture2D sBrique;

        public Vector2 pos;
        public Vector2 size;

        private Rectangle rBrique;

        public Briques() {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                this.sBrique = srvMain.LoadT2D("button_yellow");
            }else
            {
                Trace.WriteLine("!!! Echec de chargement de l'image de brique");
            }

            size = new Vector2(sBrique.Width / 2, sBrique.Height / 2);
            
        }

        public void SetCollRect()
        {
            rBrique = new Rectangle((int)pos.X, (int)pos.Y, sBrique.Width, sBrique.Height);
        }
        public void Draw()
        {

        }

        public Rectangle GetCollRect()
        {
            return rBrique;
        }

        public Vector2 GetPosition()
        {
            return pos;
        }
    }

    public class BriquesManager
    {
        public Briques[] briquesList;
        public BriquesManager()
        {
            briquesList = new Briques[10];
        }

        public void Load()
        {
            for (int i=0;i<10;i++)
            {
                Briques b = new Briques();
                b.pos = new Vector2(100 + (128 * i), 100);
                b.SetCollRect();
                this.briquesList[i] = b;
            }
        }

        public void Draw()
        {
            foreach(Briques b in briquesList)
            {
                IMain srvMain = ServicesLocator.GetService<IMain>();
                if (srvMain != null)
                {
                    srvMain.GetSpriteBatch().Draw(b.sBrique, b.pos, null, Color.White, 0, b.size, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

    }
}
