﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasseBriques;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public interface IMain
{
    Vector2 GetBounds();
    Texture2D LoadT2D(string pTex);
    SpriteBatch GetSpriteBatch();
    void Shake(float pRadius);
    void QuitGame();
    void LaunchGame(bool pBool);
}

public interface ICollider
{
    Vector2 GetPosition();
    Rectangle GetCollRect();
    bool ManageLife();
}
public interface IHole
{
    bool GetState();
    void SetState(bool pState);
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

public interface ILevel
{
    int GetCurrentLevel();
    void SetCurrentLevel(int pLevel);
}

public interface IInput
{

}

public interface IAnim
{
    void AddAnimation(int sheetId,int width, int height, Vector2 pos);
    void StartAnimation(Animation pAnim);
    void StopAnim(Animation pAnim);
}

public interface IParticle
{
    void CreateParticle(Vector2 pos);
}

public interface IVisee
{
    void Rotate(float pAngle);
    Vector2 GetAngle();
}