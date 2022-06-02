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
                sBG[0] = srvMain.LoadT2D("background_green"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBG[1] = srvMain.LoadT2D("background_blue"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBG[2] = srvMain.LoadT2D("background_grey"); // Chargée à chaque nouvelle brique !!! A REVOIR !
                sBG[3] = srvMain.LoadT2D("background_brown"); // Chargée à chaque nouvelle brique !!! A REVOIR !
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
