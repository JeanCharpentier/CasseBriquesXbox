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
    public class Bricks : ICollider
    {
        public Texture2D sBrique;

        public Vector2 pos;
        public Vector2 origin;

        public Rectangle rBrique;

        public Bricks() {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                this.sBrique = srvMain.LoadT2D("button_yellow");
            }else
            {
                Trace.WriteLine("!!! Echec de chargement de l'image de brique");
            }

            origin = new Vector2(0, 0);
            
        }

        public void SetCollRect()
        {
            rBrique = new Rectangle((int)pos.X, (int)pos.Y, sBrique.Width, sBrique.Height);
        }

        public void Update()
        {

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

        public bool DeleteMe()
        {
            Trace.WriteLine("DELETED !");

            return true;
        }
    }

    public class BricksManager : IManager
    {
        public List<Bricks> _bricksList;
        public BricksManager()
        {
            _bricksList = new List<Bricks>();
            ServicesLocator.AddService<IManager>(this);
        }

        public void Load()
        {
            for (int i=0;i<9;i++)
            {
                Bricks b = new Bricks();
                b.pos = new Vector2(100 + (128 * i), 100);
                b.SetCollRect();
                this._bricksList.Add(b);    
            }
        }


        public void Update()
        {

        }
        public void Draw()
        {
            foreach(Bricks b in _bricksList)
            {
                IMain srvMain = ServicesLocator.GetService<IMain>();
                if (srvMain != null)
                {
                    srvMain.GetSpriteBatch().Draw(b.sBrique, b.pos, null, Color.White, 0, b.origin, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        public bool DeleteObject(ICollider pCollider)
        {
            for (int i = _bricksList.Count()-1; i >= 1; i--)
            {
                if(_bricksList[i].pos == pCollider.GetPosition())
                {
                    Trace.WriteLine("Brique trouvée !");
                    _bricksList.Remove(_bricksList[i]);
                }
            }
            return true;
        }
    }
}
