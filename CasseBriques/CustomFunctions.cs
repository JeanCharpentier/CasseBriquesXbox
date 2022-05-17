using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    //public static class GameServices // N'a pas besoin du new !!! Donc pas a passer en parametre
    public static class CustomFunctions
    {
        public static float Lerp(float a, float b, float factor)
        {
            return (a * (1 - factor)) + (b * factor);
        }

        public static bool IsColliding(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }

        public static float RndFloat(float pMin, float pMax)
        {
            Random rnd = new Random();
            return (float)((rnd.NextDouble()*(pMax - pMin))+pMin);
        }
    }
}
 