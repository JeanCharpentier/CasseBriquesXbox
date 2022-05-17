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
    public class Brick : ICollider
    {
        public Texture2D sBrick;

        public Vector2 pos;
        public Vector2 origin;

        public Rectangle rBrick;

        public Brick() {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                this.sBrick = srvMain.LoadT2D("button_yellow"); // Chargée à chaque nouvelle brique !!! A REVOIR !
            }else
            {
                Trace.WriteLine("!!! Echec de chargement de l'image de brique");
            }

            origin = new Vector2(0, 0);
            
        }

        public void SetCollRect()
        {
            rBrick = new Rectangle((int)pos.X, (int)pos.Y, sBrick.Width, sBrick.Height);
            Trace.WriteLine("Rect Brick : " + rBrick);
        }

        public void Update()
        {

        }
        public void Draw()
        {

        }

        public Rectangle GetCollRect()
        {
            return rBrick;
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
        public List<Brick> _bricksList;
        public BricksManager()
        {
            _bricksList = new List<Brick>();
            ServicesLocator.AddService<IManager>(this);
        }

        public void Load()
        {
            for (int i=0;i<9;i++)
            {
                Brick b = new Brick();
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
            foreach(Brick b in _bricksList)
            {
                IMain srvMain = ServicesLocator.GetService<IMain>();
                if (srvMain != null)
                {
                    srvMain.GetSpriteBatch().Draw(b.sBrick, b.pos, null, Color.White, 0, b.origin, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        public bool DeleteObject(ICollider pCollider)
        {
            for (int i = _bricksList.Count-1; i >= 0; i--)
            {
                if(_bricksList[i].pos == pCollider.GetPosition())
                {
                    Trace.WriteLine("Brique trouvée à : "+pCollider.GetPosition());
                    _bricksList.Remove(_bricksList[i]);
                }
            }
            return true;
        }
    }
}
