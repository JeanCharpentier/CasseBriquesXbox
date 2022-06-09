using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CasseBriques
{
    public class BricksManager : IManager
    {
        public List<Brick> _bricksList;
        public List<Brick> _staticList;

        public string[] _sprStrings;
        private Texture2D[] _sprTex;

        public List<Brick> _spooler; // Liste des briques a supprimer !

        public BricksManager()
        {
            _bricksList = new List<Brick>();
            _staticList = new List<Brick>();
            _spooler = new List<Brick>();
            ServicesLocator.AddService<IManager>(this);
        }

        public void LoadFromJson(string pJsonFile)
        {
            _bricksList.Clear();
            _staticList.Clear();
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
                            b = new Violet(_sprStrings[0]);
                            break;
                        case "2":
                            b = new Indigo(_sprStrings[1]);
                            break;
                        case "3":
                            b = new Blue(_sprStrings[2]);
                            break;
                        case "4":
                            b = new Green(_sprStrings[3]);
                            break;
                        case "5":
                            b = new Yellow(_sprStrings[4]);
                            break;
                        case "6":
                            b = new Orange(_sprStrings[5]);
                            break;
                        case "7":
                            b = new Red(_sprStrings[6]);
                            break;
                        case "8":
                            b = new Grey(_sprStrings[7]);
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
                        if(b is Grey)
                        {
                            _staticList.Add(b);
                        }else
                        {
                            _bricksList.Add(b);
                        }
                        
                    }
                    cols++;

                }
                lines++;
            }
        }
        public void Load()
        {
            _sprStrings = new string[8];
            _sprStrings[0] = "button_violet";
            _sprStrings[1] = "button_indigo";
            _sprStrings[2] = "button_blue";
            _sprStrings[3] = "button_green";
            _sprStrings[4] = "button_yellow";
            _sprStrings[5] = "button_orange";
            _sprStrings[6] = "button_red";
            _sprStrings[7] = "button_grey";

            // Double tableau que c'est moche
            _sprTex = new Texture2D[8]; // Tableau de textures pour changer au runtime selon la vie des briques
            IImageLoader srvImg = ServicesLocator.GetService<IImageLoader>();
            if (srvImg != null)
            {
                _sprTex[0] = srvImg.LoadT2D(_sprStrings[0]);
                _sprTex[1] = srvImg.LoadT2D(_sprStrings[1]);
                _sprTex[2] = srvImg.LoadT2D(_sprStrings[2]);
                _sprTex[3] = srvImg.LoadT2D(_sprStrings[3]);
                _sprTex[4] = srvImg.LoadT2D(_sprStrings[4]);
                _sprTex[5] = srvImg.LoadT2D(_sprStrings[5]);
                _sprTex[6] = srvImg.LoadT2D(_sprStrings[6]);
                _sprTex[7] = srvImg.LoadT2D(_sprStrings[7]);
            }
            LoadFromJson("1");
        }

        public void Update()
        {
            
            foreach (Brick b in _bricksList)
            {
                if(_bricksList.Count <= 0) //il n’y a plus de briques dans la liste des briques destructibles
                {
                    IHole srvHole = ServicesLocator.GetService<IHole>(); //Le BrickManager appelle le service IHole
                    if (srvHole != null)
                    {
                        srvHole.SetState(true); //Le BrickManager demande au service IHole de changer son état
                    }
                }
                b.sprite = _sprTex[b.sprNum];
                b.Update();
            }
            foreach(Brick s in _staticList)
            {
                s.Update();
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
            foreach (Brick s in _staticList)
            {
                s.Draw();
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
                IAnim srvAnim = ServicesLocator.GetService<IAnim>();
                if (srvAnim != null)
                {
                    srvAnim.AddAnimation(0, 40, 40, new Vector2(pCollider.GetPosition().X+(pCollider.GetCollRect().Width/2), pCollider.GetPosition().Y));
                }

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


        public Brick(string pTexString):base(pTexString)
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
        public Yellow(string pTexString):base(pTexString)
        {
            life = 4;
            sprNum = life;
        }
    }

    public class Blue : Brick
    {
        public Blue(string pTexString) : base(pTexString)
        {
            life = 2;
            sprNum = life;
        }
    }

    public class Violet : Brick
    {
        public Violet(string pTexString) : base(pTexString)
        {
            life = 0;
            sprNum = life;
        }
    }

    public class Indigo : Brick
    {
        public Indigo(string pTexString) : base(pTexString)
        {
            life = 1;
            sprNum = life;
        }
    }

    public class Green : Brick
    {
        public Green(string pTexString) : base(pTexString)
        {
            life = 3;
            sprNum = life;
        }
    }

    public class Orange : Brick
    {
        public Orange(string pTexString) : base(pTexString)
        {
            life = 5;
            sprNum = life;
        }
    }

    public class Red : Brick // Briques explosives
    {
        public Red(string pTexString) : base(pTexString)
        {
            life = 0;
            sprNum = 6;
        }
    }

    public class Grey : Brick // Murs incassables
    {
        public Grey(string pTexString) : base(pTexString)
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