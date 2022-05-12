using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    //public static class GameServices // N'a pas besoin du new !!! Donc pas a passer en parametre
    public static class GameServices
    {
        public static float Lerp(float a, float b, float factor)
        {
            return (a * (1 - factor)) + (b * factor);
        }

        public static bool IsColliding(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }
    }
}
 