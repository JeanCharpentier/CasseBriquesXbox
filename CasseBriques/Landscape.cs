using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CasseBriques
{
    public class Landscape
    {
        private Texture2D[] sBG;
        private Vector2 bounds;
        public Landscape()
        {
            sBG = new Texture2D[4];
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                bounds = srvMain.GetBounds();
            }

            IImageLoader srvImg = ServicesLocator.GetService<IImageLoader>();
            if (srvImg != null)
            {
                sBG[0] = srvImg.LoadT2D("background_green");
                sBG[1] = srvImg.LoadT2D("background_blue");
                sBG[2] = srvImg.LoadT2D("background_grey");
                sBG[3] = srvImg.LoadT2D("background_brown");
            }
        }
        public void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (srvMain != null)
            {
                Vector2 pos = new Vector2(0, 0);
                for(int i=0;i<=bounds.X;i+=64)
                {
                    for(int j = 0; j <= bounds.Y; j += 64)
                    {
                        srvMain.GetSpriteBatch().Draw(sBG[0], pos, null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        pos.Y = j;
                    }
                    pos.X = i;
                }
            }
        }
    }
}
