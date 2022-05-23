using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CasseBriques
{
    public class Brick : ICollider
    {
        public Texture2D[] sBrick;

        public Vector2 pos;
        public Vector2 origin;

        public Rectangle rBrick;

        public int life;
        public int sprite;

        public Vector2 gridPosition;


        public Brick() {

            sBrick = new Texture2D[7];
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                sBrick[0] = srvMain.LoadT2D("button_yellow"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[1] = srvMain.LoadT2D("button_blue"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[2] = srvMain.LoadT2D("button_grey"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[3] = srvMain.LoadT2D("button_darkgrey"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[4] = srvMain.LoadT2D("button_green"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[5] = srvMain.LoadT2D("button_pink"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBrick[6] = srvMain.LoadT2D("button_red"); // Chargée à chaque nouvelle brique !!! A REVOIR !
            }
            else
            {
                Trace.WriteLine("!!! Echec de chargement de l'image de brique");
            }

            origin = new Vector2(0, 0);
            life = 0; // Points de vie - 1
        }


        public void Update()
        {
           //sprite = life;
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
        public bool ManageLife()
        {
            if(life > 0)
            {
                life--;
                sprite--;
            }else
            {
                IManager srvBricks = ServicesLocator.GetService<IManager>(); // Détruit la brique
                srvBricks.DeleteObject(this);
            }

            return true;
        }
    }

    public class BricksManager : IManager
    {
        public List<Brick> _bricksList;
        public string[] level;

        public List<Brick> _spooler; // Liste des briques a supprimer !

        public BricksManager()
        {
            _bricksList = new List<Brick>();
            _spooler = new List<Brick>();
            ServicesLocator.AddService<IManager>(this);
        }

        public void Load()
        {
            level = File.ReadAllLines("Levels/level1.txt");
            int lines = 0;
            foreach(var line in level)
            {
                int cols = 0;
                string[] columns = line.Split(',');
                foreach(string id in columns)
                {
                    Brick b;
                    switch(id)
                    {
                        case "0":
                            b = null;
                            break;
                        case "1":
                            b = new Yellow();
                            break;
                        case "2":
                            b = new Blue();
                            break;
                        case "3":
                            b = new Grey();
                            break;
                        case "4":
                            b = new DarkGrey();
                            break;
                        case "5":
                            b = new Green();
                            break;
                        case "6":
                            b = new Pink();
                            break;
                        case "7":
                            b = new Red();
                            break;
                        default:
                            b = null;
                            break;
                    }
                    if (b != null)
                    {
                        b.pos = new Vector2(64 + (128 * cols), 20 + (53 * lines));
                        b.SetCollRect();
                        b.gridPosition = new Vector2(cols, lines);
                        _bricksList.Add(b);
                        //Trace.WriteLine("Brique créée : " + b.gridPosition.ToString());
                    }
                    cols++;
                    
                }
                lines++;
            }
        }

        public void Update()
        {
            foreach(Brick b in _bricksList)
            {
                b.Update();
                //b.sprite = b.life;
            }


        }
        public void Draw()
        {
            foreach(Brick b in _bricksList)
            {

                IMain srvMain = ServicesLocator.GetService<IMain>();
                if (srvMain != null)
                {
                    srvMain.GetSpriteBatch().Draw(b.sBrick[b.sprite], b.pos, null, Color.White, 0, b.origin, 1.0f, SpriteEffects.None, 0);
                }
            }
        }

        public bool DeleteObject(ICollider pCollider)
        {
            float colPosX = pCollider.GetPosition().X;
            float colPosY = pCollider.GetPosition().Y;

            colPosX = MathF.Floor((colPosX - 64) / 128);
            colPosY = MathF.Floor((colPosY - 20) / 53);

            Vector2 colPos = new Vector2(colPosX, colPosY);

            if (pCollider is Red)
            {
                
                for (int i = _bricksList.Count-1; i >= 0; i--)
                {
                    if (_bricksList[i].gridPosition.X == colPosX && _bricksList[i].gridPosition.Y == colPosY - 1)
                    {
                        _spooler.Add(_bricksList[i]);
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX - 1 && _bricksList[i].gridPosition.Y == colPosY)
                    {
                        _spooler.Add(_bricksList[i]);
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX && _bricksList[i].gridPosition.Y == colPosY)
                    {
                        _spooler.Add(_bricksList[i]);
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX + 1 && _bricksList[i].gridPosition.Y == colPosY) 
                    {
                        _spooler.Add(_bricksList[i]);
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX && _bricksList[i].gridPosition.Y == colPosY + 1)
                    {
                        _spooler.Add(_bricksList[i]);
                    }
                }

            }
            else
            {
                for (int i = _bricksList.Count - 1; i >= 0; i--)
                {
                    if (_bricksList[i].gridPosition == colPos)
                    {
                        _spooler.Add(_bricksList[i]);
                    }
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
            sprite = life;
        }
    }

    public class Blue : Brick
    {
        public Blue()
        {
            life = 1;
            sprite = life;
        }
    }

    public class Grey : Brick
    {
        public Grey()
        {
            life = 2;
            sprite = life;
        }
    }

    public class DarkGrey : Brick
    {
        public DarkGrey()
        {
            life = 3;
            sprite = life;
        }
    }

    public class Green : Brick
    {
        public Green()
        {
            life = 4;
            sprite = life;
        }
    }

    public class Pink : Brick
    {
        public Pink()
        {
            life = 5;
            sprite = life;
        }
    }

    public class Red : Brick
    {
        public Red()
        {
            life = 0;
            sprite = 6;
        }
    }


}