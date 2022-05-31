// Fonctions persos !
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    public static class CF // N'a pas besoin du new !!! Donc pas a passer en parametre
    {
        public static float Lerp(float a, float b, float factor)
        {
            return (a * (1 - factor)) + (b * factor);
        }
        public static float RndFloat(float pMin, float pMax)
        {
            Random rnd = new Random();
            return (float)((rnd.NextDouble()*(pMax - pMin))+pMin);
        }
        public static float Dist2(Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a,b);
        }

        // Gestion des collisions
        public static bool IsColliding(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }
        public static bool CollideLeft(Rectangle a, Rectangle b, Vector2 vel)
        {
            return a.Right + vel.X > b.Left && a.Left < b.Left && a.Bottom > b.Top && a.Top < b.Bottom;
        }
        public static bool CollideRight(Rectangle a, Rectangle b, Vector2 vel)
        {
            return a.Left + vel.X < b.Right && a.Right > b.Right && a.Bottom > b.Top && a.Top < b.Bottom;
        }
        public static bool CollideTop(Rectangle a, Rectangle b, Vector2 vel)
        {
            return a.Bottom + vel.Y > b.Top && a.Top < b.Top && a.Right > b.Left && a.Left < b.Right;
        }
        public static bool CollideBottom(Rectangle a, Rectangle b, Vector2 vel)
        {
            return a.Top + vel.Y < b.Bottom && a.Bottom > b.Bottom && a.Right > b.Left && a.Left < b.Right;
        }
    }
}
 