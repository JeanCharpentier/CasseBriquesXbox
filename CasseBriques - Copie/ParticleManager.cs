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
    public class ParticleManager:IParticle
    {
        List<Particle> _partList;
        Texture2D sprTexture;
        public ParticleManager()
        {
            ServicesLocator.AddService<IParticle>(this);
            _partList = new List<Particle>();
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if(srvMain != null)
            {
                sprTexture = srvMain.LoadT2D("ball_ghost");
            }
        }

        public void Update(GameTime gameTime)
        {
            for(int i = _partList.Count - 1; i >= 0; i--)
            {
                _partList[i].Update(gameTime);
                if(_partList[i].opacity <= 0)
                {
                    _partList.Remove(_partList[i]);
                }
            }
        }

        public void Draw()
        {
            foreach (Particle p in _partList)
            {
                p.Draw();
            }
        }

        public void CreateParticle(Vector2 pos)
        {
            _partList.Add(new Particle(sprTexture, pos));
        }
    }

    public class Particle:Entity
    {
        private float elapsed;
        private float frameRate;
        public float opacity;
        public Particle(Texture2D pTexture, Vector2 position):base(pTexture)
        {
            frameRate = 20f;
            opacity = 1f;
            pos = position;
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= frameRate)
            {
                if (opacity > 0f)
                {
                    opacity -= 0.1f;
                }
                elapsed = 0;
            }
        }

        public override void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sprite, pos, null, Color.White * opacity, 0, origin, scale, SpriteEffects.None, 0);
            }
        }
    }
}
