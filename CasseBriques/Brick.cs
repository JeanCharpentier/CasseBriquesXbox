using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CasseBriques
{
    public class BricksManager : IManager
    {
        public List<Brick> _bricksList;

        public Texture2D[] _spritelist;

        public List<Brick> _spooler; // Liste des briques a supprimer !

        public BricksManager()
        {
            _bricksList = new List<Brick>();
            _spooler = new List<Brick>();
            ServicesLocator.AddService<IManager>(this);
        }

        public void LoadFromJson(string pJsonFile)
        {
            _bricksList.Clear();
            IJson srvJson = ServicesLocator.GetService<IJson>();
            string[] lignes = srvJson.ReadJson("Levels/level"+pJsonFile+".json").map.Split('/');
            int lines = 0;
            foreach (var line in lignes)
            {
                int cols = 0;
                string[] columns = line.Split(',');
                foreach (string id in columns)
                {
                    Brick b;
                    switch (id)
                    {
                        case "0":
                            b = null;
                            break;
                        case "1":
                            b = new Violet(_spritelist[0]);
                            break;
                        case "2":
                            b = new Indigo(_spritelist[1]);
                            break;
                        case "3":
                            b = new Blue(_spritelist[2]);
                            break;
                        case "4":
                            b = new Green(_spritelist[3]);
                            break;
                        case "5":
                            b = new Yellow(_spritelist[4]);
                            break;
                        case "6":
                            b = new Orange(_spritelist[5]);
                            break;
                        case "7":
                            b = new Red(_spritelist[6]);
                            break;
                        case "8":
                            b = new Grey(_spritelist[7]);
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
                    }
                    cols++;

                }
                lines++;
            }
        }
        public void Load()
        {
            _spritelist = new Texture2D[8];
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                _spritelist[0] = srvMain.LoadT2D("button_violet");
                _spritelist[1] = srvMain.LoadT2D("button_indigo"); 
                _spritelist[2] = srvMain.LoadT2D("button_blue"); 
                _spritelist[3] = srvMain.LoadT2D("button_green"); 
                _spritelist[4] = srvMain.LoadT2D("button_yellow"); 
                _spritelist[5] = srvMain.LoadT2D("button_orange"); 
                _spritelist[6] = srvMain.LoadT2D("button_red");
                _spritelist[7] = srvMain.LoadT2D("button_grey");
            }
            else
            {
                Trace.WriteLine("!!! Echec de chargement de l'image de brique");
            }
            LoadFromJson("1");
        }

        public void Update()
        {
            foreach (Brick b in _bricksList)
            {
                
                if (_bricksList.Count <= 0)
                {
                    IHole srvHole = ServicesLocator.GetService<IHole>();
                    if (srvHole != null)
                    {
                        srvHole.SetState(true);
                    }
                }
                b.sprite = _spritelist[b.sprNum];
                b.Update();
            }
        }
        public void Draw()
        {
            foreach(Brick b in _bricksList)
            {
                if (b.sprNum < 0)
                {
                    b.sprNum = 0;
                }
                b.Draw();
            }
        }

        public bool DeleteObject(ICollider pCollider)
        {
            float colPosX = pCollider.GetPosition().X;
            float colPosY = pCollider.GetPosition().Y;

            colPosX = MathF.Floor((colPosX-64) / 128);
            colPosY = MathF.Floor((colPosY-20) / 53);

            Vector2 colPos = new Vector2(colPosX, colPosY);

            if (pCollider is Red)
            {
                for (int i = _bricksList.Count-1; i >= 0; i--)
                {
                    if (_bricksList[i].gridPosition.X == colPosX && _bricksList[i].gridPosition.Y == colPosY - 1)
                    {
                        if(_bricksList[i] is Grey) // Limitation C# 7.3 sinon "is not" en C# 9.0+
                        {

                        }else
                        {
                            _spooler.Add(_bricksList[i]);
                        }
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX - 1 && _bricksList[i].gridPosition.Y == colPosY)
                    {
                        if (_bricksList[i] is Grey) // Limitation C# 7.3 sinon "is not" en C# 9.0+
                        {

                        }
                        else
                        {
                            _spooler.Add(_bricksList[i]);
                        }
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX && _bricksList[i].gridPosition.Y == colPosY)
                    {
                        if (_bricksList[i] is Grey) // Limitation C# 7.3 sinon "is not" en C# 9.0+
                        {

                        }
                        else
                        {
                            _spooler.Add(_bricksList[i]);
                        }
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX + 1 && _bricksList[i].gridPosition.Y == colPosY) 
                    {
                        if (_bricksList[i] is Grey) // Limitation C# 7.3 sinon "is not" en C# 9.0+
                        {

                        }
                        else
                        {
                            _spooler.Add(_bricksList[i]);
                        }
                    }
                    else if (_bricksList[i].gridPosition.X == colPosX && _bricksList[i].gridPosition.Y == colPosY + 1)
                    {
                        if (_bricksList[i] is Grey) // Limitation C# 7.3 sinon "is not" en C# 9.0+
                        {

                        }
                        else
                        {
                            _spooler.Add(_bricksList[i]);
                        }
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



    public abstract class Brick : Entity,ICollider
    {
        public Rectangle rBrick;

        public int life;
        public int sprNum;

        public Vector2 gridPosition;
        public Vector2 startPosition;

        public bool isFalling;


        public Brick(Texture2D pSprite):base(pSprite)
        {
            isFalling = false;
            
        }


        public void Update()
        {
            if (isFalling)
            {
                pos.Y += 9;
                rBrick.Y = (int)bounds.Y + 300;
            }
            if(pos.Y > bounds.Y + 100)
            {
                IManager srvBricks = ServicesLocator.GetService<IManager>(); // Détruit la brique
                srvBricks.DeleteObject(this);
            }
        }
        public void SetCollRect()
        {
            rBrick = new Rectangle((int)pos.X - (int)origin.X, (int)pos.Y - (int)origin.Y, sprite.Width, sprite.Height); ;
            startPosition = pos; // Garde une trace de la position de dpart
        }

        public Rectangle GetCollRect()
        {
            return rBrick;
        }

        public Vector2 GetPosition()
        {
            return startPosition;
        }

        public virtual bool ManageLife()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            IManager srvManager = ServicesLocator.GetService<IManager>();
            if (life > 0)
            {
                life--;
                sprNum--;
                if (srvMain != null)
                {
                    srvMain.Shake(2.0f);
                }
            }
            else
            {
                if(this is Red)
                {
                    if (srvMain != null)
                    {
                        srvMain.Shake(8.0f);
                    }
                    if(srvManager != null)
                    {
                        srvManager.DeleteObject(this);
                    }
                }
                else
                {
                    if (srvMain != null)
                    {
                        srvMain.Shake(4.0f);
                    }
                    isFalling = true;
                }
            }
            return true;
        }
    }
    public class Yellow : Brick
    {
        public Yellow(Texture2D pTexture) : base(pTexture)
        {
            life = 4;
            sprNum = life;
        }
    }

    public class Blue : Brick
    {
        public Blue(Texture2D pTexture) : base(pTexture)
        {
            life = 2;
            sprNum = life;
        }
    }

    public class Violet : Brick
    {
        public Violet(Texture2D pTexture) : base(pTexture)
        {
            life = 0;
            sprNum = life;
        }
    }

    public class Indigo : Brick
    {
        public Indigo(Texture2D pTexture) : base(pTexture)
        {
            life = 1;
            sprNum = life;
        }
    }

    public class Green : Brick
    {
        public Green(Texture2D pTexture) : base(pTexture)
        {
            life = 3;
            sprNum = life;
        }
    }

    public class Orange : Brick
    {
        public Orange(Texture2D pTexture) : base(pTexture)
        {
            life = 5;
            sprNum = life;
        }
    }

    public class Red : Brick // Briques explosives
    {
        public Red(Texture2D pTexture) : base(pTexture)
        {
            life = 0;
            sprNum = 6;
        }
    }

    public class Grey : Brick // Murs incassables
    {
        public Grey(Texture2D pTexture) : base(pTexture)
        {
            life = 0;
            sprNum = 7;
        }

        public override bool ManageLife()
        {
            return true;
        }
    }


}