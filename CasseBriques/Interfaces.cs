﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasseBriques;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface IMain
{
    Vector2 GetBounds();
    Texture2D LoadT2D(string pTex);
    SpriteBatch GetSpriteBatch();

    int GetCurrentLevel();

    void SetCurrentLevel(int pLevel);
}

public interface ICollider
{
    Vector2 GetPosition();
    Rectangle GetCollRect();

    bool ManageLife();

   // bool DeleteMe();
}

public interface IManager
{
    bool DeleteObject(ICollider pCollider);
    void LoadFromJson(string pJsonFile);
}

public interface IJson
{
    JsonManager ReadJson(string pFile);

}