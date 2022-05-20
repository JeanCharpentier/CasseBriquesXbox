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
        public Texture2D[] sBrick;

        public Vector2 pos;
        public Vector2 origin;

        public Rectangle rBrick;

        public int life;

        public Brick() {

            sBrick = new Texture2D[3];
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                sBrick[0] = srvMain.LoadT2D("button_yellow"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[1] = srvMain.LoadT2D("button_blue"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[2] = srvMain.LoadT2D("button_grey"); // Chargée à chaque nouvelle brique !!! A REVOIR !
            }
            else
            {
                Trace.WriteLine("!!! Echec de chargement de l'image de brique");
            }

            origin = new Vector2(0, 0);
            life = 2; // Points de vie - 1
        }

        public void SetCollRect()
        {
            rBrick = new Rectangle((int)pos.X, (int)pos.Y, sBrick[0].Width, sBrick[0].Height);
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

        public int GetLife()
        {
            return life;
        }

        public bool SetLife(int pLife)
        {
            life = pLife;
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
            for (int i=0;i<5;i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    Brick b = new Blue();
                    b.pos = new Vector2(100 + (200 * i), 100+ (100 * j));
                    b.SetCollRect();
                    this._bricksList.Add(b);
                }    
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
                    srvMain.GetSpriteBatch().Draw(b.sBrick[b.life], b.pos, null, Color.White, 0, b.origin, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        public bool DeleteObject(ICollider pCollider)
        {
            for (int i = _bricksList.Count-1; i >= 0; i--)
            {
                if(_bricksList[i].pos == pCollider.GetPosition())
                {
                    if(_bricksList[i] is Blue) // Test des explosions de briques
                    {
                        Trace.WriteLine("Boom !");
                    }
                    _bricksList.Remove(_bricksList[i]);
                }
            }
            return true;
        }
    }

    public class Yellow : Brick
    {
        public Yellow()
        {
            life = 0;
        }
    }

    public class Blue : Brick
    {
        public Blue()
        {
            life = 1;
        }
    }

    public class Grey : Brick
    {
        public Grey()
        {
            life = 2;
        }
    }


}