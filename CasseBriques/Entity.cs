using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CasseBriques
{
    public class Entity
    {
        public Texture2D sprite;

        public Vector2 pos;
        public Vector2 origin;
        public Vector2 bounds;
        public float scale;
        public Entity(Texture2D pSprite)
        {
            IMain srvScreen = ServicesLocator.GetService<IMain>();
            if (srvScreen != null)
            {
                bounds = srvScreen.GetBounds();
            }
            else
            {
                bounds = new Vector2(1920, 1080);
            }
            sprite = pSprite;
            scale = 1.0f;
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }
        public virtual void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                srvMain.GetSpriteBatch().Draw(sprite, pos, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
            }
        }
    } 
}
