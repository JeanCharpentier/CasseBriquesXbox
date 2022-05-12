using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IMain
{
    Vector2 GetBounds();
    Texture2D LoadT2D(string pTex);

    SpriteBatch GetSpriteBatch();
}

public interface ICollider
{
    Vector2 GetPosition();
    Rectangle GetCollRect();
}