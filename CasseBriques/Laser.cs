using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace CasseBriques
{
    public class Laser:Entity,IVisee
    {
        public float angle;
        public Laser(string pTexString):base(pTexString)
        {
            ServicesLocator.AddService<IVisee>(this);
            origin.X = 0;
            angle = (3*MathF.PI)/2;
        }
        public void Update()
        {
            ICollider srvPaddle = ServicesLocator.GetService<ICollider>();
            if (srvPaddle != null && srvPaddle is Paddle)
            {
                pos.X = srvPaddle.GetPosition().X;
                pos.Y = srvPaddle.GetPosition().Y - (sprite.Height) - 16;
            }
        }
        public override void Draw()
        {
            IMain srvMain = ServicesLocator.GetService<IMain>();
            if (!srvMain.GetGameOver())
            {
                srvMain.GetSpriteBatch().Draw(sprite, pos, null, Color.White, angle, origin, scale, SpriteEffects.None, 0);
            }
        }
        public void Rotate(float pAngle)
        {
            if(angle-pAngle >= (MathF.PI/16)+MathF.PI && angle-pAngle <= (31*MathF.PI)/16)
            {
                angle -= pAngle;
                if (angle >= 2 * MathF.PI)
                {
                    angle -= 2 * MathF.PI;
                }
            }
        }

        public Vector2 GetAngle()
        {
            return new Vector2(MathF.Cos(angle) * (2 * MathF.PI), MathF.Sin(angle) * (2 * MathF.PI));
        }
    }
}
