using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CasseBriques
{
    public class Animation:Entity
    {
        public Texture2D _spriteSheet;
        public Rectangle _currentFrame;
        float elapsed;
        float frameRate;
        int frame;
        public Animation(string pTexString, int width, int height, Vector2 position):base(pTexString)
        {
            /*IMain srvMain = ServicesLocator.GetService<IMain>();
            if(srvMain != null)
            {
                _spriteSheet = pTexture;
            }*/
            _spriteSheet = sprite;
            scale = 1.5f;
            frameRate = 50f;
            frame = 0;
            _currentFrame = new Rectangle(frame, 0, width, height);
            pos = position;
        }
        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= frameRate)
            {
                if(frame >= 3)
                {
                    //frame = 0; // OK si on loop l'anim
                    IAnim srvAnim = ServicesLocator.GetService<IAnim>();
                    if(srvAnim != null)
                    {
                        srvAnim.StopAnim(this);
                    }
                }else
                {
                    frame++;
                }
                elapsed = 0;
                _currentFrame.X = frame * _currentFrame.Width;
            }
        }
        public override void Draw()
        {
            //base.Draw();

            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(_spriteSheet, pos, _currentFrame, Color.White, 0, origin, scale, SpriteEffects.None, 0);
            }
        }
    }

    public class AnimManager : IAnim
    {
        List<Animation> _animList;
        //Texture2D[] _sheetsList;
        string[] _sheetsList;
        public AnimManager()
        {
            ServicesLocator.AddService<IAnim>(this);
            _animList = new List<Animation>();
            //_sheetsList = new Texture2D[1];
            _sheetsList = new string[1];

            _sheetsList[0] = "explosion";

            /*IImageLoader srvImg = ServicesLocator.GetService<IImageLoader>();
            if (srvImg != null)
            {
                _sheetsList[0] = srvImg.LoadT2D("explosion");
            }*/
        }


        public void Update(GameTime gameTime)
        {
            if(_animList.Count > 0)
            {
                for(int i = _animList.Count - 1; i >= 0; i--)
                {
                    _animList[i].Update(gameTime);
                }
            }
        }
        public void Draw()
        {
            if (_animList.Count > 0)
            {
                foreach (Animation a in _animList)
                {
                    a.Draw();
                }
            }
        }
        public void AddAnimation(int sheetID, int width, int height, Vector2 pos)
        {
            _animList.Add(new Animation(_sheetsList[sheetID],width,height,pos));
            Debug.WriteLine($"Animation créée en {_animList[0].pos}");
        }

        public void StartAnimation(Animation pAnim)
        {
            throw new NotImplementedException();
        }

        public void StopAnim(Animation pAnim)
        {
            _animList.Remove(pAnim);
        }
    }
}
